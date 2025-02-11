import { faArrowUpRightFromSquare } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { LogChatMessageProps } from '../../../types/components/communication/chats/LogChatMessageProps';

import "../../../styles/communication/chats/chatMessage.scss";

const LogChatMessage: React.FC<LogChatMessageProps> = ({ message }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const navigate = useNavigate();

    const [parsedMessage, setParsedMessage] = useState("");
    const [date, setDate] = useState<string | null>(null);
    const [combatLogId, setCombatLogId] = useState(0);

    useEffect(() => {
        parseMessage();
    }, []);

    const parseMessage = () => {
        const parse = message.split(';');

        setCombatLogId(parseInt(parse[0], 10));
        setParsedMessage(parse[1]);
        setDate(parse[2]);
    }

    const openCombatLog = () => {
        navigate(`/general-analysis?id=${combatLogId}`);
    }

    return (
        <div className="chat-messages__content">
            <div className="log-message">
                <div className="name">
                    <div>{t("CombatsLog")}</div>
                    <div className="date">{date}</div>
                </div>
                <div>{parsedMessage}</div>
                <div className="btn-shadow" title={t("OpenCombatLog") || ""} onClick={openCombatLog}>
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