import { useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatContentSharing = ({ me, connection, micStatus, roomId, peerConnection }) => {
	const [usersId, setUsersId] = useState([]);

	useEffect(() => {
		if (connection === null) {
			return;
		}

		callConnectedUsers();
	}, [connection]);

	const callConnectedUsers = () => {
		connection.on("UserJoined", async () => {
			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("UserLeft", async () => {
			await connection.invoke("RequestConnectedUsers", roomId);
		});

		connection.on("ReceiveConnectedUsers", (connectedUsers) => {
			setUsersId(connectedUsers);
		});
	}

    return (
		<div className="voice__content">
			<ul className="another-user-container">
				{usersId.map((userId) =>
					<li key={userId}>
						<VoiceChatUser
							itIsMe={me.id === userId}
							userId={userId}
							connection={connection}
							micStatus={micStatus}
							peerConnection={peerConnection}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default WithVoiceContext(VoiceChatContentSharing);