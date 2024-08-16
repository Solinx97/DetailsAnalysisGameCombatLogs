import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';

const useRTCVoiceChat = (roomId) => {
	const [connection, setConnection] = useState(null);
	const [stream, setStream] = useState(null);

	const peerConnectionsRef = useRef(new Map());
	const meIdRef = useRef(null);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const connectToChatAsync = async (signalingAddress) => {
        try {
			const connection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddress)
				.withAutomaticReconnect()
				.build();
			setConnection(connection);

			await connection.start();

			listeningSignalMessages(connection);

			await connection.invoke("JoinRoom", roomId);
			await connection.invoke("RequestConnectedUsers", roomId);

			await prepareAnswerAsync(connection);
		} catch (e) {
			console.log(e);
        }
	}

	const listeningSignalMessages = (connection) => {
		connection.on("Connected", async (userId) => {
			meIdRef.current = userId;

			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("UserJoined", async (userId) => {
			const newPeer = await getOrCreatePeerConnection(userId, connection);
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
					const newPeer = await getOrCreatePeerConnection(userId, connection);
					await createOfferAsync(userId, connection, newPeer);
				}
			}
		});
	}

	const prepareAnswerAsync = async (connection, microphoneStatus) => {
		connection.on("ReceiveOffer", async (fromConnectionId, offer) => {
			const peerConnection = await getOrCreatePeerConnection(fromConnectionId, connection, microphoneStatus);
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

	const getOrCreatePeerConnection = async (userId, connection) => {
		let peerConnection = peerConnectionsRef.current.get(userId);
		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current.set(userId, peerConnection);

		peerConnection.addEventListener("negotiationneeded", async (event) => {
			await peerConnection.setLocalDescription();

			await connection.invoke("SendOffer", roomId, userId, JSON.stringify(peerConnection.localDescription));

			await connection.invoke("RequestConnectedUsers", roomId);
		});

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
		setStream(stream);

		stream.getTracks().forEach(track => {
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

			for (const [userId, peerConnection] of peerConnectionsRef.current) {
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

				for (const [userId, peerConnection] of peerConnectionsRef.current) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						peerConnection.removeTrack(sender);
					}
				}
			});
		}

		await connection.invoke("SendCameraStatus", roomId, cameraStatus);
	}

	const cleanupAsync = async () => {
		if (connection) {
			await connection.invoke("LeaveRoom", roomId);

			connection.off("ReceiveOffer");
			connection.off("ReceiveAnswer");
			connection.off("ReceiveCandidate");
			connection.stop();
			setConnection(null);
		}

		if (stream) {
			stream.getTracks().forEach(track => track.stop());
			setStream(null);
		}

		for (const peerConnection of peerConnectionsRef.current.values()) {
			peerConnection.close();
		}

		peerConnectionsRef.current.clear();

		meIdRef.current = null;
	}

	return [connection, peerConnectionsRef, stream, connectToChatAsync, cleanupAsync, switchMicrophoneStatusAsync, switchCameraStatusAsync];
}

export default useRTCVoiceChat;