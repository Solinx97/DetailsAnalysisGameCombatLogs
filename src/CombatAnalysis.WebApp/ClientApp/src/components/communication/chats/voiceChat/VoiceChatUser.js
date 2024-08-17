import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ userId, connection, peerConnection, otherScreenSharingVideoRef, otherScreenSharingUserIdRef, otherScreenSharing, setOtherScreenSharing }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);

    const videoContentRef = useRef(null);
    const audioContentRef = useRef(null);
    const videoStreamRef = useRef(null);

    useEffect(() => {
        if (turnOnCamera && videoContentRef && videoStreamRef.current) {
            videoContentRef.current.srcObject = videoStreamRef.current;
            videoContentRef.current.muted = true;
            videoContentRef.current.autoplay = true;
        }
    }, [peerConnection, turnOnMicrophone]);

    useEffect(() => {
        const createVideoElement = (event) => {
            const track = event.track;
            if (track.kind === "audio") {
                audioContentRef.current.srcObject = event.streams[0];
                audioContentRef.current.autoplay = true;
            }
        }

        peerConnection?.addEventListener("track", createVideoElement);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createVideoElement);
            }
        }
    }, [peerConnection, turnOnMicrophone]);

    useEffect(() => {
        const createVideoFromCamera = (event) => {
            const track = event.track;
            if (track.kind === "video") {
                videoStreamRef.current = event.streams[0];

                if (videoContentRef.current) {
                    videoContentRef.current.srcObject = event.streams[0];
                    videoContentRef.current.muted = true;
                    videoContentRef.current.autoplay = true;
                }
            }
        }

        peerConnection?.addEventListener("track", createVideoFromCamera);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createVideoFromCamera);
            }
        }
    }, [peerConnection, turnOnCamera]);

    useEffect(() => {
        const createVideoFromScreenSharing = (event) => {
            const track = event.track;
            if (track.kind === "video") {
                videoStreamRef.current = event.streams[0];

                if (otherScreenSharingVideoRef.current) {
                    otherScreenSharingVideoRef.current.srcObject = event.streams[0];
                    otherScreenSharingVideoRef.current.muted = true;
                    otherScreenSharingVideoRef.current.autoplay = true;
                }
            }
        }

        peerConnection?.addEventListener("track", createVideoFromScreenSharing);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createVideoFromScreenSharing);
            }
        }
    }, [peerConnection, otherScreenSharing]);

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

        const handleReceiveScreenSharingStatus = (from, status) => {
            if (from === userId) {
                if (status && !otherScreenSharing) {
                    otherScreenSharingUserIdRef.current = from;
                    setOtherScreenSharing(true);
                }
                else if (!status && otherScreenSharing && otherScreenSharingUserIdRef.current === from) {
                    setOtherScreenSharing(false);
                }
            }
        }

        connection.on("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
        connection.on("ReceiveCameraStatus", handleReceiveCameraStatus);
        connection.on("ReceiveScreenSharingStatus", handleReceiveScreenSharingStatus);

        return () => {
            connection.off("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
            connection.off("ReceiveScreenSharingStatus", handleReceiveCameraStatus);
            connection.off("ReceiveScreenSharingStatus", handleReceiveScreenSharingStatus);
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