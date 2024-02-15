import { faAngleDown, faAngleUp, faMicrophone, faMicrophoneSlash, faPhoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import io from 'socket.io-client';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import { clear } from '../../../store/slicers/CallSlice';

import '../../../styles/voiceChatMinimazed.scss';

const VoiceChatMinimazed = ({ callMinimazedData }) => {
    const storeCallData = useSelector((state) => state.call.value);
    const me = useSelector((state) => state.customer.value);

    const navigate = useNavigate();

    const dispatch = useDispatch();

    const [hide, setHide] = useState(false);
    const [turnOnCamera, setTurnOnCamera] = useState(storeCallData === undefined ? false : storeCallData.turnOnCamera);
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(storeCallData === undefined ? false : storeCallData.turnOnMicrophone);

    const socketRef = useRef(null);

    useEffect(() => {
        if (storeCallData === undefined) {
            return;
        }

        socketRef.current = io.connect("192.168.0.161:2000");

        socketRef.current.on("connect", () => {
            socketRef.current.emit("updateSocketId", { roomId: storeCallData.roomId, socketId: storeCallData.socketId });

            socketRef.current.on("socketIdUpdated", socketId => {
                socketRef.current.id = socketId;
            });
        });
    }, [storeCallData]);

    const switchCamera = (cameraStatus) => {
        setTurnOnCamera(cameraStatus);

        if (!cameraStatus) {
            callMinimazedData.current.stream.getVideoTracks()[0].stop();
            socketRef.current.emit("cameraSwitching", { roomId: storeCallData.roomId, cameraStatus });

            return;
        }

        navigator.mediaDevices.getUserMedia({ video: cameraStatus, audio: turnOnMicrophone }).then(stream => {
            callMinimazedData.current.peers.forEach(peerRef => {
                const peerStream = peerRef.peer.streams[0];
                peerRef.peer.replaceTrack(peerStream.getVideoTracks()[0], stream.getVideoTracks()[0], peerStream);
            });

            callMinimazedData.current.stream = stream;
        });

        socketRef.current.emit("cameraSwitching", { roomId: storeCallData.roomId, cameraStatus });
    }

    const switchMicrophone = (microphoneStatus) => {
        setTurnOnMicrophone(microphoneStatus);

        if (!microphoneStatus) {
            callMinimazedData.current.stream.getAudioTracks()[0].stop();
            socketRef.current.emit("microphoneSwitching", { roomId: storeCallData.roomId, microphoneStatus });

            return;
        }

        const constraints = {
            video: turnOnCamera,
            audio: microphoneStatus
        };

        navigator.mediaDevices.getUserMedia(constraints).then(stream => {
            callMinimazedData.current.peers.forEach(peerRef => {
                const peerStream = peerRef.peer.streams[0];
                peerRef.peer.replaceTrack(peerStream.getAudioTracks()[0], stream.getAudioTracks()[0], peerStream);
            });

            callMinimazedData.current.stream = stream;
        });

        socketRef.current.emit("microphoneSwitching", { roomId: storeCallData.roomId, microphoneStatus });
    }

    const leave = () => {
        socketRef.current.emit("leavingFromRoom", { roomId: storeCallData.roomId, username: me.username });

        socketRef.current.on("userLeft", () => {
            callMinimazedData.current.stream.getVideoTracks().forEach(track => {
                track.stop();
            });

            callMinimazedData.current.stream.getAudioTracks().forEach(track => {
                track.stop();
            });

            callMinimazedData.current.peers.forEach(peerRef => {
                peerRef.destroy();
            });

            socketRef.current.disconnect();
            dispatch(clear());

            callMinimazedData.current.stream = null;
            callMinimazedData.current.peers = [];
        });
    }

    const backToCall = () => {
        navigate(`/chats/voice?roomId=${storeCallData.roomId}&chatName=${storeCallData.roomName}`);
    }

    return (
        <>
            {hide
                ? <div className="voice-chat-minimazed_min">
                    <div className="voice-chat-minimazed_min content">
                        <div className="voice-chat-minimazed__name">{storeCallData.roomName}</div>
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
                        <div className="voice-chat-minimazed__name">{storeCallData.roomName}</div>
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
                    <div className="btn-shadow back-to-call" title="Back to call" onClick={backToCall}>
                        <FontAwesomeIcon
                            icon={faRightFromBracket}
                        />
                        <div>Back</div>
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