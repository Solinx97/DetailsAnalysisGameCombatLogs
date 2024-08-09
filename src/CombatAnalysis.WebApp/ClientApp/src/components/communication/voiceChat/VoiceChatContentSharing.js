import { useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatContentSharing = ({ socket }) => {
	const [usersId, setUsersId] = useState([]);

	useEffect(() => {
		const fetchUsers = async () => {
			await callConnectedUsersAsync();
		}

		fetchUsers();
	}, []);

	useEffect(() => {
		if (socket === null) {
			return;
		}

		const handleMessageAsync = async (event) => {
			const message = event.data;
			if (message instanceof Blob) {
				return;
			}

			if (message.startsWith("joined")) {
				await callConnectedUsersAsync();
			}
		}

		socket.addEventListener("message", handleMessageAsync);

		return () => {
			socket.removeEventListener('message', handleMessageAsync);
		}
	}, [socket]);

	const callConnectedUsersAsync = async () => {
		try {
			const response = await fetch(`/api/v1/Signaling/connected`);
			const data = await response.json();

			setUsersId(data);
		} catch (error) {
			console.error('Error fetching users:', error);
		}
	}

    return (
		<div className="voice__content">
			<ul className="another-user-container">
				{usersId.map((userId, index) =>
					<li key={index}>
						<VoiceChatUser
							userId={userId}
							socket={socket}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default WithVoiceContext(VoiceChatContentSharing);