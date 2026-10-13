import { ChatType } from '@/shared/helpers/EnumHelper';
import { faPaperPlane, faCancel } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
import useMessageCreate from '../hooks/useMessageCreate';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';

const emptyMessageNotificationTimeout = 4000;

interface MessageInputProps {
    page: number;
    chatId: number;
    initiator: (GroupChatUserModel | AppUserModel) | null;
    targetChatType: number;
    t: (key: string) => string;
}

const MessageInput: React.FC<MessageInputProps> = ({ page, chatId, initiator, targetChatType, t }) => {
    const messageInput = useRef<HTMLInputElement | null>(null);

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);

    const { createPersonalMessageAsync, createGrouplMessageAsync } = useMessageCreate(page);

    const sendMessageByKeyHandle = async (code: string) => {
        if (code !== "Enter") {
            return;
        }

        await sendMessageAsync();
    }

    const sendMessageAsync = async () => {
        if (!messageInput || !messageInput.current || !initiator) {
            return;
        }

        const message = messageInput.current.value;
        messageInput.current.value = "";
        if (message === "") {
            sentEmptyMessage();

            return;
        }

        if (targetChatType === ChatType["GROUP"]) {
            await createGrouplMessageAsync(initiator.id ?? "0", initiator.username, message, chatId);
        }
        else {
            await createPersonalMessageAsync(initiator.id ?? "0", initiator.username, message, chatId);
        }
    }

    const sentEmptyMessage = () => {
        setIsEmptyMessage(true);

        setTimeout(() => {
            setIsEmptyMessage(false);
        }, emptyMessageNotificationTimeout);
    };

    if (!initiator) {
        return (
            <div>
                <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
                <div className="form-group input-message">
                    <input type="text" className="form-control" disabled />
                    <FontAwesomeIcon
                        icon={faCancel}
                        color='orange'
                    />
                </div>
            </div>
        );
    }

    return (
        <div>
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={async (e) => await sendMessageByKeyHandle(e.code)} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    onClick={sendMessageAsync} />
            </div>
        </div>
    );
}

export default MessageInput;
