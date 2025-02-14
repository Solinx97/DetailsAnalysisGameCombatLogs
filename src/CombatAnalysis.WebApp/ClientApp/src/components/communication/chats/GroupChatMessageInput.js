﻿import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef, useState } from 'react';

const GroupChatMessageInput = ({ hubConnection, chat, meId, setAreLoadingOldMessages, t }) => {
    const messageInput = useRef(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(null);

    const handleSendMessageByKeyAsync = async (event) => {
        if (event.code !== "Enter") {
            return;
        }
        else if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await hubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meId);

        messageInput.current.value = "";
    }

    const handleSendMessageAsync = async () => {
        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        setAreLoadingOldMessages(false);

        await hubConnection?.invoke("SendMessage", messageInput.current.value, chat.id, meId);

        messageInput.current.value = "";
    }

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, 4000);
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

export default memo(GroupChatMessageInput);