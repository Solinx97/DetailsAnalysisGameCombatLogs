import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ userId, connection, peerConnection, otherScreenSharingVideoRef, otherScreenSharing, setOtherScreenSharing }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);

    const videoContentRef = useRef(null);
    const audioContentRef = useRef(null);

    useEffect(() => {
        const createAudioElement = (event) => {
            let media = null;
            const track = event.track;

            if (track.kind === "video" && !otherScreenSharing) {
                media = videoContentRef.current;
                media.srcObject = new MediaStream([track]);
                media.muted = true;
                media.autoplay = true;
            } else if (track.kind === "audio") {
                media = audioContentRef.current;
                media.srcObject = new MediaStream([track]);
                media.autoplay = true;
            }
        }

        peerConnection.addEventListener("track", createAudioElement);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createAudioElement);
            }
        }
    }, [otherScreenSharing]);

    useEffect(() => {
        const createAudioElement = (event) => {
            const track = event.track;

            if (track.kind === "video" && otherScreenSharing) {
                otherScreenSharingVideoRef.current.srcObject = new MediaStream([track]);
                otherScreenSharingVideoRef.current.autoplay = true;
            }
        }

        peerConnection.addEventListener("track", createAudioElement);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createAudioElement);
            }
        }
    }, [otherScreenSharing]);

    useEffect(() => {
        if (!connection) {
            return;
        }

        const handleReceiveMicrophoneStatus = (from, status) => {
            if (from === userId) {
                setTurnOnMicrophone(status);
            }
        }

        const handleReceiveCameraStatus = (from, status) => {
            if (from === userId) {
                setTurnOnCamera(status);
            }
        }

        const handleReceiveScreenSharingtatus = (from, status) => {
            if (from === userId) {
                setOtherScreenSharing(status);
            }
        }

        connection.on("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
        connection.on("ReceiveCameraStatus", handleReceiveCameraStatus);
        connection.on("ReceiveScreenSharingStatus", handleReceiveScreenSharingtatus);

        return () => {
            connection.off("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
            connection.off("ReceiveScreenSharingStatus", handleReceiveCameraStatus);
            connection.off("ReceiveScreenSharingStatus", handleReceiveScreenSharingtatus);
        };
    }, [connection, userId]);

    return (
        <div className="user">
            <div className="information">
                <div className="another__username">{userId}</div>
                <FontAwesomeIcon
                    icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            <div className="media-content">
                <audio ref={audioContentRef}></audio>
                {turnOnCamera &&
                    <video ref={videoContentRef} muted></video>
                }
            </div>
        </div>
    );
}

export default memo(VoiceChatUser);