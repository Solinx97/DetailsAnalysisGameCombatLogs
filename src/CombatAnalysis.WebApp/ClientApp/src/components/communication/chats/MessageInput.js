import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useChatHub } from '../../../context/ChatHubProvider';

const MessageInput = ({ chat, meInChat, setAreLoadingOldMessages, t }) => {
    const { personalChatMessagesHubConnection, subscribeToPersonalMessageDelivered } = useChatHub();

    const messageInput = useRef(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(null);

    useEffect(() => {
        subscribeToPersonalMessageDelivered(chat.id);
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

        await personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, 0, meInChat?.id, meInChat?.username);

        messageInput.current.value = "";
    }

    const handleSendMessageAsync = async () => {
        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await personalChatMessagesHubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, 0, meInChat?.id, meInChat?.username);

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