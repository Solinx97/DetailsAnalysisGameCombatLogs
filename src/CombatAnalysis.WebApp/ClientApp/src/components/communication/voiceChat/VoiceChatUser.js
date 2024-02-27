import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';

const VoiceChatUser = ({ callMinimazedData, itsMe, peer, peerId, socket, username, audio, setAudio, initTurnOnCamera, initTurnOnMicrophone, setSharingStatus }) => {
    const videoStreamRef = useRef(null);
    const audioStreamRef = useRef(null);

    const [currentStream, setCurrentStream] = useState(null);
    const [turnOnCamera, setTurnOnCamera] = useState(initTurnOnCamera);
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(initTurnOnMicrophone);
    const [screenSharing, setScreenSharing] = useState(false);

    useEffect(() => {
        peer.on("stream", stream => {
            setCurrentStream(stream);
        });

        socket.on("cameraSwitched", payload => {
            if (payload.peerId === peerId) {
                setTurnOnCamera(payload.status);
            }
        });

        socket.on("microphoneSwitched", payload => {
            if (payload.peerId === peerId) {
                setTurnOnMicrophone(payload.status);
            }
        });

        socket.on("screenSharingSwitched", payload => {
            if (payload.peerId === peerId) {
                setScreenSharing(payload.status);
            }
        });
    }, []);

    useEffect(() => {
        if (!itsMe) {
            return;
        }

        setCurrentStream(callMinimazedData.current.stream);
    }, [itsMe]);

    useEffect(() => {
        if (videoStreamRef.current === null || currentStream === null) {
            return;
        }

        const videoTracks = currentStream.getVideoTracks();
        if (videoTracks.length > 0 && turnOnCamera) {
            videoStreamRef.current.srcObject = currentStream;
            videoStreamRef.current.play();
        }
    }, [turnOnCamera]);

    useEffect(() => {
        if (currentStream === null) {
            return;
        }

        const videoTracks = currentStream.getVideoTracks();
        if (videoTracks.length > 0) {
            const status = {
                stream: currentStream,
                itsMe: itsMe,
                started: screenSharing,
                username
            };

            setSharingStatus(status);
        }
    }, [screenSharing]);

    useEffect(() => {
        if (audioStreamRef.current === null || currentStream === null) {
            return;
        }

        const audioTracks = currentStream.getAudioTracks();
        if (audioTracks.length > 0 && turnOnMicrophone) {
            audioStreamRef.current.srcObject = currentStream;
            audioStreamRef.current.play();

            const allAudio = audio;
            allAudio.push(audioStreamRef.current);

            setAudio(allAudio);
        }
    }, [currentStream, turnOnMicrophone]);

    return (
        <>
            {turnOnCamera
                ? <video className="another__video" playsInline ref={videoStreamRef} muted />
                : turnOnMicrophone && <audio ref={audioStreamRef} />
            }
            <div className="information">
                <div className="another__username">{username}</div>
                {turnOnMicrophone
                    ? <FontAwesomeIcon
                        icon={faMicrophone}
                        title="TurnOffMicrophone"
                    />
                    : <FontAwesomeIcon
                        icon={faMicrophoneSlash}
                        title="TurnOffMicrophone"
                    />
                }
            </div>
        </>
    );
}

export default memo(WithVoiceContext(VoiceChatUser));