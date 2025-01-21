import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useChatHub } from '../../../context/ChatHubProvider';

const chatType = {
    personal: 0,
    group: 1
};

const MessageInput = ({ chat, meInChat, setAreLoadingOldMessages, targetChatType, t }) => {
    const { personalChatMessagesHubConnection, groupChatMessagesHubConnection, subscribeToPersonalMessageDelivered, subscribeToGroupMessageDelivered } = useChatHub();

    const messageInput = useRef(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(null);

    useEffect(() => {
        if (targetChatType === chatType["personal"]) {
            subscribeToPersonalMessageDelivered(chat.id);
        }
        else {
            subscribeToGroupMessageDelivered(chat.id);
        }
    }, []);

    const handleSendMessageByKeyAsync = async (event) => {
        if (event.code !== "Enter") {
            return;
        }
        else if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        if (targetChatType === chatType["personal"]) {
            await personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meInChat?.id, meInChat?.username);
        }
        else {
            await groupChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, 0, meInChat?.id, meInChat?.username);
        }

        messageInput.current.value = "";
    }

    const handleSendMessageAsync = async () => {
        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        if (targetChatType === chatType["personal"]) {
            await personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meInChat?.id, meInChat?.username);
        }
        else {
            await groupChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, 0, meInChat?.id, meInChat?.username);
        }

        messageInput.current.value = "";
    }

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, 4000);
    }

    return (
        <div>
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

export default MessageInput;