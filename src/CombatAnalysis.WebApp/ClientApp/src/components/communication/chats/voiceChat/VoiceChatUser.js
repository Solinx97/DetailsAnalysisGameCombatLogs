import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";
import { useGetCallByIdQuery } from '../../../../store/api/communication/chats/VoiceChat.api';
import { useLazyGetUserByIdQuery } from '../../../../store/api/Account.api';

const VoiceChatUser = ({ userId, hubConnection, peerConnection, otherScreenSharingVideoRef, otherScreenSharingUserIdRef, otherScreenSharing, setOtherScreenSharing, audioOutputDeviceId }) => {
    const { data: voice } = useGetCallByIdQuery(userId);

    const [getUserByIdAsync] = useLazyGetUserByIdQuery();

    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);
    const [user, setUser] = useState(null);
    const [stream, setStream] = useState(null);

    const videoContentRef = useRef(null);
    const audioContentRef = useRef(null);

    useEffect(() => {
        if (!peerConnection) {
            return;
        }

        peerConnection.addEventListener("track", event => {
            const track = event.track;

            if (track.kind === "video") {
                setStream(event.streams[0]);
            }
        });
    }, [peerConnection]);

    useEffect(() => {
        if (!peerConnection) {
            return;
        }

        const createAudio = async (event) => {
            const track = event.track;

            if (track.kind === "audio") {
                audioContentRef.current.id = "active";
                audioContentRef.current.srcObject = new MediaStream([track]);
                audioContentRef.current.autoplay = true;
            }
        }

        peerConnection.addEventListener("track", createAudio);

        return () => {
            if (peerConnection) {
                peerConnection.removeEventListener("track", createAudio);
            }
        }
    }, [peerConnection, turnOnMicrophone, audioOutputDeviceId]);

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
    }, [hubConnection, otherScreenSharing]);

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

    useEffect(() => {
        if (!audioOutputDeviceId || !audioContentRef.current) {
            return;
        }

        const setSinkId = async () => {
            await audioContentRef.current.setSinkId(audioOutputDeviceId);
        }

        setSinkId();
    }, [audioOutputDeviceId]);

    useEffect(() => {
        if (!stream || !turnOnCamera) {
            return;
        }

        videoContentRef.current.srcObject = stream;
        videoContentRef.current.id = "activate";
        videoContentRef.current.muted = true;
        videoContentRef.current.autoplay = true;
    }, [stream, turnOnCamera]);

    useEffect(() => {
        if (!stream || !otherScreenSharing) {
            return;
        }

        otherScreenSharingVideoRef.current.srcObject = stream;
        otherScreenSharingVideoRef.current.id = "activate";
        otherScreenSharingVideoRef.current.muted = true;
        otherScreenSharingVideoRef.current.autoplay = true;
    }, [stream, otherScreenSharingVideoRef, otherScreenSharing]);

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