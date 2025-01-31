import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef } from "react";
import { useSelector } from 'react-redux';

const MeInVoiceChat = ({ micStatus, cameraStatus, localStream }) => {
    const me = useSelector((state) => state.user.value);

    const videosRef = useRef(null);

    useEffect(() => {
        if (!localStream) {
            return;
        }

        if (cameraStatus) {
            videosRef.current.srcObject = localStream;
            videosRef.current.muted = true;
            videosRef.current.autoplay = true;
        }
        else {
            videosRef.current= null;
        }
    }, [cameraStatus, localStream]);

    if (!me) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="user me">
            <div className="information">
                <div className="another__username">{me?.username}</div>
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