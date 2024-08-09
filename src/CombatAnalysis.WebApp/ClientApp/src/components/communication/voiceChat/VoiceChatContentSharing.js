import { useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatContentSharing = ({ me, socketRef, micStatus }) => {
	const [usersId, setUsersId] = useState([]);

	useEffect(() => {
		const fetchUsers = async () => {
			await callConnectedUsersAsync();
		}

		fetchUsers();
	}, []);

	useEffect(() => {
		if (socketRef.current === null) {
			return;
		}

		const socket = socketRef.current;
		const handleMessageAsync = async (event) => {
			const message = event.data;
			if (message instanceof Blob) {
				return;
			}

			if (message.startsWith("joined")) {
				await callConnectedUsersAsync();
			}

			if (message.startsWith("leaved")) {
				await callConnectedUsersAsync();
			}
		}

		socket.addEventListener("message", handleMessageAsync);

		return () => {
			socket.removeEventListener('message', handleMessageAsync);
		}
	}, [socketRef, usersId]);

	useEffect(() => {
		if (socketRef.current === null) {
			return;
		}

		socketRef.current.send("REQUEST_MIC_STATUS");
	}, [socketRef, usersId]);

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
				{usersId.map((userId) =>
					<li key={userId}>
						<VoiceChatUser
							itIsMe={me.id === userId}
							userId={userId}
							socketRef={socketRef}
							micStatus={micStatus}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default WithVoiceContext(VoiceChatContentSharing);