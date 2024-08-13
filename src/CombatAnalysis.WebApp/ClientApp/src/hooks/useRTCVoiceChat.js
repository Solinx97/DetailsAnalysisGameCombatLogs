import * as signalR from '@microsoft/signalr';
import { useState } from 'react';

const useRTCVoiceChat = (roomId) => {
	const [connection, setConnection] = useState(null);
	const [peerConnection, setPeerConnection] = useState(null);
	const [stream, setStream] = useState(null);

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

			await connection.invoke("JoinRoom", roomId);

			await startStreamAsync(connection, microphoneStatus);
		} catch (e) {
			console.log(e);
        }
	}

	const startStreamAsync = async (connection, microphoneStatus) => {
		const peerConnection = await initAsync(connection, microphoneStatus);

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
	}

	const initAsync = async (connection, microphoneStatus, cameraStatus) => {
		const peerConnection = new RTCPeerConnection(config);
		setPeerConnection(peerConnection);

		const stream = await navigator.mediaDevices.getUserMedia({ audio: true, video: cameraStatus });
		setStream(stream);

		await microphoneStatusInit(stream, peerConnection, microphoneStatus);

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

	const microphoneStatusInit = async (stream, peerConnection, microphoneStatus) => {
		stream.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
			const hasAudioTrack = peerConnection.getSenders().some(sender => sender.track && sender.track.kind === "audio");
			if (!hasAudioTrack) {
				peerConnection.addTrack(track, stream);
			}
		});
	}

	const createOfferAsync = async (connection, peerConnection) => {
		const offer = await peerConnection.createOffer();
		await peerConnection.setLocalDescription(offer);

		await connection.invoke("SendOffer", roomId, JSON.stringify(peerConnection.localDescription));
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (stream === null || peerConnection?.signalingState === "closed") {
			return;
		}

		stream.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		await connection.invoke("SendMicrophoneStatus", roomId, microphoneStatus);

		await createOfferAsync(connection, peerConnection);
	}

	const switchCameraStatusAsync = async (cameraStatus, setCameraExecuted) => {
		if (stream === null || peerConnection?.signalingState === "closed") {
			return;
		}

		if (cameraStatus) {
			// Turn on the camera
			setCameraExecuted(true);

			const localStream = await navigator.mediaDevices.getUserMedia({ audio: true, video: cameraStatus });
			const videoTrack = localStream.getVideoTracks()[0];
			stream.addTrack(videoTrack);
			
			const hasVideoTrack = peerConnection.getSenders().some(sender => sender.track && sender.track.kind === "video");
			if (!hasVideoTrack) {
				peerConnection.addTrack(videoTrack, stream);
			}

			setCameraExecuted(false);
		} else {
			// Turn off the camera
			stream.getVideoTracks().forEach(track => {
				track.stop();
				stream.removeTrack(track);

				peerConnection.getSenders().forEach(sender => {
					if (sender.track && sender.track.kind === "video") {
						peerConnection.removeTrack(sender);
					}
				});
			});
		}

		await connection.invoke("SendCameraStatus", roomId, cameraStatus);

		await createOfferAsync(connection, peerConnection);
	}

	const cleanup = () => {
		// Stop the media stream tracks
		if (stream) {
			stream.getTracks().forEach(track => track.stop());
			setStream(null);
		}

		// Close the peer connection
		if (peerConnection) {
			peerConnection.close();
			setPeerConnection(null);
		}

		// Stop the SignalR connection
		if (connection) {
			connection.off("ReceiveOffer");
			connection.off("ReceiveAnswer");
			connection.off("ReceiveCandidate");

			connection.stop();
			setConnection(null);
		}
	}

	return [connection, peerConnection, stream, connectToChatAsync, cleanup, switchMicrophoneStatusAsync, switchCameraStatusAsync];
}

export default useRTCVoiceChat;