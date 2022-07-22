const ws = require('ws');
const wsServer = new ws.Server({port : 3000});

let idcnt = 0;
var form = { l : null, t : null, v : null};
var roomList = [];

wsServer.on('listening', () => {
  console.log(`server opened on port ${wss.options.port}`);
});

wsServer.on('connection', (client, req) => {
  client.id = idcnt++;
  client.on('message', (msg) => {
    const Data = checkJSON(msg.toString());
    if(Data == false) return;
    switch(Data.l) {
      case 'game':
        break;
      case 'room':
        RoomData(client, Data);
        break;
    }
  });
});

/**
 * @param {ws.WebSocket} socket 
 * @param {object} data 
 */
const GameData = function(socket, data) {
  
}

/**
 * @param {ws.WebSocket} socket 
 * @param {object} data 
 */
const RoomData = function(socket, data) {
  switch(data.t) {
    case 'make':
      roomList.push(new Room(data.v.n, socket.id));
      break;
    case 'join':
      break;
  }
}

class Room {
  /**
   * @param {string} n 
   * @param {Int16Array} i 
   */
  constructor(n, i) {
    this.name = n;
    this.hostID = i;
    this.roomInfo = { 
      l : 'room', t : 'make', 
      v : { n : this.name, i : this.hostID }
    };
    wsServer.clients.forEach(user => {
      if(user.id == this.hostID) {
        user.send(JSON.stringify({
          l : 'room', t : 'make', v : true
        }));
      }
      else {
        user.send(JSON.stringify(this.roomInfo));
      }
    });
  }
  addUser(socket) {
    this.otherID = socket.id;
  }
  removeUser(socket) {
    this.otherID = null;
  }
}

/**
 * @param {string} jsonData
 * @param {object} ParsedData
 */
const checkJSON = (jsonData) => {
  try {
    return JSON.parse(jsonData);
  }
  catch {
    return false;
  }
}