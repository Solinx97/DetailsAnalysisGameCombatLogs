import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';
import useRTCConnection from './useRTCConnection';

const useVoiceChatHub = (roomId) => {
	const [hubConnection, setHubConnection] = useState(null);
	const [stream, setStream] = useState(null);
	const [screenStream, setScreenStream] = useState(null);

	const peerConnectionsRef = useRef(new Map());
	const connectionIsActiveRef = useRef(false);

	const { setup, start, listeningSignalMessages, listeningAnswersAsync, sendSignalAsync, cleanupAsync, addTrackToPeer } = useRTCConnection();

	const connectToChatAsync = async (signalingAddress, canLeave, setCanLeave) => {
		try {
			const hubConnection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddress)
				.withAutomaticReconnect()
				.build();
			setHubConnection(hubConnection);

			await hubConnection.start();

			const connectionIsActive = setup(hubConnection, roomId, setStream, peerConnectionsRef.current, canLeave, setCanLeave);
			connectionIsActiveRef.current = connectionIsActive;

			start();

			listeningSignalMessages();

			await sendSignalAsync("JoinRoom");

			await listeningAnswersAsync();
		} catch (e) {
			console.log(e);
		}
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (!stream) {
			return;
		}

		stream.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		hubConnection.on("ReceiveRequestMicrophoneStatus", async (userId) => {
			await sendSignalAsync("SendMicrophoneStatus", null, null, microphoneStatus);
		});

		await sendSignalAsync("SendMicrophoneStatus", null, null, microphoneStatus);
	}

	const switchCameraStatusAsync = async (cameraStatus) => {
		if (!stream) {
			return;
		}

		if (cameraStatus) {
			const localStream = await navigator.mediaDevices.getUserMedia({ video: true });

			localStream.getVideoTracks().forEach(async (track) => {
				stream.addTrack(track);

				for (const peerConnection of peerConnectionsRef.current.values()) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						await sender.replaceTrack(track);
					} else {
						addTrackToPeer(peerConnection, track, stream);
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

		hubConnection.on("ReceiveRequestCameraStatus", async (userId) => {
			await sendSignalAsync("SendCameraStatus", null, null, cameraStatus);
		});

		await sendSignalAsync("SendCameraStatus", null, null, cameraStatus);
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

			await sendSignalAsync("SendScreenSharingStatus", null, null, true);

			newScreenStream.getVideoTracks().forEach(async (track) => {
				for (const peerConnection of peerConnectionsRef.current.values()) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						await sender.replaceTrack(track);
					} else {
						addTrackToPeer(peerConnection, track, newScreenStream);
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

		await sendSignalAsync("SendScreenSharingStatus", null, null, false);

		// Remove screen tracks from peer connections
		for (const peerConnection of peerConnectionsRef.current.values()) {
			const senders = peerConnection.getSenders().filter(sender => sender.track && sender.track.kind === "video");
			senders.forEach(sender => {
				peerConnection.removeTrack(sender);
			});
		}
	}

	const stopMediaDataAsync = async () => {
		// Stop and remove all tracks
		connectionIsActiveRef.current = false;

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
		await cleanupAsync();
	}

	return {
		properties: {
            hubConnection,
            peerConnectionsRef,
            stream,
            screenStream
		},
		methods: {
            connectToChatAsync,
            stopMediaDataAsync,
            switchMicrophoneStatusAsync,
            switchCameraStatusAsync,
			startScreenSharingAsync,
			cleanupAsync,
        }
	};
}

export default useVoiceChatHub;