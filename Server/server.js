const ws = require('ws');
const wsServer = new ws.Server({port: 3000});

let idcnt = 0;
var gameList = {};

wsServer.on('listening', () => {
  console.log(`server opened on port ${wsServer.options.port}`);
});

wsServer.on('connection', (client, req) => {
  client.id = idcnt++;
  client.on('message', (msg) => {
    const Data = JSONCheck(msg.toString());
    if(Data == null) return;
    switch(Data.l) {
      case 'lobby':
        LobbyData(Data, client);
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
const LobbyData = function(data, socket) {
  switch(data.t) {
    case 'make':
      gameList[data.v] = [].push(socket);
      socket.send(JSON.stringify({
        l : "lobby", t : "make", v : true
      }));
      wsServer.clients.forEach(client => {
        if(client.id != socket.id) client.send(JSON.stringify({
          l : "lobby", t : "make", v : data.v
        }));
      });
      break;
    case 'join':
      try {
        gameList[data.v].forEach(client => {
          client.send(JSON.stringify({
            l : "lobby", t : "join", v : data.v
          }));
        });
        gameList[data.v].push(socket.id);
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : "lobby", t : "join", v : false
        }));
      }
      break;
    case 'start':
      break;
  }
}

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const GameData = function(data, socket) {
  switch(data.t) {
    case 'input':
      break;
    case 'system':
      break;
  }
}

const JSONCheck = function(stringData) {
  try {
    return JSON.parse(stringData);
  }
  catch(error) {
    return false;
  }
}