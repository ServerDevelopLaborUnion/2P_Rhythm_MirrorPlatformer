const ws = require('ws');
const wss = new ws.Server({port:3001});

var userList = [];

wss.on('connection', (socket, req) => {
    userList.push(socket);
    socket.on('message', (msg) => {
        if(msg.toString() == 'find') {
            socket.send(userList.indexOf(socket).toString());
        }
        else if(msg.toString() == 'quit') {
            userList.splice(
                userList.indexOf(socket), userList.indexOf(socket)
                );
        }
    })
});