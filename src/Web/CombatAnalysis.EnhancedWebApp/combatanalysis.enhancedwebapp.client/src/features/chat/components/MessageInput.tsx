import { ChatMessageType } from '@/shared/helpers/EnumHelper';
import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
import useMessageCreate from '../hooks/useMessageCreate';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';

const emptyMessageNotificationTimeout = 4000;

interface MessageInputProps {
    chatId: number;
    initiator: (GroupChatUserModel | AppUserModel) | null;
    targetChatType: number;
    t: (key: string) => string;
}

const MessageInput: React.FC<MessageInputProps> = ({ chatId, initiator, targetChatType, t }) => {
    const messageInput = useRef<HTMLInputElement | null>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);

    const { createPersonalMessageAsync, createGrouplMessageAsync } = useMessageCreate();

    const sendMessageByKeyHandle = async (code: string) => {
        if (code !== "Enter") {
            return;
        }

        await sendMessageAsync();
    };

    const sendMessageHandle = async () => {
        await sendMessageAsync();
    };

    const sendMessageAsync = async () => {
        if (!messageInput || !messageInput.current || !initiator) {
            return;
        }

        if (messageInput.current.value === "") {
            sentEmptyMessage();

            return;
        }

        if (targetChatType === ChatMessageType["GROUP"]) {
            await createGrouplMessageAsync(initiator.id ?? "0", initiator.username, messageInput.current.value, chatId);
        }
        else {
            await createPersonalMessageAsync(initiator.id ?? "0", initiator.username, messageInput.current.value, chatId);
        }

        messageInput.current.value = "";
    };

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, emptyMessageNotificationTimeout);
    };

    return (
        <div>
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={async (e) => await sendMessageByKeyHandle(e.code)} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={sendMessageHandle} />
            </div>
        </div>
    );
}

export default MessageInput;
