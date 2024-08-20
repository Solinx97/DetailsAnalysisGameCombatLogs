import * as signalR from '@microsoft/signalr';
import { useRef, useState } from 'react';
import useRTCConnection from './useRTCConnection';

const useVoiceChatHub = (roomId) => {
	const [hubConnection, setHubConnection] = useState(null);

	const peerConnectionsRef = useRef(new Map());
	const connectionIsActiveRef = useRef(false);

	const { localStreamRef, setup, start, listeningSignalMessages, listeningAnswersAsync, sendSignalAsync, cleanupAsync, addTrackToPeer } = useRTCConnection();

	const connectToChatAsync = async (myId, signalingAddress, canLeave, setCanLeave) => {
		try {
			const hubConnection = new signalR.HubConnectionBuilder()
				.withUrl(signalingAddress)
				.withAutomaticReconnect()
				.build();
			setHubConnection(hubConnection);

			await hubConnection.start();

			const connectionIsActive = setup(hubConnection, roomId, peerConnectionsRef.current, canLeave, setCanLeave);
			connectionIsActiveRef.current = connectionIsActive;

			start();

			listeningSignalMessages();
			await listeningAnswersAsync();

			await sendSignalAsync("JoinRoom", myId);
			await sendSignalAsync("RequestConnectedUsers");
		} catch (e) {
			console.log(e);
		}
	}

	const switchMicrophoneStatusAsync = async (microphoneStatus) => {
		if (!localStreamRef.current) {
			return;
		}

		localStreamRef.current.getAudioTracks().forEach(track => {
			track.enabled = microphoneStatus;
		});

		hubConnection.on("ReceiveRequestMicrophoneStatus", async () => {
			await sendSignalAsync("SendMicrophoneStatus", null, null, microphoneStatus);
		});

		await sendSignalAsync("SendMicrophoneStatus", null, null, microphoneStatus);
	}

	const handleVideoTracks = () => {
		localStreamRef.current.getTracks().forEach(async (track) => {
			for (const peerConnection of peerConnectionsRef.current.values()) {
				const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === track.kind);
				if (sender) {
					await sender.replaceTrack(track);
				} else {
					addTrackToPeer(peerConnection, track, localStreamRef.current);
				}
			}
		});
	}

	const switchCameraStatusAsync = async (cameraStatus) => {
		if (!localStreamRef.current) {
			return;
		}

		hubConnection.on("ReceiveRequestCameraStatus", async () => {
			await sendSignalAsync("SendCameraStatus", null, null, cameraStatus);

			handleVideoTracks();
		});

		await sendSignalAsync("SendCameraStatus", null, null, cameraStatus);

		if (cameraStatus) {
			navigator.mediaDevices?.getUserMedia({ video: true, audio: true }).then(localStream => {
				localStreamRef.current?.getVideoTracks().forEach(track => {
					localStreamRef.current.removeTrack(track);
				});

				localStream.getVideoTracks().forEach(track => {
					localStreamRef.current.addTrack(track);
				})

				handleVideoTracks();
			}).catch(e => console.log(e));
		} else {
			localStreamRef.current?.getVideoTracks().forEach(track => {
				track.stop();
				localStreamRef.current.removeTrack(track);

				for (const peerConnection of peerConnectionsRef.current.values()) {
					const sender = peerConnection.getSenders().find(s => s.track && s.track.kind === "video");
					if (sender) {
						peerConnection.removeTrack(sender);
					}
				}
			});
		}
	}

	const startScreenSharingAsync = async (screenSharingStatus, setScreenSharingIsActivated) => {
		if (!screenSharingStatus) {
			await stopScreenSharingAsync();

			return;
		}

		setScreenSharingIsActivated(true);

		hubConnection.on("ReceiveRequestScreenSharingStatus", async () => {
			await sendSignalAsync("SendScreenSharingStatus", null, null, true);

			handleVideoTracks(localStreamRef.current);
		});

		await sendSignalAsync("SendScreenSharingStatus", null, null, true);

		navigator.mediaDevices.getDisplayMedia({ video: true }).then(localStream => {
			localStreamRef.current?.getVideoTracks().forEach(track => {
				localStreamRef.current.removeTrack(track);
			});

			localStream.getVideoTracks().forEach(track => {
				localStreamRef.current.addTrack(track);
			})

			handleVideoTracks(localStream);

			localStreamRef.current.getVideoTracks()[0].addEventListener("ended", async () => {
				await stopScreenSharingAsync();

				setScreenSharingIsActivated(false);
			});
		}).catch((e) => {
			setScreenSharingIsActivated(false);

			console.log(e);
		});
	}

	const stopScreenSharingAsync = async () => {
		localStreamRef.current?.getVideoTracks().forEach(track => {
			track.stop();
			localStreamRef.current.removeTrack(track);
		});

		await sendSignalAsync("SendScreenSharingStatus", null, null, false);

		// Remove screen tracks from peer connections
		for (const peerConnection of peerConnectionsRef.current.values()) {
			const senders = peerConnection.getSenders().filter(sender => sender.track && sender.track.kind === "video");
			senders.forEach(sender => {
				peerConnection.removeTrack(sender);
			});
		}
	}

	const mediaRequestsAsync = async () => {
		await sendSignalAsync("SendRequestMicrophoneStatus");
		await sendSignalAsync("SendRequestCameraStatus");
		await sendSignalAsync("SendRequestScreenSharingStatus");
	}

	const stopMediaDataAsync = async () => {
		// Stop and remove all tracks
		connectionIsActiveRef.current = false;

		localStreamRef.current?.getTracks().forEach(track => {
			track.stop();
		});

		localStreamRef.current = null;

		await cleanupAsync();
	}

	return {
		properties: {
            hubConnection,
			peerConnectionsRef,
			localStreamRef,
		},
		methods: {
            connectToChatAsync,
            stopMediaDataAsync,
            switchMicrophoneStatusAsync,
			switchCameraStatusAsync,
			startScreenSharingAsync,
			mediaRequestsAsync,
			cleanupAsync,
        }
	};
}

export default useVoiceChatHub;