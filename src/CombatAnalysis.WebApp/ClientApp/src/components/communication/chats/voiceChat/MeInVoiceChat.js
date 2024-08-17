import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef } from "react";

const MeInVoiceChat = ({ myId, micStatus, cameraStatus, localStream }) => {
    const videosRef = useRef(null);

    useEffect(() => {
        if (!localStream) {
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
        <div className="user me">
            <div className="information">
                <div className="another__username">{myId}</div>
                <FontAwesomeIcon
                    icon={micStatus ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            {cameraStatus &&
                <div className="media-content">
                    <video ref={videosRef} muted></video>
                </div>
            }
        </div>
    );
}

export default MeInVoiceChat;