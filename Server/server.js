const ws = require('ws');
const wss = new ws.Server({ port: 3000 });

let idcnt = 0;
var roomList = new Array();

wss.on('listening', () => {
  console.log(`server opened on port ${wss.options.port}`);
});

wss.on('connection', (socket, req) => {
  socket.id = idcnt++;
  socket.on('message', (msg) => {
    const data = JSON.parse(msg);
    switch(data.l) {
      case 'game':
        GameData(data, socket);
        break;
      case 'error':
        return;
    }
  });
});

/**
 * 
 * @param {object} data 
 * @param {ws.WebSocket} socket 
 */
function GameData(data, socket) {
  var result = {
    l : 'game',
    t : data.t,
    v : data.v
  };
  var stringData = JSON.stringify(result);
  switch(data.t) {
    case 'input':
      BroadCast(false, stringData, socket);
      break;
    case 'system':
      BroadCast(true, stringData, socket);
      break;
    // case 'error':
    //   break;
  }
}

/**
 * @param {object} data
 * @param {ws.WebSocket} socket 
 */
function RoomData(data, socket) {
  var result = {
    l : 'room',
    t : null,
    v : null
  }
  var dataToSocket = {};
  var dataToUsers = {};
  switch(data.t) {
    case 'make':
      break;
    case 'join':
      break;
  }
}

class Room {
  /**
   * @param {string} name 
   * @param {Int16Array} id
   */
  constructor(name, id) {
    this.name = name
    this.hostID = id;
    this.userList = [];
    this.roomInfo = {
      n : this.name,
      i : this.hostID
    }
  }
  
  /**
   * @param {ws.WebSocket} socket 
   */
  addMember(socket) {
    if(this.userList.length >= 2)
      socket.send(JSON.stringify({
        l : 'room',
        t : 'error',
        v : 'overflow member'
      }));
    this.userList.push(socket.id);
  }
  rmMember(socket) {
    
  }

}

/**
 * @param {boolean} sendSelf 
 * @param {string} data 
 * @param {ws.WebSocket} socket
 */
function BroadCast(sendSelf, data, socket) {
  if(sendSelf) {
    wss.clients.forEach(socket => {
      socket.send(data);
    });
  }
  else {
    wss.clients.forEach(soc => {
      if(soc.id != socket.id) soc.send(data);
    });
  }
}