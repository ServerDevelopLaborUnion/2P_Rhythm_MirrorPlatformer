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
    client.send(JSON.stringify({
      l : 'room', t : 'init', v : name
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
    }
  });
});

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const GameData = function(data, socket) {
  if(socket.game == undefined) return;
  BroadCast(gameList[socket.game], true, socket, JSON.stringify(data));
  // switch(data.t) {
  //   case 'start':
  //     gameList[socket.game].forEach(client => {
  //       client.send(JSON.stringify(data));
  //     }); 
  //     break;
  //   case 'input':
  //     gameList[socket.game].forEach(client => {
  //       if(client.id != socket.id) 
  //         client.send(JSON.stringify(data));
  //     });
  //     break;
  //   case 'dead':
  //     gameList[socket.game].forEach(client => {
  //       // if(socket.id != client.id) {
  //         client.send(JSON.stringify(data));
  //       // }
  //     });
  //     break;
  //   default:
  //     gameList[socket.game].forEach(client => {
  //       client.send(JSON.stringify(data));
  //     });
  //     break;
  // }
}

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const RoomData = function(data, socket) {
  switch(data.t) {
    case 'create':
      try {
        if(gameList[data.v] != undefined) 
          throw new Error('game name already on server');
        gameList[data.v] = [];
        gameList[data.v].push(socket);
        socket.game = data.v;
        console.log(
          `client ${socket.id} create room. name : ${socket.game}`
        );
        BroadCast(wss.clients, false, socket, JSON.stringify(data));
        // wss.clients.forEach(client => {
        //   if(socket.id != client.id) 
        //     client.send(JSON.stringify(data));
        // });
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : 'room', t : 'create', v : err.toString()
        }));
      }
      break;
    case 'join':
      try {
        if(gameList[data.v] == undefined) 
          throw new Error('game not found');
        else if(gameList[data.v].length >= 2)
          throw new Error('the room is full');
        gameList[data.v].push(socket);
        socket.game = data.v;
        console.log(
          `client ${socket.id} join room. name : ${socket.game}`
        );
        BroadCast(gameList[data.v], false, socket, JSON.stringify(data));
        // gameList[data.v].forEach(client => {
        //   if(client.id != socket.id)
        //     client.send(JSON.stringify(data));
        // });
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : 'room', t : 'join', v : err.toString()
        }));
      }
      break;
    case 'quit':
        if(gameList[socket.game].indexOf(socket) == 0) {
          gameList[socket.game].forEach(client => {
            client.send(JSON.stringify(data));
            if(socket.id != client.id) 
              client.game = undefined;
          });
          gameList[socket.game] = undefined;
        }
        else if(gameList[socket.game].indexOf(socket) == 1) {
          gameList[socket.game].splice(
            gameList[socket.game].indexOf(socket),
            gameList[socket.game].indexOf(socket)
          );
          BroadCast(gameList[socket.game], true, socket, JSON.stringify(data));
          // gameList[socket.game].forEach(client => {
          //   client.send(JSON.stringify(data));
          // });
          console.log(
            `client ${socket.id} quit room. name : ${socket.game}`
          );
        }
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