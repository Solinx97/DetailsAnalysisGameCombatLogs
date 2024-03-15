import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';

const VoiceChatUser = ({ callMinimazedData, itsMe, peer, peerId, socket, username, audio, setAudio, initTurnOnCamera, initTurnOnMicrophone, setSharingStatus, voice }) => {
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

        socket.on("openingNewSharingScreen", payload => {
            if (payload.peerId === peerId) {
                const stream = itsMe ? callMinimazedData.current.stream : currentStream;
                stream.getVideoTracks()[0].stop();

                voice.func.stopSharing();
            }
        });

        socket.on("screenSharingSwitched", payload => {
            if (payload.peerId === peerId) {
                setScreenSharing(payload.status);
            }
        });
    }, []);

    useEffect(() => {
        if (!itsMe || callMinimazedData.current.stream === null) { 
            return;
        }

        setVideoStream(callMinimazedData.current.stream);
    }, [turnOnCamera]);

    useEffect(() => {
        if (itsMe || (videoStreamRef.current === null || currentStream === null)) {
            return;
        }

        setVideoStream(currentStream);
    }, [turnOnCamera]);

    useEffect(() => {
        if (!itsMe && currentStream === null) {
            return;
        }

        const stream = itsMe ? callMinimazedData.current.stream : currentStream;
        const videoTracks = stream.getVideoTracks();
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

    const setVideoStream = (stream) => {
        const videoTracks = stream.getVideoTracks();
        if (videoTracks.length > 0 && turnOnCamera) {
            videoStreamRef.current.srcObject = stream;
            videoStreamRef.current.play();
        }
    }

    return (
        <div className={`user${turnOnCamera ? "_video" : ""}`}>
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
        </div>
    );
}

export default memo(WithVoiceContext(VoiceChatUser));