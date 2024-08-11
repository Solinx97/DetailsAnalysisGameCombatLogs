import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState, useRef } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';

const VoiceChatUser = ({ userId, connection, peerConnection }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);

    const videosRef = useRef(null);

    useEffect(() => {
        if (peerConnection === null) {
            return;
        }

        connection.on("ReceiveMicrophoneStatus", async (from, status) => {
            if (from === userId) {
                setTurnOnMicrophone(status);
            }
        });

        connection.on("ReceiveCameraStatus", async (from, status) => {
            if (from === userId) {
                setTurnOnCamera(status);
            }
        });
    }, [peerConnection]);

    useEffect(() => {
        const addVideoTrack = (event) => {
            if (event.track.kind === "video") {
                videosRef.current.srcObject = event.streams[0];
                videosRef.current.play();
            }
        }

        if (turnOnCamera) {
            peerConnection.addEventListener("track", addVideoTrack);
        }
        else {
            peerConnection.removeEventListener("track", addVideoTrack);
        }
    }, [turnOnCamera]);

    return (
        <div className="user">
            <div className="information">
                <div className="another__username">{userId}</div>
                <FontAwesomeIcon
                    icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            {turnOnCamera &&
                <video ref={videosRef}></video>
            }
        </div>
    );
}

export default memo(WithVoiceContext(VoiceChatUser));