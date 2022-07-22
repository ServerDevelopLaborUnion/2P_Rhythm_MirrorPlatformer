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
      case 'room':
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
    case 'create':
      gameList[data.v] = [].push(socket);
      socket.game = data.v;
      socket.send(JSON.stringify({
        l : "room", t : "create", v : true
      }));
      wsServer.clients.forEach(client => {
        if(client.id != socket.id) client.send(JSON.stringify({
          l : "room", t : "create", v : data.v
        }));
      });
      break;
    case 'join':
      try {
        gameList[data.v].forEach(client => {
          client.send(JSON.stringify({
            l : "room", t : "join", v : data.v
          }));
        });
        socket.game = data.v
        gameList[data.v].push(socket);
        socket.send(JSON.stringify({
          l : "room", t : "join", v: true
        }));
      }
      catch(err) {
        socket.send(JSON.stringify({
          l : "room", t : "join", v : false
        }));
      }
      break;
    case 'start':
      // 대충 씬을 넘기라는 거를 보내는 코드
      break;
  }
}

/**
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
const GameData = function(data, socket) {
  var result = {
    l : "game", t : data.t, v : data.v
  }
  switch(data.t) {
    case 'input':
      gameList[socket.game].forEach(client => {
        if(client.id != socket.id) {
          client.send(JSON.stringify(result));
        }
      });
      break;
    case 'system':
      gameList[socket.game].forEach(client => {
        client.send(JSON.stringify(result));
      });
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