import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';

const useRTCVoiceChat = (roomId) => {
	const [connection, setConnection] = useState(null);

	const streamRef = useRef(null);
	const peerConnectionsRef = useRef(new Map());
	const meIdRef = useRef(null);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const connectToChatAsync = async (signalingAddress, microphoneStatus) => {
        try {
			const connection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddress)
				.withAutomaticReconnect()
				.build();
			setConnection(connection);

			await connection.start();

			connection.on("Connected", async (userId) => {
				meIdRef.current = userId;

				const newPeer = await getOrCreatePeerConnection(userId, connection, microphoneStatus);
				await createOfferAsync(userId, connection, newPeer);

				await connection.invoke("RequestConnectedUsers", roomId);
			});

			connection.on("ReceiveConnectedUsers", async (connectedUsers) => {
				for (const userId of connectedUsers) {
					if (userId !== meIdRef.current) {
						const newPeer = await getOrCreatePeerConnection(userId, connection, microphoneStatus);
						await createOfferAsync(userId, connection, newPeer);
					}
                }

			});

			connection.on("UserLeft", async (userId) => {
				const peerConnection = peerConnectionsRef.current.get(userId);
				if (peerConnection) {
					peerConnection.close();
					peerConnectionsRef.current.delete(userId);
				}

				await connection.invoke("RequestConnectedUsers", roomId);
			});

			await connection.invoke("JoinRoom", roomId);

			await startStreamAsync(connection, microphoneStatus);
		} catch (e) {
			console.log(e);
        }
	}

	const startStreamAsync = async (connection, microphoneStatus) => {
		connection.on("ReceiveOffer", async (userId, offer) => {
			const peerConnection = await getOrCreatePeerConnection(userId, connection, microphoneStatus);
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
			const answer = await peerConnection.createAnswer();
			await peerConnection.setLocalDescription(answer);

			await connection.invoke("SendAnswer", roomId, userId, JSON.stringify(peerConnection.localDescription));
		});

		connection.on("ReceiveAnswer", async (userId, answer) => {
			const peerConnection = peerConnectionsRef.current.get(userId);
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

	const getOrCreatePeerConnection = async (userId, connection, microphoneStatus) => {
		let peerConnection = peerConnectionsRef.current.get(userId);

		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current.set(userId, peerConnection);

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true, video: false });
		streamRef.current = stream;

		microphoneStatusInit(stream, peerConnection, microphoneStatus);

		peerConnection.addEventListener("icecandidate", async (event) => {
			if (event.candidate) {
				await connection.invoke("SendCandidate", roomId, userId, JSON.stringify(event.candidate));
			}
		});

		peerConnection.addEventListener("track", (event) => {
			const audioElement = document.createElement('audio');
			audioElement.srcObject = event.streams[0];
			audioElement.autoplay = true;
			audioElement.play();

			document.body.appendChild(audioElement);
		});

		return peerConnection;
	}

	const microphoneStatusInit = (stream, peerConnection, microphoneStatus) => {
		stream.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
			peerConnection.addTrack(track, stream);
		});
	}

	const createOfferAsync = async (userId, connection, peerConnection) => {
		const offer = await peerConnection.createOffer();
		await peerConnection.setLocalDescription(offer);

		await connection.invoke("SendOffer", roomId, userId, JSON.stringify(peerConnection.localDescription));
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (streamRef.current === null) {
			return;
		}

		streamRef.current.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		await connection.invoke("SendMicrophoneStatus", roomId, microphoneStatus);

		for (const [userId, peerConnection] of peerConnectionsRef.current) {
			if (peerConnection.signalingState !== "closed") {
				await createOfferAsync(userId, connection, peerConnection);
			}
		}
	}

	const switchCameraStatusAsync = async (cameraStatus, setCameraExecuted) => {
		if (streamRef.current === null) {
			return;
		}

		if (cameraStatus) {
			// Turn on the camera
			setCameraExecuted(true);

			const localStream = await navigator.mediaDevices.getUserMedia({ audio: true, video: cameraStatus });
			const videoTrack = localStream.getVideoTracks()[0];
			streamRef.current.addTrack(videoTrack);

			for (const peerConnection of peerConnectionsRef.current.values()) {
				const hasVideoTrack = peerConnection.getSenders().some(sender => sender.track && sender.track.kind === "video");
				if (!hasVideoTrack) {
					peerConnection.addTrack(videoTrack, streamRef.current);
				}
			}

			setCameraExecuted(false);
		} else {
			// Turn off the camera
			streamRef.current.getVideoTracks().forEach(track => {
				track.stop();
				streamRef.current.removeTrack(track);

				for (const peerConnection of peerConnectionsRef.current.values()) {
					peerConnection.getSenders().forEach(sender => {
						if (sender.track && sender.track.kind === "video") {
							peerConnection.removeTrack(sender);
						}
					});
				}
			});
		}

		await connection.invoke("SendCameraStatus", roomId, cameraStatus);

		for (const [userId, peerConnection] of peerConnectionsRef.current) {
			if (peerConnection.signalingState !== "closed") {
				await createOfferAsync(userId, connection, peerConnection);
			}
		}
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

		if (streamRef.current) {
			streamRef.current.getTracks().forEach(track => track.stop());
			streamRef.current = null;
		}

		for (const peerConnection of peerConnectionsRef.current.values()) {
			peerConnection.close();
		}

		peerConnectionsRef.current.clear();

		meIdRef.current = null;
	}

	return [connection, connectToChatAsync, cleanupAsync, switchMicrophoneStatusAsync, switchCameraStatusAsync];
}

export default useRTCVoiceChat;