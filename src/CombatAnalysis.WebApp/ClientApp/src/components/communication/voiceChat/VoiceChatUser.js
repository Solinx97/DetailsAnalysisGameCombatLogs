import { faMicrophone, faMicrophoneSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from "react";
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';

const VoiceChatUser = ({ itIsMe, userId, socketRef, micStatus }) => {
    const [turnOnMicrophone, setTurnOnMicrophone] = useState(false);
    const [user, setUser] = useState(null);

    useEffect(() => {
        if (itIsMe) {
            setTurnOnMicrophone(micStatus);
        }

        const callUser = async () => {
            await callUserAsync(userId);
        }

        callUser();
    }, [micStatus, userId, itIsMe]);

    useEffect(() => {
        if (socketRef.current === null) {
            return;
        }

        const socket = socketRef.current;
        const handleMessageAsync = async (event) => {
            const message = event.data;
            if (message instanceof Blob) {
                return;
            }

            if (message.startsWith("requestedMicStatus")) {
                const message = `MIC_STATUS;${micStatus ? "on" : "off"}`;

                socket.send(message);
            }

            if (message.startsWith("microphoneStatus")) {
                const messageData = message.split(";");
                const updatedMicrophoneUserId = messageData[1];
                const status = messageData[2];

                if (updatedMicrophoneUserId === userId) {
                    setTurnOnMicrophone(status === "True");
                }
            }
        }

        socket.addEventListener("message", handleMessageAsync);

        return () => {
            socket.removeEventListener('message', handleMessageAsync);
        }
    }, [socketRef, userId, micStatus]);

    const callUserAsync = async (userId) => {
        try {
            const response = await fetch(`/api/v1/Account/${userId}`);
            const data = await response.json();

            setUser(data);
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    }

    if (user === null) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="user">
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