const ws = require('ws');
const wss = new ws.Server({ port: 3000 });
let idcnt = 0;

wss.on('listening', () => {
  console.log(`server opened on port ${wss.options.port}`);
});

wss.on('connection', (socket, req) => {
  socket.id = idcnt;
  console.log(`client connect. ip : ${req.socket.remoteAddress.split(':')[3]}`);
  socket.on('message', (msg) => {
    const data = JSON.parse(msg);
    if(data?.err) console.log(msg);
    switch (data?.locate) {
      case 'room':
        roomData(data, socket);
        break;
      case 'game':
        gameData(data, socket);
        break;
      case 'dev':
        // for debug
        break;
    }
  });
  socket.on('close', () => {
    console.log(`client off. ip : ${req.socket.remoteAddress.split(':')[3]}`);
  })
});

/**
 * @param {string} type the type of data
 * @param {Object} data Message changed to JavaScript Object
 * @param {ws.WebSocket} socket WebSocket which send message
 */
function roomData(data, socket) {
  switch(data.type) {

  }
}

function gameData(data, socket) {
  var result = {
    type : null,
    id : socket.id,
    err : false
  };
  switch(data.type) {
    case 'jump':
      result.type = 'jump';
      wss.clients.forEach(soc => {
        if(soc.id != socket.id) 
          soc.send(JSON.stringify(result));
      });
      break;
  }
}