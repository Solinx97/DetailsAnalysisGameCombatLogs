import { useEffect, useRef, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import VoiceChatUser from "./VoiceChatUser";

const VoiceChatContentSharing = ({ callMinimazedData, voice }) => {
	const videoRef = useRef(null);

	const [sharingStatus, setSharingStatus] = useState({
		stream: null,
		itsMe: false,
		started: false,
		username: ""
	});

	useEffect(() => {
		getData(sharingStatus.stream, sharingStatus.itsMe);
	}, [sharingStatus]);

	const getData = (stream, itsMe) => {
		if (videoRef.current === null) {
			return;
		}

		videoRef.current.srcObject = itsMe ? callMinimazedData.current.stream : stream;
		videoRef.current.play();
	}

    return (
		<div className="voice__content">
			{sharingStatus.started &&
				<div className="sharing">
					<video playsInline ref={videoRef} muted />
					<div className="username">{sharingStatus.username}</div>
				</div>
			}
			<ul className={`${sharingStatus.started ? "another-user-container" : "members"}`}>
				{voice.data.peersRef.current?.map((peer, index) =>
					<li key={index}>
						<VoiceChatUser
							itsMe={index === 0}
							peer={peer.peer}
							peerId={peer.peerId}
							socket={voice.data.socketRef.current}
							username={peer?.username}
							audio={voice.data.anotherUsersAudio}
							setAudio={voice.data.setAnotherUsersAudio}
							initTurnOnCamera={peer?.turnOnCamera}
							initTurnOnMicrophone={peer?.turnOnMicrophone}
							setSharingStatus={setSharingStatus}
							voice={voice}
						/>
					</li>
				)}
			</ul>
		</div>
    );
}

export default WithVoiceContext(VoiceChatContentSharing);