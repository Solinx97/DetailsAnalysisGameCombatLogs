import logger from '@/utils/Logger';
import { faArrowLeft, faUserXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from 'react';
import { useRemovePersonalChatAsyncMutation } from '../../api/PersonalChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

interface PersonalChatTitleProps {
    chat: PersonalChatModel;
    companionUsername: string;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    t: (key: string) => string;
}

const PersonalChatTitle: React.FC<PersonalChatTitleProps> = ({ chat, companionUsername, setSelectedChat, t }) => {
    const [removePersonalChatAsync] = useRemovePersonalChatAsyncMutation();

    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);

    const leaveFromChatAsync = async () => {
        try {
            await removePersonalChatAsync(chat.id).unwrap();
            setSelectedChat(null);
        } catch (e) {
            logger.error("Failed to remove personal chat", e);
        }
    }

    return (
        <>
            <div className="title">
                <FontAwesomeIcon
                    icon={faArrowLeft}
                    onClick={() => setSelectedChat(null)}
                />
                <div className="name">{companionUsername}</div>
                <FontAwesomeIcon
                    icon={faUserXmark}
                    title={t("RemoveChat")}
                    className={`remove-chat${showRemoveChatAlert ? "_active" : ""}`}
                    onClick={() => setShowRemoveChatAlert((item) => !item)}
                />
                {showRemoveChatAlert &&
                    <div className="remove-chat-alert box-shadow">
                        <p>{t("AreYouSureRemoveChat")}</p>
                        <p>{t("ThatWillBeRemoveChat")}</p>
                        <div className="remove-chat-alert__actions">
                            <div className="btn-shadow remove" onClick={leaveFromChatAsync}>{t("Remove")}</div>
                            <div className="btn-shadow cancel" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("Cancel")}</div>
                        </div>
                    </div>
                }
            </div>
        </>
    );
}

export default PersonalChatTitle;