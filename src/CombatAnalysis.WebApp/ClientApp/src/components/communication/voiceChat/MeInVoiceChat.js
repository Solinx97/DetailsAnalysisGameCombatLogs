import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef } from "react";

const MeInVoiceChat = ({ meId, micStatus, cameraStatus, localStream }) => {
    const videosRef = useRef(null);

    useEffect(() => {
        if (localStream === null) {
            return;
        }

        if (cameraStatus) {
            videosRef.current.srcObject = localStream;
            videosRef.current.muted = true;
            videosRef.current.play();
        }
        else {
            videosRef.current= null;
        }
    }, [cameraStatus, localStream]);

    return (
        <div className="user">
            <div className="information">
                <div className="another__username">{meId}</div>
                <FontAwesomeIcon
                    icon={micStatus ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            {cameraStatus &&
                <video ref={videosRef}></video>
            }
        </div>
    );
}

export default MeInVoiceChat;