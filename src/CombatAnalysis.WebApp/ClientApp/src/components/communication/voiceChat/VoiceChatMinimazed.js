import { faAngleDown, faAngleUp, faMicrophone, faMicrophoneSlash, faPhoneSlash, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import io from 'socket.io-client';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import { updateCall } from '../../../store/slicers/CallSlice';

import '../../../styles/voiceChatMinimazed.scss';

const VoiceChatMinimazed = ({ callMinimazedData }) => {
    const callData = useSelector((state) => state.call.value);
    const me = useSelector((state) => state.customer.value);

    const dispatch = useDispatch();

    const [hide, setHide] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(callData === undefined ? false : callData.turnOnCamera);
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(callData === undefined ? false : callData.turnOnMicrophone);

    const socketRef = useRef(null);

    useEffect(() => {
        socketRef.current = io.connect("192.168.0.161:2000");

        socketRef.current.emit("getSocketId", { roomId: callData.roomId, userId: me.id });

        socketRef.current.on("gotSocketId", socketId => {
            socketRef.current.id = socketId;
        });
    }, []);

    const switchCamera = (cameraStatus) => {
        setTurnOnCamera(cameraStatus);

        if (!cameraStatus) {
            callMinimazedData.current.stream.getVideoTracks()[0].stop();
            socketRef.current.emit("cameraSwitching", { roomId: callData.roomId, cameraStatus });

            return;
        }

        navigator.mediaDevices.getUserMedia({ video: cameraStatus, audio: turnOnMicrophone }).then(stream => {
            callMinimazedData.current.peers.forEach(peer => {
                const peerStream = peer.streams[0];
                peer.replaceTrack(peerStream.getVideoTracks()[0], stream.getVideoTracks()[0], peerStream);
            });

            callMinimazedData.current.stream = stream;
        });

        socketRef.current.emit("cameraSwitching", { roomId: callData.roomId, cameraStatus });
    }

    const switchMicrophone = (microphoneStatus) => {
        setTurnOnMicrophone(microphoneStatus);

        if (!microphoneStatus) {
            callMinimazedData.current.stream.getAudioTracks()[0].stop();
            socketRef.current.emit("microphoneSwitching", { roomId: callData.roomId, microphoneStatus });

            return;
        }

        const constraints = {
            video: turnOnCamera,
            audio: microphoneStatus
        };

        navigator.mediaDevices.getUserMedia(constraints).then(stream => {
            callMinimazedData.current.peers.forEach(peer => {
                const peerStream = peer.streams[0];
                peer.replaceTrack(peerStream.getAudioTracks()[0], stream.getAudioTracks()[0], peerStream);
            });

            callMinimazedData.current.stream = stream;
        });

        socketRef.current.emit("microphoneSwitching", { roomId: callData.roomId, microphoneStatus });
    }

    const leave = () => {
        socketRef.current.emit("leavingFromRoom", { roomId: callData.roomId, username: me.username });

        socketRef.current.on("userLeft", () => {
            callMinimazedData.current.stream.getVideoTracks().forEach(track => {
                track.stop();
            });

            callMinimazedData.current.stream.getAudioTracks().forEach(track => {
                track.stop();
            });

            console.log(callMinimazedData.current);
            callMinimazedData.current.peers.forEach(peerRef => {
                peerRef.destroy();
            });

            socketRef.current.disconnect();

            const updateCallData = Object.assign({}, callData);
            updateCallData.useMinimaze = false;
            dispatch(updateCall(updateCallData));
        });
    }

    return (
        <>
            {hide
                ? <div className="voice-chat-minimazed_min">
                    <div className="voice-chat-minimazed_min content">
                        <div className="voice-chat-minimazed__name">{callData.roomName}</div>
                    </div>
                    <FontAwesomeIcon
                        icon={faAngleDown}
                        title="Show"
                        className="show"
                        onClick={() => setHide(!hide)}
                    />
                </div>
                : <div className="voice-chat-minimazed">
                    <div className="voice-chat-minimazed content">
                        <div className="voice-chat-minimazed__name">{callData.roomName}</div>
                        <div className="voice-chat-minimazed__tools">
                            {turnOnCamera
                                ? <FontAwesomeIcon
                                    icon={faVideo}
                                    title="TurnOffCamera"
                                    className="device__camera"
                                    onClick={() => switchCamera(false)}
                                />
                                : <FontAwesomeIcon
                                    icon={faVideoSlash}
                                    title="TurnOnCamera"
                                    className="device__camera"
                                    onClick={() => switchCamera(true)}
                                />
                            }
                            {turnOnMicrophone
                                ? <FontAwesomeIcon
                                    icon={faMicrophone}
                                    title="TurnOffMicrophone"
                                    className="device__microphone"
                                    onClick={() => switchMicrophone(false)}
                                />
                                : <FontAwesomeIcon
                                    icon={faMicrophoneSlash}
                                    title="TurnOnMicrophone"
                                    className="device__microphone"
                                    onClick={() => switchMicrophone(true)}
                                />
                            }
                            <FontAwesomeIcon
                                icon={faPhoneSlash}
                                title="Leave"
                                onClick={leave}
                            />
                        </div>
                    </div>
                    <FontAwesomeIcon
                        icon={faAngleUp}
                        title="Hide"
                        className="hide"
                        onClick={() => setHide(!hide)}
                    />
                </div>
            }
        </>
    );
}

export default WithVoiceContext(VoiceChatMinimazed);