const ws = require('ws');
const wss = new ws.Server({ port: 3000 });
let idcnt = 0;

wss.on('connection', () => {
  console.log(`server opened on port ${wss.options.port}`);
});

wss.on('connection', (socket, req) => {
  socket.id = idcnt++;
  socket.on('message', (msg) => {
    const data = JSON.parse(msg);
    console.log(data.payload);
    var result = {
      type: null,
      payload: null
    };
    switch (data.type) {
      case 'jump':
        result.type = 'jump';
        result.payload = 'this is JumpData';
        wss.clients.forEach(soc => {
          if(soc.id != socket.id)
            soc.send(JSON.stringify(result));
        });
        break;
      case 'slide':
        result.type = 'slide';
        result.payload = 'this is SlideData';
        wss.clients.forEach(soc => {
          if (soc.id != socket.id)
            soc.send(JSON.stringify(result));
        });
        break;
      case 'error':
        result.type = 'error';
        result.payload = 'throw error on other';
        wss.clients.forEach(soc => {
          if (soc.id != socket.id)
            soc.send(JSON.stringify(result));
        });
        break;
    }
  });
});