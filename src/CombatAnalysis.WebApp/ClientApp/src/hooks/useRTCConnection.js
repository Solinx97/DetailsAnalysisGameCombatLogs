import { useRef } from 'react';

const useRTCConnection = () => {
	const hubConnectionRef = useRef(null);
	const roomIdRef = useRef(null);
	const setStreamRef = useRef(null);

	const peerConnectionsRef = useRef(new Map());

	const myConnectionIdRef = useRef(null);
	const canLeaveRef = useRef(false);
	const setCanLeaveRef = useRef(false);
	const connectionIsActiveRef = useRef(false);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const setup = (hubConnection, roomId, setStream, peerConnections, canLeave = true, setCanLeave = () => {}) => {
		hubConnectionRef.current = hubConnection;
		roomIdRef.current = roomId;
		setStreamRef.current = setStream;
		peerConnectionsRef.current = peerConnections;
		canLeaveRef.current = canLeave;
		setCanLeaveRef.current = setCanLeave;

		return connectionIsActiveRef.current;
	}

	const start = () => {
		connectionIsActiveRef.current = true;
	}

	const listeningSignalMessages = () => {
		hubConnectionRef.current.on("Connected", async (userId) => {
			myConnectionIdRef.current = userId;
			setCanLeaveRef.current(true);

			await getOrCreatePeerConnectionAsync(userId);
		});

		hubConnectionRef.current.on("UserJoined", async (userId) => {
			await getOrCreatePeerConnectionAsync(userId);
		});

		hubConnectionRef.current.on("UserLeft", async (userId) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
			if (peerConnection) {
				peerConnection.close();
				peerConnectionsRef.current.delete(userId);
			}
		});

		hubConnectionRef.current.on("ReceiveConnectedUsers", async (connectedUsers) => {
			for (const userId of connectedUsers) {
				if (myConnectionIdRef.current && userId !== myConnectionIdRef.current) {
					await getOrCreatePeerConnectionAsync(userId);
				}
			}
		});
	}

	const listeningAnswersAsync = async () => {
		hubConnectionRef.current.on("ReceiveOffer", async (fromConnectionId, offer) => {
			const peerConnection = await getOrCreatePeerConnectionAsync(fromConnectionId);
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

	const getOrCreatePeerConnectionAsync = async (userId) => {
		let peerConnection = peerConnectionsRef.current.get(userId);
		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current.set(userId, peerConnection);

		peerConnection.addEventListener("negotiationneeded", async (event) => {
			try {
				await createOfferAsync(userId, peerConnection);
			} catch (e) {
				console.log(e);
			}
		});

		peerConnection.addEventListener("icecandidate", async (event) => {
			if (event.candidate) {
				await sendSignalAsync("SendCandidate", userId, JSON.stringify(event.candidate));
			}
		});

		navigator.mediaDevices.getUserMedia({ audio: true }).then(steam => {
			steam.getAudioTracks().forEach(track => {
				addTrackToPeer(peerConnection, track, steam);
			});

			setStreamRef.current(steam);
		});

		return peerConnection;
	}

	const createOfferAsync = async (targetConnectionId, peerConnection) => {
		const offer = await peerConnection.createOffer();
		await peerConnection.setLocalDescription(offer);

		await sendSignalAsync("SendOffer", targetConnectionId, JSON.stringify(peerConnection.localDescription));
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
			console.log(e);
		}
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
			if (hubConnectionRef.current) {
				await hubConnectionRef.current.stop();
				hubConnectionRef.current = null;
			}

			// Clear other references
			myConnectionIdRef.current = null;
		} catch (error) {
			console.error("Error during cleanup:", error);
		}
	}

	const getKeyByValue = (map, searchValue) => {
		for (let [key, value] of map.entries()) {
			if (value === searchValue) {
				return key;
			}
		}

		return undefined;
	}

	const addTrackToPeer = (peerConnection, track, stream) => {
		peerConnection.addTrack(track, stream);
	}

	return { setup, start, listeningSignalMessages, listeningAnswersAsync, sendSignalAsync, cleanupAsync, addTrackToPeer };
}

export default useRTCConnection;