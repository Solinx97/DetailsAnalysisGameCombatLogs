import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';

const useRTCVoiceChat = (roomId) => {
	const [connection, setConnection] = useState(null);
	const [stream, setStream] = useState(null);
	const [screenStream, setScreenStream] = useState(null);

	const peerConnectionsRef = useRef(new Map());
	const meIdRef = useRef(null);

	const signalingAddressRef = useRef(null);
	const setCanLeaveRef = useRef(null);
	const turnOnCameraRef = useRef(null);
	const connectionIsActiveRef = useRef(false);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const connectToChatAsync = async (signalingAddress, setCanLeave, turnOnCamera) => {
		signalingAddressRef.current = signalingAddress;
		setCanLeaveRef.current = setCanLeave;
		turnOnCameraRef.current = turnOnCamera;

		await startAsync();
	}

	const startAsync = async () => {
		try {
			const connection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddressRef.current)
				.withAutomaticReconnect()
				.build();
			setConnection(connection);

			await connection.start();

			connectionIsActiveRef.current = true;

			listeningSignalMessages(connection, setCanLeaveRef.current, turnOnCameraRef.current);

			await sendSignalAsync(connection, "JoinRoom");
			await sendSignalAsync(connection, "RequestConnectedUsers");

			await prepareAnswerAsync(connection, turnOnCameraRef.current);
		} catch (e) {
			console.log(e);
		}
	}

	const listeningSignalMessages = (connection, setCanLeave, turnOnCamera) => {
		connection.on("Connected", async (userId) => {
			meIdRef.current = userId;
			setCanLeave(true);

			await sendSignalAsync(connection, "RequestConnectedUsers");
		});

		connection.on("UserJoined", async (userId) => {
			const newPeer = await getOrCreatePeerConnectionAsync(userId, connection, turnOnCamera);
			await createOfferAsync(userId, connection, newPeer);

			await sendSignalAsync(connection, "RequestConnectedUsers");
		});

		connection.on("UserLeft", async (userId) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
			if (peerConnection) {
				peerConnection.close();
				peerConnectionsRef.current.delete(userId);
			}

			await sendSignalAsync(connection, "RequestConnectedUsers");
		});

		connection.on("ReceiveConnectedUsers", async (connectedUsers) => {
			for (const userId of connectedUsers) {
				if (userId !== meIdRef.current) {
					const newPeer = await getOrCreatePeerConnectionAsync(userId, connection, turnOnCamera);
					await createOfferAsync(userId, connection, newPeer);
				}
			}
		});
	}

	const prepareAnswerAsync = async (connection, turnOnCamera) => {
		connection.on("ReceiveOffer", async (fromConnectionId, offer) => {
			const peerConnection = await getOrCreatePeerConnectionAsync(fromConnectionId, connection, turnOnCamera);
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
			const answer = await peerConnection.createAnswer();
			await peerConnection.setLocalDescription(answer);

			await sendSignalAsync(connection, "SendAnswer", fromConnectionId, JSON.stringify(peerConnection.localDescription));
		});

		connection.on("ReceiveAnswer", async (fromConnectionId, answer) => {
			const peerConnection = peerConnectionsRef.current.get(fromConnectionId);
			if (peerConnection) {
				await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
			}
		});

		connection.on("ReceiveCandidate", async (userId, candidate) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
			if (peerConnection) {
				await peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
			}
		});
	}

	const getOrCreatePeerConnectionAsync = async (userId, connection, turnOnCamera) => {
		let peerConnection = peerConnectionsRef.current.get(userId);
		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current.set(userId, peerConnection);

		peerConnection.addEventListener("negotiationneeded", async (event) => {
            try {
				if (connection._connectionState === "Connected") {
					await event.currentTarget.setLocalDescription();

					await sendSignalAsync(connection, "SendAnswer", userId, JSON.stringify(event.currentTarget.localDescription));
					await sendSignalAsync(connection, "RequestConnectedUsers");
				}
			} catch (e) {
				console.log(e);
            }
		});

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
		setStream(stream);

		stream.getAudioTracks().forEach(track => {
			peerConnection.addTrack(track, stream);
		});

		peerConnection.addEventListener("icecandidate", async (event) => {
			if (event.candidate) {
				await sendSignalAsync(connection, "SendCandidate", userId, JSON.stringify(event.candidate));
			}
		});

		return peerConnection;
	}

	const createOfferAsync = async (targetConnectionId, connection, peerConnection) => {
		const offer = await peerConnection.createOffer();
		await peerConnection.setLocalDescription(offer);

		await sendSignalAsync(connection, "SendOffer", targetConnectionId, JSON.stringify(peerConnection.localDescription));
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (stream === null) {
			return;
		}

		stream.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		await sendSignalAsync(connection, "SendMicrophoneStatus", microphoneStatus);
	}

	const switchCameraStatusAsync = async (cameraStatus, setCameraExecuted) => {
		if (stream === null) {
			return;
		}

		setCameraExecuted(true);

		if (cameraStatus) {
			const localStream = await navigator.mediaDevices.getUserMedia({ video: true });

			localStream.getVideoTracks().forEach(track => {
				stream.addTrack(track);

				for (const peerConnection of peerConnectionsRef.current.values()) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						sender.replaceTrack(track);
					} else {
						peerConnection.addTrack(track, stream);
					}
				}
			});
		} else {
			stream.getVideoTracks().forEach(track => {
				track.stop();
				stream.removeTrack(track);

				for (const peerConnection of peerConnectionsRef.current.values()) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						peerConnection.removeTrack(sender);
					}
				}
			});
		}

		await sendSignalAsync(connection, "SendCameraStatus", null, null, cameraStatus);

		setCameraExecuted(false);
	}

	const startScreenSharingAsync = async (screenSharingStatus) => {
		try {
			if (!screenSharingStatus) {
				await stopScreenSharingAsync();

				return null;
			}

			let newScreenStream = null;
			if (!screenStream) {
				newScreenStream = await navigator.mediaDevices.getDisplayMedia({ video: true });
				setScreenStream(newScreenStream);
			}
			else {
				newScreenStream = screenStream;
			}

			await sendSignalAsync(connection, "SendScreenSharingStatus", null, null, true);

			newScreenStream.getVideoTracks().forEach(track => {
				for (const peerConnection of peerConnectionsRef.current.values()) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						sender.replaceTrack(track);
					} else {
						peerConnection.addTrack(track, stream);
					}
				}
			});

			newScreenStream.getVideoTracks()[0].addEventListener("ended", async () => {
				await stopScreenSharingAsync();
			});

			return newScreenStream;
		} catch (e) {
			console.log(e);

			return null;
		}
	}

	const stopScreenSharingAsync = async () => {
		screenStream?.getTracks().forEach(track => {
			track.stop();
			screenStream.removeTrack(track);
		});

		setScreenStream(null);

		await sendSignalAsync(connection, "SendScreenSharingStatus", null, null, false);

		// Remove screen tracks from peer connections
		for (const peerConnection of peerConnectionsRef.current.values()) {
			const senders = peerConnection.getSenders().filter(sender => sender.track && sender.track.kind === "video");
			senders.forEach(sender => {
				peerConnection.removeTrack(sender);
			});
		}
	}

	const stopMediaDataAsync = async () => {
		// Stop and remove all video tracks
		connectionIsActiveRef.current = false;

		// Stop and remove all audio tracks
		stream?.getAudioTracks().forEach(track => {
			track.stop();
			stream.removeTrack(track);
		});

		stream?.getVideoTracks().forEach(track => {
			track.stop();
			stream.removeTrack(track);
		});

		setStream(null);

		await stopScreenSharingAsync();
	}

	const cleanupAsync = async () => {
		try {
			// Close all peer connections
			for (const peerConnection of peerConnectionsRef.current.values()) {
				const senders = peerConnection.getSenders();
				senders.forEach(sender => {
					const track = sender.track;
					if (track) {
						track.stop();
					}

					peerConnection.removeTrack(sender);
				});

				peerConnection.close();
			}

			// Clear peer connections map
			peerConnectionsRef.current.clear();

			// Leave the room and stop the connection
			if (connection) {
				await connection.stop();
				setConnection(null);
			}

			// Clear other references
			meIdRef.current = null;
			signalingAddressRef.current = null;
			setCanLeaveRef.current = null;
			turnOnCameraRef.current = null;
		} catch (error) {
			console.error("Error during cleanup:", error);
		}
	}

	const sendSignalAsync = async (connection, message, userId, content, status = null) => {
		try {
			if (!connection || !connectionIsActiveRef.current) {
				return;
			}

			if (userId && content && status !== null) {
				await connection.invoke(message, roomId, userId, content, status);
			}
			else if (userId && content && status === null) {
				await connection.invoke(message, roomId, userId, content);
			}
			else if (userId && status !== null) {
				await connection.invoke(message, roomId, userId, status);
			}
			else if (userId) {
				await connection.invoke(message, roomId, userId);
			}
			else if (status !== null) {
				await connection.invoke(message, roomId, status);
			}
			else {
				await connection.invoke(message, roomId);
			}
		} catch (e) {
			console.log(e);
        }
	}

	return {
		properties: {
            connection,
            peerConnectionsRef,
            stream,
            screenStream
		},
		methods: {
            connectToChatAsync,
            stopMediaDataAsync,
            cleanupAsync,
            switchMicrophoneStatusAsync,
            switchCameraStatusAsync,
			startScreenSharingAsync,
        }
	};
}

export default useRTCVoiceChat;