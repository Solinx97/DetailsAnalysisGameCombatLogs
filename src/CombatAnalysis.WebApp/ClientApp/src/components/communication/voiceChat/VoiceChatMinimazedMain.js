import { faAngleUp, faMicrophone, faMicrophoneSlash, faPhoneSlash, faRightFromBracket, faVideo, faVideoSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import useVoice from '../../../hooks/useVoice';

const VoiceChatMinimazedMain = ({ callMinimazedData, setUseMinimaze, setSimpleVersion }) => {
    const me = useSelector((state) => state.customer.value);

    const navigate = useNavigate();

    const voice = useVoice(me, callMinimazedData, setUseMinimaze);

    const backToCall = () => {
        navigate(`/chats/voice/${callMinimazedData.current.roomId}/${callMinimazedData.current.roomName}`);
    }

    return (
        <div className="full-version">
            <div className="full-version content">
                <div className="full-version__name">{callMinimazedData.current.roomName}</div>
                <div className="full-version__tools">
                    {callMinimazedData.current.turnOnCamera
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
                    {callMinimazedData.current.turnOnMicrophone
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
                    <div className="leave">
                        <FontAwesomeIcon
                            icon={faPhoneSlash}
                            title="Leave"
                            onClick={() => voice.func.leave()}
                        />
                    </div>
                </div>
            </div>
            <FontAwesomeIcon
                icon={faAngleUp}
                title="Hide"
                className="hide"
                onClick={() => setSimpleVersion(true)}
            />
            <div className="btn-shadow back-to-call" title="Back to call" onClick={backToCall}>
                <FontAwesomeIcon
                    icon={faRightFromBracket}
                />
                <div>Back</div>
            </div>
        </div>
    );
}

export default WithVoiceContext(VoiceChatMinimazedMain);