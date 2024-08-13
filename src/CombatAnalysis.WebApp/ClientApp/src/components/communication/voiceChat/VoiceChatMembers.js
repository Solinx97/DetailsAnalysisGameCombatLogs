import { useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import MeInVoiceChat from "./MeInVoiceChat";
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatMembers = ({ roomId, connection, peerConnection, micStatus, cameraStatus, stream, switchCameraStatusAsync, setCameraExecute  }) => {
	const [usersId, setUsersId] = useState([]);
	const [meId, setmMeId] = useState("");

	useEffect(() => {
		if (connection === null) {
			return;
		}

		callConnectedUsers();

		return () => {
			connection.off("ReceiveConnectionId");
			connection.off("UserJoined");
			connection.off("UserLeft");
			connection.off("ReceiveRequestMicrophoneStatus");
		};
	}, [connection, micStatus, cameraStatus, roomId]);

	useEffect(() => {
		if (meId === "" || connection === null) {
			return;
		}

		const handleReceiveConnectedUsers = async (connectedUsers) => {
			const anotherUser = connectedUsers.filter((user) => user !== meId);
			setUsersId(anotherUser);

			await connection.invoke("RequestMicrophoneStatus", roomId);
			await connection.invoke("RequestCameraStatus", roomId);
		}

		connection.on("ReceiveConnectedUsers", handleReceiveConnectedUsers);

		return () => {
			connection.off("ReceiveConnectedUsers", handleReceiveConnectedUsers);
		};
	}, [connection, meId, roomId]);

	const callConnectedUsers = () => {
		connection.on("ReceiveConnectionId", (connectionId) => {
			setmMeId(connectionId);
		});

		connection.on("UserJoined", async () => {
			await connection.invoke("RequestConnectionId", roomId);
			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("UserLeft", async () => {
			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("ReceiveRequestMicrophoneStatus", async () => {
			await connection.invoke("SendMicrophoneStatus", roomId, micStatus);
		});
	}

    return (
		<div className="voice__content">
			<ul className="another-user-container">
				<li>
					<MeInVoiceChat
						meId={meId}
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
							peerConnection={peerConnection}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default WithVoiceContext(VoiceChatMembers);