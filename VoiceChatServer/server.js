const fs = require("fs");
const express = require("express");
const https = require("https");
const app = express();
const socket = require("socket.io");

const server = https.createServer({
    key: fs.readFileSync("./sertifications/combatanalysis.voicechat.key.pem"),
    cert: fs.readFileSync("./sertifications/combatanalysis.voicechat.cert.pem")
}, app);

const io = socket(server, {
    cors: {
        origin: "*"
    }
});

const port = 2000;
const ip = "192.168.0.161";
const roomSize = 5;

const users = {};
const chatUsers = {};
const socketToRoom = {};

io.on("connection", socket => {
    socket.on("updateSocketId", payload => {
        const me = users[payload.roomId]?.filter(user => user.socketId === payload.socketId)[0];

        socket.id = me?.socketId;
        socket.emit("socketIdUpdated", me?.socketId);
    });

    socket.on("checkUsersOnCall", roomId => {
        if (chatUsers[roomId] === undefined) {
            chatUsers[roomId] = [socket.id];
        }
        else {
            chatUsers[roomId].push(socket.id);
        }
        
        const count = users[roomId] === undefined ? 0 : users[roomId].length;

        chatUsers[roomId].forEach(chatUserId => {
            io.to(chatUserId).emit("checkedUsersOnCall", count);
        });
    });

    socket.on("removeUserFromChat", roomId => {
        if (chatUsers[roomId] === undefined) {
            return;
        }

        const index = chatUsers[roomId].indexOf(socket.id);
        chatUsers[roomId].splice(index, 1);
    });

    socket.on("joinToRoom", joinedUser => {
        const user = { 
            socketId: socket.id,
            userId: joinedUser.userId,
            username: joinedUser.username,
            turnOnCamera: joinedUser.turnOnCamera,
            turnOnMicrophone: joinedUser.turnOnMicrophone,
        };

        if (users[joinedUser.roomId]) {
            const length = users[joinedUser.roomId].length;
            if (length === roomSize) {
                socket.emit("roomFull");
                return;
            }
            
            const userAlreadyJoined = users[joinedUser.roomId].filter(user => user.userId === joinedUser.userId);
            if (userAlreadyJoined.length === 0) {
                users[joinedUser.roomId].push(user);
            }
        } else {
            users[joinedUser.roomId] = [user];
        }

        socketToRoom[socket.id] = joinedUser.roomId;
        const usersInThisRoom = users[joinedUser.roomId].filter(user => user.userId !== joinedUser.userId);

        socket.emit("allUsers", usersInThisRoom);
        if (chatUsers[joinedUser.roomId] === undefined) {
            return;
        }
        
        chatUsers[joinedUser.roomId].forEach(chatUserId => {
            io.to(chatUserId).emit("checkedUsersOnCall", users[joinedUser.roomId].length);
        });
    });

    socket.on("sendingSignal", payload => {
        const userToSignalData = users[payload.roomId].filter(user => user.socketId === payload.userToSignal.socketId)[0];

        io.to(payload.userToSignal.socketId).emit("userJoined", { 
            signal: payload.signal,
            callerId: payload.callerId,
            username: payload.username,
            turnOnCamera: userToSignalData.turnOnCamera,
            turnOnMicrophone: userToSignalData.turnOnMicrophone,
        });
    });

    socket.on("returningSignal", payload => {
        io.to(payload.callerId).emit("receivingReturnedSignal", { 
            signal: payload.signal,
            id: socket.id,
        });
    });

    socket.on("cameraSwitching", cameraData => {
        const me = users[cameraData.roomId]?.filter(user => user.socketId === socket.id)[0];
        if (me === undefined) {
            return;
        }

        me.turnOnCamera = cameraData.cameraStatus;

        const usersInThisRoom = users[cameraData.roomId].filter(user => user.socketId !== socket.id);

        usersInThisRoom.forEach(user => {
            io.to(user.socketId).emit("cameraSwitched", cameraData.cameraStatus);
        });
    });

    socket.on("microphoneSwitching", microphoneData => {
        const me = users[microphoneData.roomId]?.filter(user => user.socketId === socket.id)[0];
        if (me === undefined) {
            return;
        }
        
        me.turnOnMicrophone = microphoneData.microphoneStatus;

        const usersInThisRoom = users[microphoneData.roomId].filter(user => user.socketId !== socket.id);

        usersInThisRoom.forEach(user => {
            io.to(user.socketId).emit("microphoneSwitched", microphoneData.microphoneStatus);
        });
    });

    socket.on("leavingFromRoom", leavingUser => {
        const roomUsers = users[leavingUser.roomId];
        if (roomUsers === undefined) {
            return;
        }

        const targetUser = roomUsers.filter(user => user.socketId === socket.id)[0];
        const targetUserIndex = users[leavingUser.roomId].indexOf(targetUser);

        users[leavingUser.roomId].splice(targetUserIndex, 1);

        if (users[leavingUser.roomId].indexOf(targetUser) >= 0) {
            return;
        }

        socket.emit("userLeft");

        users[leavingUser.roomId].forEach(element => {
            io.to(element.socketId).emit("someUserLeft", targetUser?.socketId);

            const usersInThisRoom = users[leavingUser.roomId].filter(user => user.socketId !== element.socketId);
            io.to(element.socketId).emit("allUsers", usersInThisRoom);
        });

        chatUsers[leavingUser.roomId]?.forEach(chatUserId => {
            io.to(chatUserId).emit("checkedUsersOnCall", users[leavingUser.roomId].length);
        });
    });

    socket.on("disconnect", () => {
        const roomId = socketToRoom[socket.id];
        let room = users[roomId];

        if (room) {
            room = room.filter(id => id !== socket.id);
            users[roomId] = room;
        }
    });
});

server.listen(port, ip, () => console.log(`Server is running on: ${ip}:${port}`));
