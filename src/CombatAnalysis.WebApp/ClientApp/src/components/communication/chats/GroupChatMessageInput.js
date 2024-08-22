import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef } from 'react';
import useCreateGroupChatMessage from '../../../hooks/useCreateGroupChatMessage';

const GroupChatMessageInput = ({ chat, me, groupChatUsers, messageType, setAreLoadingOldMessages, t }) => {
    const messageInput = useRef(null);

    const { sendMessageAsync, isEmptyMessage } = useCreateGroupChatMessage(messageInput, chat, me?.id, groupChatUsers, messageType);

    const handleSendMessageByKeyAsync = async (event) => {
        if (event.code !== "Enter") {
            return;
        }

        setAreLoadingOldMessages(false);

        await sendMessageAsync(event);
    }

    const handleSendMessageAsync = async () => {
        setAreLoadingOldMessages(false);

        await sendMessageAsync();
    }

    return (
        <div className="send-message">
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={handleSendMessageByKeyAsync} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={handleSendMessageAsync}
                />
            </div>
        </div>
    );
}

export default GroupChatMessageInput;