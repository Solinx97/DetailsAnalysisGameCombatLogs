import { useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import MeInVoiceChat from "./MeInVoiceChat";
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatContentSharing = ({ connection, micStatus, cameraStatus, localStream, roomId, peerConnection }) => {
	const [usersId, setUsersId] = useState([]);
	const [meId, setmMeId] = useState("");

	useEffect(() => {
		if (connection === null) {
			return;
		}

		callConnectedUsers();
	}, [connection]);

	useEffect(() => {
		if (meId === "") {
			return;
		}

		connection.on("ReceiveConnectedUsers", (connectedUsers) => {
			const anotherUser = connectedUsers.filter((user) => user !== meId);
			setUsersId(anotherUser);
		});
	}, [meId]);

	const callConnectedUsers = () => {
		connection.on("UserJoined", async (user) => {
			setmMeId(user);

			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("UserLeft", async () => {
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
						localStream={localStream}
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

export default WithVoiceContext(VoiceChatContentSharing);