import { useEffect, useRef, useState } from "react";
import MeInVoiceChat from "./MeInVoiceChat";
import VoiceChatUser from "./VoiceChatUser";
import useRTCConnection from '../../../../hooks/useRTCConnection';

const VoiceChatContent = ({ roomId, hubConnection, peerConnections, stream, micStatus, cameraStatus, screenSharing, setScreenSharing, screenSharingVideoRef, audioOutputDeviceIdRef  }) => {
	const [usersId, setUsersId] = useState([]);
	const [myId, setMyId] = useState("");
	const [otherScreenSharing, setOtherScreenSharing] = useState(false);

	const otherScreenSharingVideoRef = useRef(null);
	const otherScreenSharingUserIdRef = useRef("");

	const { setup, start, sendSignalAsync } = useRTCConnection();

	useEffect(() => {
		if (!hubConnection) {
			return;
		}

		setup(hubConnection);
		start();
	}, [hubConnection]);

	useEffect(() => {
		if (!hubConnection) {
			return;
		}

		callConnectedUsers();

		return () => {
			hubConnection.off("ReceiveConnectionId");
			hubConnection.off("Connected");
		}
	}, [hubConnection, roomId]);

	useEffect(() => {
		if (!myId || !hubConnection) {
			return;
		}

		const handleReceiveConnectedUsers = async (connectedUsers) => {
			const anotherUsers = connectedUsers.filter((user) => user !== myId);
			setUsersId(anotherUsers);

			await sendSignalAsync("SendRequestCameraStatus");
		}

		const handleUserLeft = async (userId) => {
			if (userId === otherScreenSharingUserIdRef.current) {
				setOtherScreenSharing(false);
				otherScreenSharingUserIdRef.current = "";
			}
		}

		hubConnection.on("ReceiveConnectedUsers", handleReceiveConnectedUsers);
		hubConnection.on("UserLeft", handleUserLeft);

		return () => {
			hubConnection.off("ReceiveConnectedUsers", handleReceiveConnectedUsers);
			hubConnection.off("UserLeft", handleUserLeft);
		}
	}, [hubConnection, myId]);

	useEffect(() => {
		if (otherScreenSharing) {
			setScreenSharing(false);
		}
	}, [otherScreenSharing]);

	const callConnectedUsers = () => {
		hubConnection.on("Connected", async (userId) => {
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
			<ul className="users">
				<li>
					<MeInVoiceChat
						myId={myId}
						micStatus={micStatus}
						cameraStatus={cameraStatus}
						localStream={stream}
					/>
				</li>
				{usersId.map((userId) =>
					<li key={userId}>
						<VoiceChatUser
							userId={userId}
							hubConnection={hubConnection}
							peerConnection={peerConnections.get(userId)}
							otherScreenSharingVideoRef={otherScreenSharingVideoRef}
							otherScreenSharingUserIdRef={otherScreenSharingUserIdRef}
							otherScreenSharing={otherScreenSharing}
							setOtherScreenSharing={setOtherScreenSharing}
							audioOutputDeviceIdRef={audioOutputDeviceIdRef}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default VoiceChatContent;