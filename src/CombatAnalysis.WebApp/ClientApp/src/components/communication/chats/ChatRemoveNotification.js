import { faUserXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useRemovePersonalChatAsyncMutation } from '../../../store/api/communication/chats/PersonalChat.api';

const ChatRemoveNotification = ({ chat, setSelectedChat, t }) => {
    const [removePersonalChatAsync] = useRemovePersonalChatAsyncMutation();

    const [showRemoveChatAlert, setShowRemoveChatAlert] = useState(false);

    const leaveFromChatAsync = async () => {
        const deletedItem = await removePersonalChatAsync(chat.id);
        if (deletedItem.data !== undefined) {
            setSelectedChat({ type: null, chat: null });
        }
    }

    return (
        <div>
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
                        <div className="btn-shadow remove" onClick={async () => await leaveFromChatAsync()}>{t("Remove")}</div>
                        <div className="btn-shadow cancel" onClick={() => setShowRemoveChatAlert((item) => !item)}>{t("Cancel")}</div>
                    </div>
                </div>
            }
        </div>
    );
}

export default ChatRemoveNotification;