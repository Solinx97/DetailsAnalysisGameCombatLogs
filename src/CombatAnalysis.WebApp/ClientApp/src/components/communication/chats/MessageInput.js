import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";

const MessageInput = ({ hubConnection, unreadMessageHubConnection, chat, meInChat, setAreLoadingOldMessages, t }) => {
    const messageInput = useRef(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(null);

    useEffect(() => {
        if (!hubConnection) {
            return;
        }

        hubConnection.on("MessageDelivered", async () => {
            await unreadMessageHubConnection?.invoke("SendUnreadMessageIncreased", chat.id);
        });

        return () => {
            if (hubConnection) {
                hubConnection.off("MessageDelivered");
            }
        }
    }, [hubConnection]);

    const handleSendMessageByKeyAsync = async (event) => {
        if (event.code !== "Enter") {
            return;
        }
        else if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await hubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meInChat?.id, meInChat?.username);

        messageInput.current.value = "";
    }

    const handleSendMessageAsync = async () => {
        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await hubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meInChat?.id, meInChat?.username);

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