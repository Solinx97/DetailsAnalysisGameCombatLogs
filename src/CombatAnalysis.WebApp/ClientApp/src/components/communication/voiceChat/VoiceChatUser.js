import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ userId, connection, peerConnection }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);

    const contentRef = useRef(null);

    useEffect(() => {
        if (connection === null) {
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

        connection.on("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
        connection.on("ReceiveCameraStatus", handleReceiveCameraStatus);

        return () => {
            connection.off("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
            connection.off("ReceiveCameraStatus", handleReceiveCameraStatus);
        };
    }, [connection, peerConnection, turnOnCamera, userId]);

    useEffect(() => {
        if (!turnOnCamera) {
            return;
        }

        const addVideoTrack = (event) => {
            if (event.track.kind === "video") {
                const video = document.createElement("video");
                video.srcObject = event.streams[0];
                video.muted = true;
                video.autoplay = true;
                video.playsInline = true;

                if (contentRef.current) {
                    contentRef.current.innerHTML = ""; // Clear any existing content
                    contentRef.current.appendChild(video);
                }
            }
        }

        if (turnOnCamera) {
            peerConnection.addEventListener("track", addVideoTrack);
        } else {
            peerConnection.removeEventListener("track", addVideoTrack);
            if (contentRef.current) {
                contentRef.current.innerHTML = "";
            }
        }

        return () => {
            peerConnection.addEventListener("track", addVideoTrack);
            if (contentRef.current) {
                contentRef.current.innerHTML = "";
            }
        };
    }, [turnOnCamera, userId]);

    return (
        <div className="user">
            <div className="information">
                <div className="another__username">{userId}</div>
                <FontAwesomeIcon
                    icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
            <div className="content" ref={contentRef}></div>
        </div>
    );
}

export default memo(VoiceChatUser);