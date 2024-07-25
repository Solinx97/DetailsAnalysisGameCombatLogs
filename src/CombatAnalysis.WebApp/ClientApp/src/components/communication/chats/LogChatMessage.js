import { faArrowUpRightFromSquare } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import ChatMessageTitle from './ChatMessageTitle';
import { useNavigate } from 'react-router-dom';

import "../../../styles/communication/chats/chatMessage.scss";

const LogChatMessage = ({ me, message }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const navigate = useNavigate();

    const [parsedMessage, setParsedMessage] = useState("");
    const [date, setDate] = useState(null);
    const [combatLogId, setCombatLogId] = useState(0);

    const parseMessage = () => {
        const parse = message?.message.split(";");

        setCombatLogId(parse[0]);
        setParsedMessage(parse[1]);
        setDate(parse[2]);
    }

    const openCombatLog = () => {
        navigate(`/general-analysis?id=${combatLogId}`);
    }

    useState(() => {
        parseMessage();
    }, [])

    return (
        <div className="chat-messages__content">
            <ChatMessageTitle
                me={me}
                itIsMe={me?.id !== message?.appUserId}
                message={message}
            />
            <div className="log-message">
                <div className="name">
                    <div>{t("CombatsLog")}</div>
                    <div className="date">{date}</div>
                </div>

                <div>{parsedMessage}</div>
                <div className="btn-shadow" title={t("OpenCombatLog")} onClick={openCombatLog}>
                    <FontAwesomeIcon
                        icon={faArrowUpRightFromSquare}
                    />
                    <div>{t("Open")}</div>
                </div>
            </div>
        </div>
    );
}

export default LogChatMessage;