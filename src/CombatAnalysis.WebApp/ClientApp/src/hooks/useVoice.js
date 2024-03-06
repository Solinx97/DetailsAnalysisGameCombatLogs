import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import io from 'socket.io-client';

const useVoice = (me, callMinimazedData, setUseMinimaze) => {
	const socketRef = useRef(null);
	const peersRef = useRef([]);
	const videoRef = useRef(null);

	const navigate = useNavigate();

	const [turnOnCamera, setTurnOnCamera] = useState(false);
	const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
	const [screenSharing, setScreenSharing] = useState(false);
	const [myStream, setMyStream] = useState(null);
	const [renderRoomId, setRenderRoomId] = useState(0);
	const [renderChatName, setRenderChatName] = useState("");
	const [peers, setPeers] = useState([]);

	const [microphoneDeviceId, setMicrophoneDeviceId] = useState("");

	const { roomId, chatName } = useParams();

	const [anotherUsersAudio, setAnotherUsersAudio] = useState([]);

	useEffect(() => {
		if (videoRef.current === null) {
			return;
		}

		videoRef.current.srcObject = myStream;
	}, [myStream]);

	const initConnection = () => {
		socketRef.current = io.connect("192.168.0.161:2000");

		socketRef.current.on("connect", () => {
			if (callMinimazedData.current.roomId > 0) {
				cameFromMinimazedCall();
			}
			else {
				callMinimazedData.current.socketId = socketRef.current.id;
			}

			callMinimazedData.current.roomId = roomId;
			callMinimazedData.current.roomName = chatName;

			setRenderRoomId(roomId);
			setRenderChatName(chatName);
		});
	}

	const cameFromMinimazedCall = () => {
		setUseMinimaze(false);

		socketRef.current.emit("updateSocketId", { roomId: callMinimazedData.current.roomId, socketId: callMinimazedData.current.socketId });

		socketRef.current.on("socketIdUpdated", socketId => {
			socketRef.current.id = socketId;

			callMinimazedData.current.socketId = socketId;

			switchCallType();
		});
	}

	const switchCallType = () => {
		setMyStream(callMinimazedData.current.stream);
		setPeers(callMinimazedData.current.peers);

		peersRef.current = callMinimazedData.current.peers;

		setTurnOnCamera(callMinimazedData.current.turnOnCamera);
		setTurnOnMicrophone(callMinimazedData.current.turnOnMicrophone);
		setRenderRoomId(callMinimazedData.current.roomId);
	}

	const createPeer = (userToSignal, callerId, stream) => {
		const peer = new window.SimplePeer({
			initiator: true,
			trickle: false,
			stream,
		});

		peer.on("signal", signal => {
			socketRef.current.emit("sendingSignal", { userToSignal, callerId, signal, username: me.username, roomId: renderRoomId });
		});

		return peer;
	}

	// when:
	//		1) user connected to call and on call don't present another users: need add peer for users, that will connect later
	//		2) user connected to call and on call present another users: need add peer for users, that will connect later
	const addPeer = (incomingSignal, callerId, stream) => {
		const peer = new window.SimplePeer({
			initiator: false,
			trickle: false,
			stream,
		});

		peer.on("signal", signal => {
			socketRef.current.emit("returningSignal", { signal, callerId });
		});

		peer.signal(incomingSignal);

		return peer;
	}

	const switchCamera = (cameraStatus) => {
		setTurnOnCamera(cameraStatus);

		callMinimazedData.current.turnOnCamera = cameraStatus;

		if (!cameraStatus) {
			myStream.getVideoTracks()[0].stop();
			socketRef.current.emit("cameraSwitching", { roomId: renderRoomId, cameraStatus });

			return;
		}

		navigator.mediaDevices.getUserMedia({ video: cameraStatus, audio: turnOnMicrophone })
			.then(stream => {
				peersRef.current.forEach(peerRef => {
					const peerStream = peerRef.peer.streams[0];
					peerRef.peer.replaceTrack(peerStream.getVideoTracks()[0], stream.getVideoTracks()[0], peerStream);
				});

				callMinimazedData.current.stream = stream;
				setMyStream(stream);

				socketRef.current.emit("cameraSwitching", { roomId: renderRoomId, cameraStatus });
			})
			.catch((e) => {
				setTurnOnCamera(false);
				callMinimazedData.current.turnOnCamera = false;

				console.log(e);
			});
	}

	const switchMicrophone = (microphoneStatus) => {
		setTurnOnMicrophone(microphoneStatus);

		callMinimazedData.current.turnOnMicrophone = microphoneStatus;

		if (!microphoneStatus) {
			myStream.getAudioTracks()[0].stop();
			socketRef.current.emit("microphoneSwitching", { roomId: renderRoomId, microphoneStatus });

			return;
		}

		const constraints = {
			video: turnOnCamera,
			audio: microphoneStatus
		};

		if (microphoneDeviceId !== "") {
			constraints.audio = { deviceId: microphoneDeviceId };
		}

		navigator.mediaDevices.getUserMedia(constraints)
			.then(stream => {
				peersRef.current.forEach(peerRef => {
					const peerStream = peerRef.peer.streams[0];
					peerRef.peer.replaceTrack(peerStream.getAudioTracks()[0], stream.getAudioTracks()[0], peerStream);
				});

				callMinimazedData.current.stream = stream;
				setMyStream(stream);

				socketRef.current.emit("microphoneSwitching", { roomId: renderRoomId, microphoneStatus });
			})
			.catch((e) => {
				setTurnOnMicrophone(false);
				callMinimazedData.current.turnOnMicrophone = false;

				console.log(e);
			});
	}

	const switchMicrophoneDevice = (deviceId) => {
		navigator.mediaDevices.getUserMedia({ video: turnOnCamera, audio: { deviceId: deviceId } }).then(stream => {
			peersRef.current.forEach(peerRef => {
				const peerStream = peerRef.peer.streams[0];
				peerRef.peer.replaceTrack(peerStream.getAudioTracks()[0], stream.getAudioTracks()[0], peerStream);
			});

			callMinimazedData.current.stream = stream;
			setMyStream(stream);
		});
	}

	const switchAudioOutputDevice = (deviceId) => {
		for (let i = 0; i < anotherUsersAudio.length; i++) {
			anotherUsersAudio[i].setSinkId(deviceId);
		}
	}

	const shareScreen = (screenStatus) => {
		setScreenSharing(screenStatus);
		callMinimazedData.current.screenSharing = screenStatus;

		if (!screenStatus) {
			myStream.getVideoTracks()[0].stop();

			socketRef.current.emit("screenSharingSwitching", { roomId: renderRoomId, screenStatus });

			return;
		}

		const displayMediaOptions = {
			video: {
				displaySurface: "browser",
			},
			audio: {
				suppressLocalAudioPlayback: false,
			},
			preferCurrentTab: false,
			selfBrowserSurface: "exclude",
			systemAudio: "include",
			surfaceSwitching: "include",
			monitorTypeSurfaces: "include",
		};

		navigator.mediaDevices.getDisplayMedia(displayMediaOptions)
			.then(captureStream => {
				const capture = captureStream.getVideoTracks()[0];
				peersRef.current.forEach(peerRef => {
					const peerStream = peerRef.peer.streams[0];
					peerRef.peer.replaceTrack(peerStream.getVideoTracks()[0], capture, peerStream);
				});

				callMinimazedData.current.stream = captureStream;
				setMyStream(captureStream);

				capture.addEventListener("ended", () => {
					shareScreen(false);
				});

				socketRef.current.emit("screenSharingSwitching", { roomId: renderRoomId, screenStatus });
			})
			.catch((e) => {
				setScreenSharing(false);
				callMinimazedData.current.screenSharing = false;

				console.log(e);
			});
	}

	const joinToRoom = () => {
		callMinimazedData.current.stream = createDummyStream(1, 1);
		setMyStream(callMinimazedData.current.stream);

		const peer = new window.SimplePeer({
			initiator: true,
			trickle: false,
			stream: callMinimazedData.current.stream,
		});

		peersRef.current.push({
			peerId: socketRef.current.id,
			username: me.username,
			turnOnCamera: turnOnCamera,
			turnOnMicrophone: turnOnMicrophone,
			screenSharing: screenSharing,
			peer,
		});

		socketRef.current.emit("joinToRoom", { roomId: renderRoomId, userId: me.id, username: me.username, turnOnCamera, turnOnMicrophone, screenSharing });

		listen();
	}

	const listen = () => {
		socketRef.current.on("allUsers", users => {
			const peers = [];

			users.forEach(user => {
				const peer = createPeer(user, socketRef.current.id, callMinimazedData.current.stream);
				peersRef.current.push({
					peerId: user.socketId,
					username: user.username,
					turnOnCamera: user.turnOnCamera,
					turnOnMicrophone: user.turnOnMicrophone,
					screenSharing: user.screenSharing,
					peer,
				});

				peers.push(peer);
			});

			setPeers(peers);
			callMinimazedData.current.peers = peersRef.current;
		});

		socketRef.current.on("userJoined", payload => {
			const peer = addPeer(payload.signal, payload.callerId, callMinimazedData.current.stream);

			peersRef.current.push({
				peerId: payload.callerId,
				username: payload.username,
				peer,
			})

			setPeers(users => [...users, peer]);

			callMinimazedData.current.peers = peersRef.current;
		});

		socketRef.current.on("receivingReturnedSignal", payload => {
			const item = peersRef.current.find(p => p.peerId === payload.id);
			item.peer.signal(payload.signal);
		});

		socketRef.current.on("someUserLeft", socketId => {
			if (peersRef.current.length === 0) {
				return;
			}

			const peerRef = peersRef.current.filter(peer => peer.peerId === socketId)[0];
			peerRef.peer.destroy();

			const peerRefIndex = peersRef.current.indexOf(peerRef);
			peersRef.current.splice(peerRefIndex, 1);
		});
	}

	const createDummyStream = (width, height) => {
		let ctx = new AudioContext(), oscillator = ctx.createOscillator();
		let dst = oscillator.connect(ctx.createMediaStreamDestination());
		oscillator.start();
		const audioTrack = Object.assign(dst.stream.getAudioTracks()[0], { enabled: false });

		let canvas = Object.assign(document.createElement("canvas"), { width: width, height: height });
		canvas.getContext("2d").fillRect(0, 0, width, height);
		let stream = canvas.captureStream();
		const videoTrack = Object.assign(stream.getVideoTracks()[0], { enabled: false });

		const mediaStream = new MediaStream([videoTrack, audioTrack]);

		return mediaStream;
	}

	const leave = (useRedirect = false) => {
		clear();

		socketRef.current.emit("leavingFromRoom", { roomId: renderRoomId, username: me?.username });

		socketRef.current.on("userLeft", () => {
			myStream?.getVideoTracks().forEach(track => {
				track.stop();
			});

			myStream?.getAudioTracks().forEach(track => {
				track.stop();
			});

			peersRef.current.forEach(peerRef => {
				peerRef.peer.destroy();
			});

			socketRef.current.disconnect();

			if (useRedirect) {
				navigate(`/chats`);
			}
		});
	}

	const clear = () => {
		callMinimazedData.current = {
			stream: null,
			peers: [],
			turnOnCamera: false,
			turnOnMicrophone: false,
			roomId: 0,
			socketId: "",
			roomName: "",
		};

		setUseMinimaze(false);
	}

	return {
		func: {
			initConnection,
			joinToRoom,
			switchCallType,
			listen,
			switchCamera,
			switchMicrophone,
			switchMicrophoneDevice,
			switchAudioOutputDevice,
			shareScreen,
			leave,
			setMicrophoneDeviceId
		},
		data: {
			renderRoomId,
			renderChatName,
			socketRef,
			peersRef,
			videoRef,
			turnOnCamera,
			turnOnMicrophone,
			anotherUsersAudio,
			setAnotherUsersAudio,
			screenSharing,
			setScreenSharing,
			microphoneDeviceId,
		},
	};
}

export default useVoice;