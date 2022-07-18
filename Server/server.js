const ws = require('ws');
const wss = new ws.Server({ port: 3000 });
let idcnt = 0;

wss.on('listening', () => {
  console.log(`server opened on port ${wss.options.port}`);
});

wss.on('connection', (socket) => {
  socket.id = idcnt;
  socket.on('message', (msg) => {
    const data = JSON.parse(msg);
    if(data?.err) return;
    switch (data?.locate) {
      case 'room':
        roomData(data, socket);
        break;
      case 'game':
        gameData(data, socket);
        break;
    }
  });
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
  switch(data.type) {
    case 'value':
      console.log(data.value);
      break;
  }
}