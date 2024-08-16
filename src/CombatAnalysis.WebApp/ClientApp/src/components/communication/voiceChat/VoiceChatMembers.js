import { useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import MeInVoiceChat from "./MeInVoiceChat";
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatMembers = ({ roomId, connection, peerConnectionsRef, micStatus, cameraStatus, stream  }) => {
	const [usersId, setUsersId] = useState([]);
	const [meId, setmMeId] = useState("");

	useEffect(() => {
		if (connection === null) {
			return;
		}

		callConnectedUsers();

		return () => {
			connection.off("ReceiveConnectionId");
			connection.off("Connected");
		};
	}, [connection, roomId]);

	useEffect(() => {
		if (meId === "" || connection === null || usersId === []) {
			return;
		}

		const handleReceiveConnectedUsers = async (connectedUsers) => {
			const anotherUsers = connectedUsers.filter((user) => user !== meId);
			setUsersId(anotherUsers);

			await connection.invoke("RequestMicrophoneStatus", roomId);
			await connection.invoke("RequestCameraStatus", roomId);
		}

		connection.on("ReceiveConnectedUsers", handleReceiveConnectedUsers);

		return () => {
			connection.off("ReceiveConnectedUsers", handleReceiveConnectedUsers);
		};
	}, [connection, meId, usersId, roomId]);

	const callConnectedUsers = () => {
		connection.on("Connected", async (userId) => {
			setmMeId(userId);

			await connection.invoke("RequestConnectedUsers", roomId);
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
							peerConnection={peerConnectionsRef.current.get(userId)}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default WithVoiceContext(VoiceChatMembers);