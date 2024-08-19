import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";
import { useGetCallByIdQuery } from '../../../../store/api/communication/chats/VoiceChat.api';
import { useLazyGetUserByIdQuery } from '../../../../store/api/Account.api';

const VoiceChatUser = ({ userId, hubConnection, peerConnection, otherScreenSharingVideoRef, otherScreenSharingUserIdRef, otherScreenSharing, setOtherScreenSharing, audioOutputDeviceIdRef }) => {
    const { data: voice } = useGetCallByIdQuery(userId);

    const [getUserByIdAsync] = useLazyGetUserByIdQuery();

    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);
    const [user, setUser] = useState(null);

    const videoContentRef = useRef(null);
    const audioContentRef = useRef(null);

    useEffect(() => {
        if (!peerConnection) {
            return;
        }

        const createAudio = async (event) => {
            const track = event.track;
            if (track.kind === "audio" && audioContentRef.current) {
                audioContentRef.current.id = "active";
                audioContentRef.current.srcObject = event.streams[0];
                audioContentRef.current.autoplay = true;

                if (audioOutputDeviceIdRef.current) {
                    await audioContentRef.current.setSinkId(audioOutputDeviceIdRef.current);
                }
            }
        }

        peerConnection.addEventListener("track", createAudio);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createAudio);
            }
        }
    }, [peerConnection, turnOnMicrophone]);

    useEffect(() => {
        if (!peerConnection || !turnOnCamera) {
            return;
        }

        const createVideoFromCamera = (event) => {
            const track = event.track;

            if (track.kind === "video" && videoContentRef.current) {
                videoContentRef.current.srcObject = new MediaStream([track]);
                videoContentRef.current.muted = true;
                videoContentRef.current.autoplay = true;
            }
        }

        peerConnection.addEventListener("track", createVideoFromCamera);
        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createVideoFromCamera);
            }
        }
    }, [peerConnection, turnOnCamera]);

    useEffect(() => {
        if (!peerConnection || !otherScreenSharing) {
            return;
        }

        const createVideoFromScreenSharing = (event) => {
            const track = event.track;
            if (track.kind === "video" && otherScreenSharingVideoRef.current) {
                otherScreenSharingVideoRef.current.srcObject = new MediaStream([track]);
                otherScreenSharingVideoRef.current.muted = true;
                otherScreenSharingVideoRef.current.autoplay = true;
            }
        }

        peerConnection.addEventListener("track", createVideoFromScreenSharing);
        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createVideoFromScreenSharing);
            }
        }
    }, [peerConnection, otherScreenSharing]);

    useEffect(() => {
        if (!hubConnection) {
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
                if (status) {
                    otherScreenSharingUserIdRef.current = from;
                    setOtherScreenSharing(true);
                }
                else if (!status && otherScreenSharing && otherScreenSharingUserIdRef.current === from) {
                    setOtherScreenSharing(false);
                }
            }
        }

        hubConnection.on("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
        hubConnection.on("ReceiveCameraStatus", handleReceiveCameraStatus);
        hubConnection.on("ReceiveScreenSharingStatus", handleReceiveScreenSharingStatus);

        return () => {
            hubConnection.off("ReceiveMicrophoneStatus", handleReceiveMicrophoneStatus);
            hubConnection.off("ReceiveCameraStatus", handleReceiveCameraStatus);
            hubConnection.off("ReceiveScreenSharingStatus", handleReceiveScreenSharingStatus);
        };
    }, [hubConnection, userId, otherScreenSharing]);

    useEffect(() => {
        if (!voice) {
            return;
        }

        const getUserById = async () => {
            const response = await getUserByIdAsync(voice?.userId);
            if (response.data) {
                setUser(response.data);
            }
        }

        getUserById();
    }, [voice]);

    return (
        <div className="user">
            <div className="information">
                {user
                    ? <>
                        <div className="another__username">{user.username}</div>
                        <FontAwesomeIcon
                            icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                            title="TurnOffMicrophone"
                        />
                    </>
                    : <>
                        <div className="another__username">Loading...</div>
                    </>
                }
            </div>
            <div className="media-content">
                <audio ref={audioContentRef} autoPlay></audio>
                {turnOnCamera &&
                    <video ref={videoContentRef} muted></video>
                }
            </div>
        </div>
    );
}

export default memo(VoiceChatUser);