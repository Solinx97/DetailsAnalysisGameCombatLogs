import { useEffect, useRef, useState, memo } from "react";
import MeInVoiceChat from "./MeInVoiceChat";
import VoiceChatUser from "./VoiceChatUser";
import useRTCConnection from '../../../../hooks/useRTCConnection';

const VoiceChatContent = ({ roomId, hubConnection, peerConnections, stream, micStatus, cameraStatus, screenSharing, setScreenSharing, screenSharingVideoRef, audioOutputDeviceId  }) => {
	const [usersId, setUsersId] = useState([]);
	const [myId, setMyId] = useState("");
	const [otherScreenSharing, setOtherScreenSharing] = useState(false);
	const [isReady, setIsReady] = useState(false);

	const otherScreenSharingVideoRef = useRef(null);
	const otherScreenSharingUserIdRef = useRef("");

	const { setup, start, sendSignalAsync } = useRTCConnection();

	useEffect(() => {
		if (!hubConnection) {
			return;
		}

		setup(hubConnection, roomId);
		start();

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

		const handleReceiveConnectedUsers = async (connectedUsers) => {
			setIsReady(true);

			const anotherUsers = connectedUsers.filter((user) => user !== myId);
			setUsersId(anotherUsers);

			await sendSignalAsync("SendRequestMicrophoneStatus");
			await sendSignalAsync("SendRequestCameraStatus");
		}

		const handleUserLeft = async (userId) => {
			if (userId === otherScreenSharingUserIdRef.current) {
				setOtherScreenSharing(false);
				otherScreenSharingUserIdRef.current = "";
			}

			const anotherUsers = usersId.filter(element => element !== userId);
			setUsersId(anotherUsers);
		}

		const handleUserJoined = async (userId) => {
			const joinedUsers = Object.assign([], usersId);
			joinedUsers.push(userId);

			setUsersId(joinedUsers);
		}

		hubConnection.on("ReceiveConnectedUsers", handleReceiveConnectedUsers);
		hubConnection.on("UserJoined", handleUserJoined);
		hubConnection.on("UserLeft", handleUserLeft);

		return () => {
			hubConnection.off("ReceiveConnectedUsers", handleReceiveConnectedUsers);
			hubConnection.off("UserJoined", handleUserJoined);
			hubConnection.off("UserLeft", handleUserLeft);
		}
	}, [hubConnection, myId, usersId]);

	const callConnectedUsers = () => {
		hubConnection.on("Connected", async (userId) => {
			setMyId(userId);

			await sendSignalAsync("RequestConnectedUsers");
		});
	}

	if (!isReady) {
		return (<div>Loading...</div>);
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
					<MeInVoiceChat
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
							audioOutputDeviceId={audioOutputDeviceId}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default VoiceChatContent;