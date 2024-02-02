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
const socketToRoom = {};

io.on("connection", socket => {
    socket.on("joinToRoom", joinedUser => {
        if (users[joinedUser.roomId]) {
            const length = users[joinedUser.roomId].length;
            if (length === roomSize) {
                socket.emit("roomFull");
                return;
            }
            
            const userAlreadyJoined = users[joinedUser.roomId].filter(user => user.userId === joinedUser.userId);
            if (userAlreadyJoined.length === 0) {
                users[joinedUser.roomId].push({ socketId: socket.id, userId: joinedUser.userId, username: joinedUser.username });
            }
        } else {
            users[joinedUser.roomId] = [{ socketId: socket.id, userId: joinedUser.userId, username: joinedUser.username }];
        }

        socketToRoom[socket.id] = joinedUser.roomId;
        const usersInThisRoom = users[joinedUser.roomId].filter(user => user.socketId !== socket.id);

        socket.emit("allUsers", usersInThisRoom);
    });

    socket.on("sendingSignal", payload => {
        io.to(payload.userToSignal.socketId).emit("userJoined", { signal: payload.signal, callerId: payload.callerId, username: payload.userToSignal.username });
    });

    socket.on("returningSignal", payload => {
        io.to(payload.callerId).emit("receivingReturnedSignal", { signal: payload.signal, id: socket.id });
    });

    socket.on("leaveFromRoom", leavingUser => {
        const roomUsers = users[leavingUser.roomId];

        const targetUser = roomUsers.filter(user => user.socketId === socket.id)[0];
        const targetUserIndex = users[leavingUser.roomId].indexOf(targetUser);

        users[leavingUser.roomId].splice(targetUserIndex, 1);

        if (users[leavingUser.roomId].indexOf(targetUser) < 0) {
            socket.emit("userLeft");

            users[leavingUser.roomId].forEach(element => {
                const usersInThisRoom = users[leavingUser.roomId].filter(user => user.socketId !== element.socketId);
                io.to(element.socketId).emit("allUsers", usersInThisRoom);
            });
        }
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
