const ws = require('ws');
const wss = new ws.Server({port:3000});

let idcnt = 0;
var gameList = {};

wss.on('listening', () => {
  console.log(`server opened on port ${wss.options.port}`);
});

wss.on('connection', (client, req) => {
  client.id = idcnt++;
  Object.keys(gameList).forEach(name => {
    if(gameList[name] != undefined) 
      client.send(JSON.stringify({
        l : 'room', t : 'create', v : { n : name, p : gameList[name][0].pwd }
      }));
  });
  client.on('message', msg => {
    const Data = ObjectParser(msg.toString());
    if(Data == false) return;
    switch(Data.l) {
      case 'room':
        RoomData(Data, client);
        break;
      case 'game':
        GameData(Data, client);
        break;
      case 'dev':
        client.send(JSON.stringify(gameList));
        break;
    }
  });
  client.on('close', () => {
    if(client.game == undefined) return;
    if(client.game != undefined) {
      gameList[client.game].forEach(socket => {
        if(socket.id != client.id) {
          socket.send(JSON.stringify({
            l : 'room', t : 'quit', v : client.game
          }));
          socket.game = undefined;
        }
      });
      BroadCast(wss.clients, true, client, JSON.stringify({
        l : 'room', t : 'roomDel', v : client.game
      }));
      gameList[client.game] = undefined;
    }
  })
});

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const GameData = function(data, socket) {
  if(socket.game == undefined) return;
  if(data.t == 'input') {
    BroadCast(gameList[socket.game], false, socket, JSON.stringify(data));
  }
  else if(data.t == 'finish') {
    socket.finish = true;
    gameList[socket.game].forEach(client => {
      if((socket.id != client.id) && (client.finish)) {
        BroadCast(gameList[socket.game], true, socket, JSON.stringify(data));
        gameList[socket.game].forEach(soc => {
          soc.finish = false;
        });
      }
    });
  }
  else {
    BroadCast(gameList[socket.game], true, socket, JSON.stringify(data));
  }
}

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const RoomData = function(data, socket) {
  switch(data.t) {
    case 'createReq':
      try {
        if((gameList[data.v] != undefined) || (data.v == "") || (data.v == null))
          throw new Error('Failed Creating Room');
        gameList[data.v.n] = [];
        gameList[data.v.n].push(socket);
        socket.game = data.v.n;
        socket.pwd  = data.v.p;
        console.log(
          `client ${socket.id} create room. name : ${socket.game}`
        );
        BroadCast(wss.clients, false, socket, JSON.stringify({
          l : 'room', t : 'create', v : data.v
        }));
        socket.send(JSON.stringify({
          l : 'room', t : 'createRes', v : data.v
        }));
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : 'room', t : 'err', v : err.toString()
        }));
      }
      break;
    case 'joinReq':
      try {
        if(gameList[data.v] == undefined) 
          throw new Error('Game not found');
        else if(gameList[data.v].length >= 2)
          throw new Error('The room is full');
        gameList[data.v].push(socket);
        socket.game = data.v;
        console.log(
          `client ${socket.id} join room. name : ${socket.game}`
        );
        BroadCast(gameList[data.v], false, socket, JSON.stringify({
          l : 'room', t : 'join', v : socket.id
        }));
        socket.send(JSON.stringify({
          l : 'room', t : 'joinRes', v : data.v
        }));
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : 'room', t : 'err', v : err.toString()
        }));
      }
      break;
    case 'quitReq':
      if(socket.game == undefined) return;
      BroadCast(gameList[socket.game], false, socket, JSON.stringify({
        l : 'room', t : 'quit', v : socket.game
      }));
      socket.send(JSON.stringify({ l : 'room', t : 'quitRes', v : socket.game }));
      BroadCast(wss.clients, true, socket, JSON.stringify({
        l : 'room', t : 'roomDel', v : socket.game
      }));
      console.log(
        `client ${socket.id} quit room. name : ${socket.game}`
      );
      gameList[socket.game].forEach(client => {
        if(socket.pwd != undefined) socket.pwd = undefined;
        if(client.id != socket.id) client.game = undefined;
      });
      gameList[socket.game] = undefined;
      socket.game = undefined;
      break;
  }
}

/**
 * @param {string} stringData 
 * @returns {object}
 */
const ObjectParser = function(stringData) {
  try {
    return JSON.parse(stringData);
  }
  catch(err) {
    return false;
  }
};

/**
 * @param {Array} array 
 * @param {boolean} sendSelf 
 * @param {ws.WebSocket} socket 
 * @param {string} data 
 */
const BroadCast = function(array, sendSelf, socket, data) {
  if(sendSelf) {
    array.forEach(client => {
      client.send(data);
    });
  }
  else {
    array.forEach(client => {
      if(client.id != socket.id) {
        client.send(data);
      }
    });
  }
};