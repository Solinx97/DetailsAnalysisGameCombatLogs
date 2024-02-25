import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";

const VoiceChatUser = ({ peer, peerId, socket, username, audio, setAudio, initTurnOnCamera, initTurnOnMicrophone }) => {
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
        if (videoStreamRef.current === null || currentStream === null) {
            return;
        }

        const videoTracks = currentStream.getVideoTracks();
        if (videoTracks.length > 0 && (turnOnCamera || screenSharing)) {
            videoStreamRef.current.srcObject = currentStream;
            videoStreamRef.current.play();
        }
    }, [currentStream, turnOnCamera, screenSharing]);

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
        <div className="another">
            {(turnOnCamera || screenSharing)
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

export default memo(VoiceChatUser);