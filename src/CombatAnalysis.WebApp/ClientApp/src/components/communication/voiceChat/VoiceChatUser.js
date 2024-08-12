import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ userId, connection, peerConnection }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);

    const videosRef = useRef(null);

    useEffect(() => {
        if (peerConnection === null) {
            return;
        }

        const handleReceiveMicrophoneStatus = async (from, status) => {
            if (from === userId) {
                setTurnOnMicrophone(status);
            }
        };

        const handleReceiveCameraStatus = async (from, status) => {
            if (from === userId) {
                cameraSwitch(status);
            }
        };

        connection.on("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
        connection.on("ReceiveCameraStatus", handleReceiveCameraStatus);

        return () => {
            connection.off("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
            connection.off("ReceiveCameraStatus", handleReceiveCameraStatus);
        };
    }, [connection, userId, peerConnection]);

    useEffect(() => {
        const addVideoTrack = (event) => {
            if (event.track.kind === "video") {
                videosRef.current.srcObject = event.streams[0];
                videosRef.current.play();
            }
        };

        if (turnOnCamera) {
            peerConnection.addEventListener("track", addVideoTrack);
        } else {
            peerConnection.removeEventListener("track", addVideoTrack);
            if (videosRef.current) {
                videosRef.current.srcObject = null;
            }
        }

        return () => {
            peerConnection.removeEventListener("track", addVideoTrack);
            if (videosRef.current) {
                videosRef.current.srcObject = null;
            }
        }
    }, [turnOnCamera]);

    const cameraSwitch = async (status) => {
        if (videosRef.current !== null && !status) {
            videosRef.current.pause();
            videosRef.current.currentTime = 0;

            videosRef.current = null;
        }

        setTurnOnCamera(status);
    }

    return (
        <div className="user">
            <div className="information">
                {/*<div className="another__username">{userId}</div>*/}
                <FontAwesomeIcon
                    icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            {turnOnCamera &&
                <video autoPlay ref={videosRef}></video>
            }
        </div>
    );
}

export default memo(VoiceChatUser);