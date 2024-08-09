import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';

const VoiceChatUser = ({ userId, socket }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);

    useEffect(() => {
        if (socket === null) {
            return;
        }

        const handleMessageAsync = async (event) => {
            const message = event.data;
            if (message instanceof Blob) {
                return;
            }

            if (message.startsWith("microphoneStatus")) {
                const messageData = message.split(";");
                const userId = messageData[1];
                const status = messageData[2];

                setTurnOnMicrophone(status === "True");
            }
        }

        socket.addEventListener("message", handleMessageAsync);

        return () => {
            socket.removeEventListener('message', handleMessageAsync);
        }
    }, [socket]);

    return (
        <div className="user$">
            <div className="information">
                <div className="another__username">{userId}</div>
                <FontAwesomeIcon
                    icon={turnOnMicrophone ? faMicrophone : faMicrophoneSlash}
                    title="TurnOffMicrophone"
                />
            </div>
        </div>
    );
}

export default memo(WithVoiceContext(VoiceChatUser));