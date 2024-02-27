import { faAngleDown, faDisplay, faLinkSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import io from 'socket.io-client';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';
import VoiceChatMinimazedMain from './VoiceChatMinimazedMain';

import '../../../styles/voiceChatMinimazed.scss';

const VoiceChatMinimazed = ({ callMinimazedData, setUseMinimaze }) => {
    const me = useSelector((state) => state.customer.value);

    const [simpleVersion, setSimpleVersion] = useState(false);
    const [microphoneDeviceId, setMicrophoneDeviceId] = useState("");

    const voice = useVoice(me, callMinimazedData, microphoneDeviceId, setUseMinimaze);

    useEffect(() => {
        voice.data.socketRef.current = io.connect("192.168.0.161:2000");

        voice.data.socketRef.current.on("connect", () => {
            voice.data.socketRef.current.emit("updateSocketId", { roomId: callMinimazedData.current.roomId, socketId: callMinimazedData.current.socketId });

            voice.data.socketRef.current.on("socketIdUpdated", socketId => {
                voice.data.socketRef.current.id = socketId;
            });

            voice.func.switchCallType();
            voice.func.listen();
        });
    }, []);

    useEffect(() => {
        if (voice.data.screenSharing !== callMinimazedData.current.screenSharing) {
            voice.data.setScreenSharing(callMinimazedData.current.screenSharing);
        }
    }, [voice.data.screenSharing]);

    return (
        <div className="voice-chat-minimazed">
            {simpleVersion
                ? <div className="simple_version">
                    <div className="content">
                        <div className="voice-chat-minimazed__name">{callMinimazedData.current.roomName}</div>
                    </div>
                    <FontAwesomeIcon
                        icon={faAngleDown}
                        title="Show"
                        className="show"
                        onClick={() => setSimpleVersion(false)}
                    />
                </div>
                : <VoiceChatMinimazedMain
                    setUseMinimaze={setUseMinimaze}
                    microphoneDeviceId={microphoneDeviceId}
                    setSimpleVersion={setSimpleVersion}
                />
            }
            {voice.data.screenSharing
                ? <FontAwesomeIcon
                    icon={faLinkSlash}
                    className="sharing"
                    title="Stop screen scharing"
                    onClick={() => voice.func.shareScreen(false)}
                />
                : <FontAwesomeIcon
                    icon={faDisplay}
                    className="sharing"
                    title="Screen scharing"
                    onClick={() => voice.func.shareScreen(true)}
                />
            }
        </div>
    );
}

export default WithVoiceContext(VoiceChatMinimazed);