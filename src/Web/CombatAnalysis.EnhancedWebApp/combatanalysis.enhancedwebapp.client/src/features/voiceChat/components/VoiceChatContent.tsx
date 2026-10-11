import * as signalR from '@microsoft/signalr';
import { useEffect, useRef, useState, type RefObject, type SetStateAction } from 'react';
import AppUserInVoiceChat from './AppUserInVoiceChat';
import VoiceChatUser from './VoiceChatUser';

interface VoiceChatContentProps {
	roomId: string;
	hubConnection: signalR.HubConnection | null;
	peerConnections: Map<string, RTCPeerConnection>;
	stream: MediaStream | null;
	mediaRequestsAsync: () => Promise<void>;
	micStatus: boolean;
	cameraStatus: boolean;
	screenSharing: boolean;
	setScreenSharing: (value: SetStateAction<boolean>) => void;
	screenSharingVideoRef: RefObject<HTMLVideoElement | null>;
	audioOutputDeviceId: string;
}

const VoiceChatContent: React.FC<VoiceChatContentProps> = ({
	roomId,
	hubConnection,
	peerConnections,
	stream,
	mediaRequestsAsync,
	micStatus,
	cameraStatus,
	screenSharing,
	setScreenSharing,
	screenSharingVideoRef,
	audioOutputDeviceId
}) => {
	const [usersId, setUsersId] = useState<Set<string>>(new Set<string>());
	const [myId, setMyId] = useState("");
	const [otherScreenSharing, setOtherScreenSharing] = useState(false);

	const otherScreenSharingVideoRef = useRef(null);
	const otherScreenSharingUserIdRef = useRef("");

	useEffect(() => {
		if (!hubConnection) {
			return;
		}

		callConnectedUsers();
	}, [hubConnection]);

	useEffect(() => {
		if (otherScreenSharing) {
			setScreenSharing(false);
		}
	}, [otherScreenSharing]);

	useEffect(() => {
		if (!myId || !hubConnection) {
			return;
		}

		const handleUserLeft = (userId: string) => {
			if (userId === otherScreenSharingUserIdRef.current) {
				setOtherScreenSharing(false);
				otherScreenSharingUserIdRef.current = "";
			}

			const anotherUsers = new Set<string>(usersId);
			anotherUsers.delete(userId)
			setUsersId(anotherUsers);
		}

		const handleUserJoined = (userId: string) => {
			const anotherUsers = new Set<string>(usersId);
			anotherUsers.add(userId)
			setUsersId(anotherUsers);
		}

		const getConnectedUsersAsync = async () => {
			const connectedUsers = await hubConnection.invoke<string[]>("GetOtherConnectedUsers", roomId);
			setUsersId(new Set<string>(connectedUsers));

			await mediaRequestsAsync();
		}

		getConnectedUsersAsync();
		
		hubConnection.on("UserJoined", handleUserJoined);
		hubConnection.on("UserLeft", handleUserLeft);

		return () => {
			hubConnection.off("UserJoined", handleUserJoined);
			hubConnection.off("UserLeft", handleUserLeft);
		}
	}, [hubConnection, myId]);

	const callConnectedUsers = () => {
		hubConnection?.on("Connected", (userId) => {
			setMyId(userId);
		});
	}

    return (
		<div className="voice__content">
			{screenSharing &&
				<div className="sharing">
					<video ref={screenSharingVideoRef}></video>
				</div>
			}
			{otherScreenSharing &&
				<div className="sharing">
					<video ref={otherScreenSharingVideoRef}></video>
				</div>
			}
			<ul className={`users ${otherScreenSharing || screenSharing ? "sharing-content" : ""}`}>
				<li>
					<AppUserInVoiceChat
						micStatus={micStatus}
						cameraStatus={cameraStatus}
						localStream={stream}
					/>
				</li>
				{Array.from(usersId.entries()).map(([userId]) =>
					<li key={userId}>
						<VoiceChatUser
							userId={userId}
							hubConnection={hubConnection}
							peerConnection={peerConnections.get(userId)}
							otherScreenSharingVideoRef={otherScreenSharingVideoRef}
							otherScreenSharingUserIdRef={otherScreenSharingUserIdRef}
							otherScreenSharing={otherScreenSharing}
							setOtherScreenSharing={setOtherScreenSharing}
							audioOutputDeviceId={audioOutputDeviceId}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default VoiceChatContent;