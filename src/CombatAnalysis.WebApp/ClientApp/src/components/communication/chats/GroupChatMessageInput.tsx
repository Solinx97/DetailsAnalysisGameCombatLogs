import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef, useState } from 'react';
import { GroupChatMessageInputProps } from '../../../types/components/communication/chats/GroupChatMessageInputProps';

const GroupChatMessageInput: React.FC<GroupChatMessageInputProps> = ({ hubConnection, chat, meId, setAreLoadingOldMessages, t }) => {
    const messageInput = useRef<any>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState<boolean | null>(null);

    const handleSendMessageByKeyAsync = async (e: any) => {
        if (e.code !== "Enter") {
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