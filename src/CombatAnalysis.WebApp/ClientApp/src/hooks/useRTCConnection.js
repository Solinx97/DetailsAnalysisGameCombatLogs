import { useRef } from 'react';

const useRTCConnection = () => {
	const hubConnectionRef = useRef(null);
	const roomIdRef = useRef(null);
	const localStreamRef = useRef(null);

	const peerConnectionsRef = useRef(new Map());

	const myConnectionIdRef = useRef(null);
	const canLeaveRef = useRef(false);
	const setCanLeaveRef = useRef(false);
	const connectionIsActiveRef = useRef(false);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const setup = (hubConnection, roomId, peerConnections, canLeave = true, setCanLeave = () => {}) => {
		hubConnectionRef.current = hubConnection;
		roomIdRef.current = roomId;
		peerConnectionsRef.current = peerConnections;
		canLeaveRef.current = canLeave;
		setCanLeaveRef.current = setCanLeave;

		return connectionIsActiveRef.current;
	}

	const start = () => {
		createStream();

		connectionIsActiveRef.current = true;
	}

	const createStream = () => {
		navigator.mediaDevices.getUserMedia({ audio: true }).then(stream => {
			localStreamRef.current = stream;
		})
	}

	const listeningSignalMessages = () => {
		hubConnectionRef.current.on("Connected", (userId) => {
			myConnectionIdRef.current = userId;
			setCanLeaveRef.current(true);
		});

		hubConnectionRef.current.on("UserJoined", (userId) => {
			getOrCreatePeerConnection(userId);
		});

		hubConnectionRef.current.on("UserLeft", (userId) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
			if (peerConnection) {
				peerConnection.close();
				peerConnectionsRef.current.delete(userId);
			}
		});

		hubConnectionRef.current.on("ReceiveConnectedUsers", (connectedUsers) => {
			for (const userId of connectedUsers) {
				if (myConnectionIdRef.current && userId !== myConnectionIdRef.current) {
					getOrCreatePeerConnection(userId);
				}
			}
		});
	}

	const listeningAnswersAsync = async () => {
		hubConnectionRef.current.on("ReceiveOffer", async (fromConnectionId, offer) => {
			const peerConnection = getOrCreatePeerConnection(fromConnectionId);
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
			const answer = await peerConnection.createAnswer();
			await peerConnection.setLocalDescription(answer);

			await sendSignalAsync("SendAnswer", fromConnectionId, JSON.stringify(peerConnection.localDescription));
		});

		hubConnectionRef.current.on("ReceiveAnswer", async (fromConnectionId, answer) => {
			const peerConnection = peerConnectionsRef.current.get(fromConnectionId);
			if (peerConnection) {
				await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
			}
		});

		hubConnectionRef.current.on("ReceiveCandidate", async (userId, candidate) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
			if (peerConnection) {
				await peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
			}
		});
	}

	const getOrCreatePeerConnection = (userId) => {
		let peerConnection = peerConnectionsRef.current.get(userId);
		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current.set(userId, peerConnection);

		peerConnection.addEventListener("negotiationneeded", async (event) => {
			const pc = event.currentTarget;

			const offer = await pc.createOffer();
			await pc.setLocalDescription(offer);

			await sendSignalAsync("SendOffer", userId, JSON.stringify(pc.localDescription));
		});

		peerConnection.addEventListener("icecandidate", async (event) => {
			if (event.candidate) {
				await sendSignalAsync("SendCandidate", userId, JSON.stringify(event.candidate));
			}
		});

		navigator.mediaDevices.getUserMedia({ audio: true }).then(stream => {
			localStreamRef.current.getAudioTracks().forEach(track => {
				track.stop();
				localStreamRef.current.removeTrack(track);
			});

			stream.getAudioTracks().forEach(track => {
				localStreamRef.current.addTrack(track);
			});

			localStreamRef.current.getTracks().forEach(track => {
				addTrackToPeer(peerConnection, track, localStreamRef.current);
			});
		})

		return peerConnection;
	}

	const sendSignalAsync = async (message, userId, content, status = null) => {
		try {
			if (!hubConnectionRef.current || !connectionIsActiveRef.current) {
				return;
			}

			const roomId = !roomIdRef.current ? "" : roomIdRef.current;
			if (userId && content && status !== null) {
				await hubConnectionRef.current.invoke(message, roomId, userId, content, status);
			}
			else if (userId && content && status === null) {
				await hubConnectionRef.current.invoke(message, roomId, userId, content);
			}
			else if (userId && status !== null) {
				await hubConnectionRef.current.invoke(message, roomId, userId, status);
			}
			else if (userId) {
				await hubConnectionRef.current.invoke(message, roomId, userId);
			}
			else if (status !== null) {
				await hubConnectionRef.current.invoke(message, roomId, status);
			}
			else {
				await hubConnectionRef.current.invoke(message, roomId);
			}
		} catch (e) {
			console.error(e);
		}
	}

	const cleanup = () => {
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
			if (hubConnectionRef.current) {
				hubConnectionRef.current.stop().then(() => {
					hubConnectionRef.current = null;
				});
			}

			// Clear other references
			myConnectionIdRef.current = null;
		} catch (error) {
			console.error("Error during cleanup:", error);
		}
	}

	const addTrackToPeer = (peerConnection, track, stream) => {
		peerConnection.addTrack(track, stream);
	}

	return { localStreamRef, setup, start, listeningSignalMessages, listeningAnswersAsync, sendSignalAsync, cleanup, addTrackToPeer };
}

export default useRTCConnection;