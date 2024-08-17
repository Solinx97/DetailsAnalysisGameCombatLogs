import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';
import { connect } from 'react-redux';

const useRTCVoiceChat = (roomId) => {
	const [connection, setConnection] = useState(null);
	const [stream, setStream] = useState(null);
	const [screenStream, setScreenStream] = useState(null);

	const peerConnectionsRef = useRef(new Map());
	const meIdRef = useRef(null);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const connectToChatAsync = async (signalingAddress, setCanLeave, turnOnCamera) => {
        try {
			const connection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddress)
				.withAutomaticReconnect()
				.build();
			setConnection(connection);

			console.log(connection);
			await connection.start();

			listeningSignalMessages(connection, setCanLeave, turnOnCamera);

			await connection.invoke("JoinRoom", roomId);
			await connection.invoke("RequestConnectedUsers", roomId);

			await prepareAnswerAsync(connection, turnOnCamera);
		} catch (e) {
			console.log(e);
        }
	}

	const listeningSignalMessages = (connection, setCanLeave, turnOnCamera) => {
		connection.on("Connected", async (userId) => {
			meIdRef.current = userId;
			setCanLeave(true);

			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("UserJoined", async (userId) => {
			const newPeer = await getOrCreatePeerConnection(userId, connection, turnOnCamera);
			await createOfferAsync(userId, connection, newPeer);

			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("UserLeft", async (userId) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
			if (peerConnection) {
				peerConnection.close();
				peerConnectionsRef.current.delete(userId);
			}

			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("ReceiveConnectedUsers", async (connectedUsers) => {
			for (const userId of connectedUsers) {
				if (userId !== meIdRef.current) {
					const newPeer = await getOrCreatePeerConnection(userId, connection, turnOnCamera);
					await createOfferAsync(userId, connection, newPeer);
				}
			}
		});
	}

	const prepareAnswerAsync = async (connection, turnOnCamera) => {
		connection.on("ReceiveOffer", async (fromConnectionId, offer) => {
			const peerConnection = await getOrCreatePeerConnection(fromConnectionId, connection, turnOnCamera);
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
			const answer = await peerConnection.createAnswer();
			await peerConnection.setLocalDescription(answer);

			await connection.invoke("SendAnswer", roomId, fromConnectionId, JSON.stringify(peerConnection.localDescription));
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

	const getOrCreatePeerConnection = async (userId, connection, turnOnCamera) => {
		let peerConnection = peerConnectionsRef.current.get(userId);
		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current.set(userId, peerConnection);

		peerConnection.addEventListener("negotiationneeded", async (event) => {
			if (connection._connectionState === "Connected") {
				await event.currentTarget.setLocalDescription();

				await connection.invoke("SendOffer", roomId, userId, JSON.stringify(event.currentTarget.localDescription));

				await connection.invoke("RequestConnectedUsers", roomId);
			}
		});

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
		setStream(stream);

		stream.getAudioTracks().forEach(track => {
			peerConnection.addTrack(track, stream);
		});

		peerConnection.addEventListener("icecandidate", async (event) => {
			if (event.candidate) {
				await connection.invoke("SendCandidate", roomId, userId, JSON.stringify(event.candidate));
			}
		});

		return peerConnection;
	}

	const createOfferAsync = async (targetConnectionId, connection, peerConnection) => {
		const offer = await peerConnection.createOffer();
		await peerConnection.setLocalDescription(offer);

		await connection.invoke("SendOffer", roomId, targetConnectionId, JSON.stringify(peerConnection.localDescription));
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (stream === null) {
			return;
		}

		stream.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		await connection.invoke("SendMicrophoneStatus", roomId, microphoneStatus);
	}

	const switchCameraStatusAsync = async (cameraStatus, setCameraExecuted) => {
		if (stream === null) {
			return;
		}

		if (cameraStatus) {
			setCameraExecuted(true);

			const localStream = await navigator.mediaDevices.getUserMedia({ video: true });
			const videoTrack = localStream.getVideoTracks()[0];
			stream.addTrack(videoTrack);

			for (const peerConnection of peerConnectionsRef.current.values()) {
				const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
				if (sender) {
					sender.replaceTrack(videoTrack);
				} else {
					peerConnection.addTrack(videoTrack, stream);
				}
			}

			setCameraExecuted(false);
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

		await connection.invoke("SendCameraStatus", roomId, cameraStatus);
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

			await connection.invoke("SendScreenSharingStatus", roomId, true);

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
		});

		setScreenStream(null);

		await connection.invoke("SendScreenSharingStatus", roomId, false);

		// Remove screen tracks from peer connections
		for (const peerConnection of peerConnectionsRef.current.values()) {
			const senders = peerConnection.getSenders().filter(sender => sender.track && sender.track.kind === "video" && sender.track.label.includes("screen"));
			senders.forEach(sender => {
				peerConnection.removeTrack(sender);
			});
		}
	}

	const stopMediaData = () => {
		// Stop and remove all video tracks
		stream?.getVideoTracks().forEach(track => {
			track.stop();
			stream.removeTrack(track);
		});

		// Stop and remove all audio tracks
		stream?.getAudioTracks().forEach(track => {
			track.stop();
			stream.removeTrack(track);
		});

		setStream(null);

		stopScreenSharingAsync();
	}

	const cleanupAsync = async () => {
		//for (const [userId, peerConnection] of peerConnectionsRef.current) {
		//	const senders = peerConnection?.getSenders().filter(sender => sender.track);
		//	senders?.forEach(sender => {
		//		if (sender.track) {
		//			sender.track.stop();
		//		}

		//		peerConnection?.removeTrack(sender);
		//	});
		//}

		if (connection) {
			await connection.invoke("LeaveRoom", roomId);

			connection.stop();

			setConnection(null);
		}

		peerConnectionsRef.current.clear();

		meIdRef.current = null;
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
            stopMediaData,
            cleanupAsync,
            switchMicrophoneStatusAsync,
            switchCameraStatusAsync,
			startScreenSharingAsync,
        }
	};
}

export default useRTCVoiceChat;