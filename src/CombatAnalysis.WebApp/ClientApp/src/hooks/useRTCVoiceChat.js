import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';

const useRTCVoiceChat = (roomId) => {
	const [connection, setConnection] = useState(null);
	const [peerConnection, setPeerConnection] = useState(null);

	//const connectionRef = useRef(null);
	const streamRef = useRef(null);
	//const peerConnectionRef = useRef(null);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const connectToChatAsync = async (signalingAddress, videos) => {
        try {
			const connection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddress)
				.withAutomaticReconnect()
				.build();
			setConnection(connection);

			await connection.start();
			await connection.invoke("JoinRoom", roomId);

			await startStreamAsync(connection, videos);
		} catch (e) {
			console.log(e);
        }
	}

	const startStreamAsync = async (connection, videos) => {
		const peerConnection = await initializationAsync(connection, videos);

		connection.on("ReceiveOffer", async (offer) => {
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
			const answer = await peerConnection.createAnswer();
			await peerConnection.setLocalDescription(answer);

			await connection.invoke("SendAnswer", roomId, JSON.stringify(peerConnection.localDescription));
		});

		connection.on("ReceiveAnswer", async (answer) => {
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
		});

		connection.on("ReceiveCandidate", async (candidate) => {
			await peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
		});

		connection.on("UserLeft", () => {
			if (peerConnection) {
				peerConnection.close();
			}
		});

		await createOfferAsync(connection, peerConnection);
	}

	const initializationAsync = async (connection, videos) => {
		const peerConnection = new RTCPeerConnection(config);
		setPeerConnection(peerConnection);

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true, video: false });
		streamRef.current = stream;

		stream.getTracks().forEach(track => peerConnection.addTrack(track, stream));

		peerConnection.addEventListener("icecandidate", async (event) => {
			if (event.candidate) {
				await connection.invoke("SendCandidate", roomId, JSON.stringify(event.candidate));
			}
		});

		peerConnection.addEventListener("track", (event) => {
			if (event.track.kind === "audio") {
				const remoteStream = document.createElement(event.track.kind);
				remoteStream.srcObject = event.streams[0];
				remoteStream.play();
			}
		});

		return peerConnection;
	}

	const createOfferAsync = async (connection, peerConnection) => {
		const offer = await peerConnection.createOffer();
		await peerConnection.setLocalDescription(offer);

		await connection.invoke("SendOffer", roomId, JSON.stringify(peerConnection.localDescription));
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (streamRef.current === null) {
			return;
		}

		streamRef.current.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		await connection.invoke("SendMicrophoneStatus", roomId, microphoneStatus);
	}

	const switchCameraStatusAsync = async (cameraStatus) => {
		if (streamRef.current === null) {
			return;
		}

		if (cameraStatus) {
			// Turn on the camera
			const videoStream = await navigator.mediaDevices.getUserMedia({ video: true });
			const videoTrack = videoStream.getVideoTracks()[0];
			streamRef.current.addTrack(videoTrack);
			peerConnection.addTrack(videoTrack, streamRef.current);
		} else {
			// Turn off the camera
			streamRef.current.getVideoTracks().forEach(track => {
				track.stop();
				streamRef.current.removeTrack(track);
				peerConnection.getSenders().forEach(sender => {
					if (sender.track && sender.track.kind === "video") {
						peerConnection.removeTrack(sender);
					}
				});
			});
		}

		createOfferAsync(connection, peerConnection);

		await connection.invoke("SendCameraStatus", roomId, cameraStatus);
	}

	const cleanupAudioResources = () => {
		// Stop the media stream tracks
		if (streamRef.current) {
			streamRef.current.getTracks().forEach(track => track.stop());
			streamRef.current = null;
		}

		// Close the peer connection
		if (peerConnection) {
			peerConnection.close();
			setPeerConnection(null);
		}

		// Stop the SignalR connection
		if (connection) {
			connection.stop();
			setConnection(null);
		}
	}

	return [connection, peerConnection, connectToChatAsync, cleanupAudioResources, switchMicrophoneStatusAsync, switchCameraStatusAsync];
}

export default useRTCVoiceChat;