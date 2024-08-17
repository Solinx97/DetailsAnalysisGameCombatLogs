import { useEffect, useRef, useState } from "react";
import MeInVoiceChat from "./MeInVoiceChat";
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatContent = ({ roomId, connection, peerConnectionsRef, stream, micStatus, cameraStatus, screenSharing, setScreenSharing, screenSharingVideoRef  }) => {
	const [usersId, setUsersId] = useState([]);
	const [myId, setMyId] = useState("");
	const [otherScreenSharing, setOtherScreenSharing] = useState(false);

	const otherScreenSharingVideoRef = useRef(null);

	useEffect(() => {
		if (!connection) {
			return;
		}

		callConnectedUsers();

		return () => {
			connection.off("ReceiveConnectionId");
			connection.off("Connected");
		}
	}, [connection, roomId]);

	useEffect(() => {
		if (!myId || !connection) {
			return;
		}

		const handleReceiveConnectedUsers = async (connectedUsers) => {
			const anotherUsers = connectedUsers.filter((user) => user !== myId);
			setUsersId(anotherUsers);
		}

		connection.on("ReceiveConnectedUsers", handleReceiveConnectedUsers);

		return () => {
			connection.off("ReceiveConnectedUsers", handleReceiveConnectedUsers);
		}
	}, [connection, myId, roomId]);

	useEffect(() => {
		if (otherScreenSharing) {
			setScreenSharing(false);
		}
	}, [otherScreenSharing]);

	const callConnectedUsers = () => {
		connection.on("Connected", async (userId) => {
			setMyId(userId);

			await connection.invoke("RequestConnectedUsers", roomId);
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
							connection={connection}
							peerConnection={peerConnectionsRef.current.get(userId)}
							otherScreenSharingVideoRef={otherScreenSharingVideoRef}
							otherScreenSharing={otherScreenSharing}
							setOtherScreenSharing={setOtherScreenSharing}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default VoiceChatContent;