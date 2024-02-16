import { faAngleDown, faAngleUp, faMicrophone, faMicrophoneSlash, faPhoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import io from 'socket.io-client';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';

import '../../../styles/voiceChatMinimazed.scss';

const VoiceChatMinimazed = ({ callMinimazedData }) => {
    const storeCallData = useSelector((state) => state.call.value);
    const me = useSelector((state) => state.customer.value);

    const navigate = useNavigate();

    const [hide, setHide] = useState(false);
    const [microphoneDeviceId, setMicrophoneDeviceId] = useState("");

    const voice = useVoice(me, callMinimazedData, microphoneDeviceId);

    useEffect(() => {
        if (storeCallData === undefined) {
            return;
        }

        voice.data.socketRef.current = io.connect("192.168.0.161:2000");

        voice.data.socketRef.current.on("connect", () => {
            voice.data.socketRef.current.emit("updateSocketId", { roomId: storeCallData.roomId, socketId: storeCallData.socketId });

            voice.data.socketRef.current.on("socketIdUpdated", socketId => {
                voice.data.socketRef.current.id = socketId;
            });
        });
    }, [storeCallData]);

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
                            {voice.data.turnOnCamera
                                ? <FontAwesomeIcon
                                    icon={faVideo}
                                    title="TurnOffCamera"
                                    className="device__camera"
                                    onClick={() => voice.func.switchCamera(false)}
                                />
                                : <FontAwesomeIcon
                                    icon={faVideoSlash}
                                    title="TurnOnCamera"
                                    className="device__camera"
                                    onClick={() => voice.func.switchCamera(true)}
                                />
                            }
                            {voice.data.turnOnMicrophone
                                ? <FontAwesomeIcon
                                    icon={faMicrophone}
                                    title="TurnOffMicrophone"
                                    className="device__microphone"
                                    onClick={() => voice.func.switchMicrophone(false)}
                                />
                                : <FontAwesomeIcon
                                    icon={faMicrophoneSlash}
                                    title="TurnOnMicrophone"
                                    className="device__microphone"
                                    onClick={() => voice.func.switchMicrophone(true)}
                                />
                            }
                            <FontAwesomeIcon
                                icon={faPhoneSlash}
                                title="Leave"
                                onClick={voice.data.leave}
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