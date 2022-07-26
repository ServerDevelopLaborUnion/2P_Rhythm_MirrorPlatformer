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
  switch(data.t) {
    case 'start':
      gameList[socket.game].forEach(client => {
        client.send(JSON.stringify(data));
      }); 
      break;
    case 'input':
      gameList[socket.game].forEach(client => {
        if(client.id != socket.id) 
          client.send(JSON.stringify(data));
      });
      break;
    case 'dead':
      gameList[socket.game].forEach(client => {
        // if(socket.id != client.id) {
          client.send(JSON.stringify(data));
        // }
      })
      break;
    default:
      gameList[socket.game].forEach(client => {
        client.send(JSON.stringify(data));
      });
      break;
  }
}

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const RoomData = function(data, socket) {
  switch(data.t) {
    case 'create':
      gameList[data.v] = [];
      gameList[data.v].push(socket);
      socket.game = data.v;
      console.log(
        `client ${socket.id} create room. name : ${socket.game}`
      );
      wss.clients.forEach(client => {
        if(socket.id != client.id) 
          client.send(JSON.stringify(data));
      });
      break;
    case 'join':
      try {
        if(gameList[data.v] == undefined) 
          throw new Error('game not found');
        gameList[data.v].push(socket);
        socket.game = data.v;
        console.log(
          `client ${socket.id} join room. name : ${socket.game}`
        );
        gameList[data.v].forEach(client => {
          if(client.id != socket.id)
            client.send(JSON.stringify(data));
        });
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : 'room', t : 'join', v : err.toString()
        }));
      }
      break;
    case 'exit':
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
}