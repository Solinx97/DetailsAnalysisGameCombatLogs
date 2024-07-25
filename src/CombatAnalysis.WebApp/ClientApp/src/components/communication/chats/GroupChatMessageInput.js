import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef } from 'react';
import useCreateGroupChatMessage from '../../../hooks/useCreateGroupChatMessage';

const GroupChatMessageInput = ({ chat, me, groupChatUsers, messageType, t }) => {
    const messageInput = useRef(null);

    const { sendMessageAsync, sendMessageByKeyAsync, isEmptyMessage } = useCreateGroupChatMessage(messageInput, chat, me?.id, groupChatUsers, messageType);

    return (
        <div className="send-message">
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>{t("CanNotSendEmpty")}</div>
            <div className="form-group input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                    ref={messageInput} onKeyDown={async (event) => await sendMessageByKeyAsync(event)} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={async () => await sendMessageAsync()}
                />
            </div>
        </div>
    );
}

export default GroupChatMessageInput;