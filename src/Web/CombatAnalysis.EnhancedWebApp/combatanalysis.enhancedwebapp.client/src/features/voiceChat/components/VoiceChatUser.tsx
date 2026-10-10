import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as signalR from '@microsoft/signalr';
import { memo, useEffect, useRef, useState, type RefObject, type SetStateAction } from 'react';
import { useGetUserByIdentityUserIdQuery } from '../../user/api/Account.api';

interface VoiceChatUserProps {
    identityUserId: string;
    hubConnection: signalR.HubConnection | null;
    peerConnection: RTCPeerConnection | undefined;
    otherScreenSharingVideoRef: RefObject<HTMLVideoElement | null>;
    otherScreenSharingUserIdRef: RefObject<string | null>;
    otherScreenSharing: boolean;
    setOtherScreenSharing: (value: SetStateAction<boolean>) => void;
    audioOutputDeviceId: string;
}

const VoiceChatUser: React.FC<VoiceChatUserProps> = ({
    identityUserId,
    hubConnection,
    peerConnection,
    otherScreenSharingVideoRef,
    otherScreenSharingUserIdRef,
    otherScreenSharing,
    setOtherScreenSharing,
    audioOutputDeviceId
}) => {
    const { data: user } = useGetUserByIdentityUserIdQuery(identityUserId);

    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(false);
    const [stream, setStream] = useState(null);

    const videoContentRef = useRef<HTMLVideoElement | null>(null);
    const audioContentRef = useRef<HTMLAudioElement | null>(null);

    useEffect(() => {
        if (!peerConnection) {
            return;
        }

        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        peerConnection.addEventListener("track", (event: any) => {
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

        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const createAudio = async (event: any) => {
            const track = event.track;

            if (track.kind === "audio" && audioContentRef.current) {
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

        const handleReceiveMicrophoneStatus = (from: string, status: boolean) => {
            if (from === identityUserId) {
                setTurnOnMicrophone(status);
            }
        }

        const handleReceiveCameraStatus = (from: string, status: boolean) => {
            if (from === identityUserId) {
                setTurnOnCamera(status);
            }
        }

        const handleReceiveScreenSharingStatus = (from: string, status: boolean) => {
            if (from === identityUserId) {
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
        if (!audioOutputDeviceId || !audioContentRef.current) {
            return;
        }

        const setSinkId = async () => {
            await audioContentRef.current?.setSinkId(audioOutputDeviceId);
        }

        setSinkId();
    }, [audioOutputDeviceId]);

    useEffect(() => {
        if (!stream || !turnOnCamera || !videoContentRef.current) {
            return;
        }

        videoContentRef.current.srcObject = stream;
        videoContentRef.current.id = "activate";
        videoContentRef.current.muted = true;
        videoContentRef.current.autoplay = true;
    }, [stream, turnOnCamera]);

    useEffect(() => {
        if (!stream || !otherScreenSharing || !otherScreenSharingVideoRef.current) {
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