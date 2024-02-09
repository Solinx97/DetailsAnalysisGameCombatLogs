import { memo, useEffect, useRef, useState } from "react";
import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const VoiceChatUser = ({ peer, socket, username, audio, setAudio }) => {
    const videoStreamRef = useRef(null);
    const audioStreamRef = useRef(null);

    const [currentStream, setCurrentStream] = useState(null);
    const [turnOnCamera, setTurnOnCamera] = useState(false);
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);

    useEffect(() => {
        peer.on("stream", stream => {
            setCurrentStream(stream);
        });

        socket.on("cameraSwitched", status => {
            setTurnOnCamera(status.cameraStatus);
        });

        socket.on("microphoneSwitched", status => {
            setTurnOnMicrophone(status.microphoneStatus);
        });
    }, []);

    useEffect(() => {
        if (videoStreamRef.current === null || currentStream === null) {
            return;
        }

        const videoTracks = currentStream.getVideoTracks();
        if (videoTracks.length > 0 && turnOnCamera) {
            videoStreamRef.current.srcObject = currentStream;
            videoStreamRef.current.play();
        }
    }, [currentStream, turnOnCamera]);

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

export default memo(VoiceChatUser);