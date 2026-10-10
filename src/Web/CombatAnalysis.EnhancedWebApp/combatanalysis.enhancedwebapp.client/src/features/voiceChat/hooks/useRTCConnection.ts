import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import { useRef, type SetStateAction } from 'react';

const useRTCConnection = () => {
	const hubConnectionRef = useRef<signalR.HubConnection | null>(null);
	const roomIdRef = useRef<string | null>(null);
	const localStreamRef = useRef<MediaStream | null>(null);

	const peerConnectionsRef = useRef<Map<string, RTCPeerConnection>>(new Map());

	const myConnectionIdRef = useRef(null);
	const haveControllBarRef = useRef<(value: SetStateAction<boolean>) => void | null>(null);
	const connectionIsActiveRef = useRef(false);

	const config = {
		iceServers: [{ urls: 'stun:stun.l.google.com:19302' }]
	};

	const setup = (hubConnection: signalR.HubConnection, roomId: string, peerConnections: Map<string, RTCPeerConnection>, setHaveControllBar: (value: SetStateAction<boolean>) => void) => {
		hubConnectionRef.current = hubConnection;
		roomIdRef.current = roomId;
		peerConnectionsRef.current = peerConnections;
		haveControllBarRef.current = setHaveControllBar;

		return connectionIsActiveRef.current;
	}

	const startAsync = async () => {
		await createStreamAsync();

		connectionIsActiveRef.current = true;
	}

	const createStreamAsync = async () => {
		const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
		localStreamRef.current = stream;

		stream.getAudioTracks().forEach(track => (track.enabled = false));
	}

	const listeningSignalMessages = () => {
		hubConnectionRef.current?.on("Connected", (identityUserId) => {
			myConnectionIdRef.current = identityUserId;
			if (haveControllBarRef.current) {
				haveControllBarRef.current(true);
			}
		});

		hubConnectionRef.current?.on("UserJoined", (identityUserId) => {
			getOrCreatePeerConnection(identityUserId);
		});

		hubConnectionRef.current?.on("UserLeft", (identityUserId) => {
			const peerConnection = peerConnectionsRef.current?.get(identityUserId);
			if (peerConnection) {
				peerConnection.close();
				peerConnectionsRef.current?.delete(identityUserId);
			}
		});

		hubConnectionRef.current?.on("ReceiveConnectedUsers", (connectedUsers) => {
			for (const userId of connectedUsers) {
				if (myConnectionIdRef.current && userId !== myConnectionIdRef.current) {
					getOrCreatePeerConnection(userId);
				}
			}
		});
	}

	const listeningAnswersAsync = async () => {
		hubConnectionRef.current?.on("ReceiveOffer", async (fromConnectionId, offer) => {
			const peerConnection = getOrCreatePeerConnection(fromConnectionId);
			await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));
			const answer = await peerConnection.createAnswer();
			await peerConnection.setLocalDescription(answer);

			await sendSignalAsync("SendAnswer", fromConnectionId, JSON.stringify(peerConnection.localDescription));
		});

		hubConnectionRef.current?.on("ReceiveAnswer", async (fromConnectionId, answer) => {
			const peerConnection = peerConnectionsRef.current?.get(fromConnectionId);
			if (peerConnection) {
				await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
			}
		});

		hubConnectionRef.current?.on("ReceiveCandidate", async (identityUserId, candidate) => {
			const peerConnection = peerConnectionsRef.current?.get(identityUserId);
			if (peerConnection) {
				await peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
			}
		});
	}

	const getOrCreatePeerConnection = (identityUserId: string) => {
		let peerConnection = peerConnectionsRef.current?.get(identityUserId);
		if (peerConnection) {
			return peerConnection;
		}

		peerConnection = new RTCPeerConnection(config);
		peerConnectionsRef.current?.set(identityUserId, peerConnection);

		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		peerConnection.addEventListener("negotiationneeded", async (e: any) => {
			const pc = e.currentTarget;

			const offer = await pc.createOffer();
			await pc.setLocalDescription(offer);

			await sendSignalAsync("SendOffer", null, JSON.stringify(pc.localDescription));
		});

		// eslint-disable-next-line @typescript-eslint/no-explicit-any
		peerConnection.addEventListener("icecandidate", async (e: any) => {
			if (e.candidate) {
				await sendSignalAsync("SendCandidate", identityUserId, JSON.stringify(e.candidate));
			}
		});

		navigator.mediaDevices.getUserMedia({ audio: true }).then(stream => {
			localStreamRef.current?.getAudioTracks().forEach(track => {
				track.stop();
				localStreamRef.current?.removeTrack(track);
			});

			stream.getAudioTracks().forEach(track => {
				localStreamRef.current?.addTrack(track);
			});

			localStreamRef.current?.getTracks().forEach(track => {
				addTrackToPeer(peerConnection, track, localStreamRef.current);
			});
		})

		return peerConnection;
	}

	const sendSignalAsync = async (message: string, userId?: string | null, content?: string | null, status?: boolean | null) => {
		try {
			if (!hubConnectionRef.current || !connectionIsActiveRef.current) {
				return;
			}

			const roomId = !roomIdRef.current ? "" : roomIdRef.current;
			if (message && userId && content && (status !== undefined && status !== null)) {
				await hubConnectionRef.current.invoke(message, roomId, userId, content, status);
			}
			else if (message && userId && content) {
				await hubConnectionRef.current.invoke(message, roomId, userId, content);
			}
			else if (message && userId && (status !== undefined && status !== null)) {
				await hubConnectionRef.current.invoke(message, roomId, userId, status);
			}
			else if (message && userId) {
				await hubConnectionRef.current.invoke(message, roomId, userId);
			}
			else if (message && (status !== undefined && status !== null)) {
				await hubConnectionRef.current.invoke(message, roomId, status);
			}
			else {
				await hubConnectionRef.current.invoke(message, roomId);
			}
		} catch (e) {
			logger.error(`Failed to send signal '${message}' from Voice chat`, e);
		}
	}

	const cleanup = () => {
		try {
			if (!peerConnectionsRef.current) {
				if (hubConnectionRef.current) {
					hubConnectionRef.current.stop().then(() => {
						hubConnectionRef.current = null;
					});
				}

				myConnectionIdRef.current = null;

				return;
			}

			// Close all peer connections
			for (const peerConnection of peerConnectionsRef.current.values()) {
				const senders = peerConnection.getSenders();
				senders.forEach((sender: RTCRtpSender) => {
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
			logger.error("Error during cleanup:", error);
		}
	}

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	const addTrackToPeer = (peerConnection: any, track: any, stream: MediaStream | null) => {
		peerConnection.addTrack(track, stream);
	}

	return { localStreamRef, setup, startAsync, listeningSignalMessages, listeningAnswersAsync, sendSignalAsync, cleanup, addTrackToPeer };
}

export default useRTCConnection;