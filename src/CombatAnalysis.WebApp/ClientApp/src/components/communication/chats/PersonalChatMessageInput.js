import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef } from "react";
import useCreatePersonalChatMessage from '../../../hooks/useCreatePersonalChatMessage';

const PersonalChatMessageInput = ({ chat, meId, companionId, t }) => {
    const messageInput = useRef(null);

    const { sendMessageAsync, sendMessageByKeyAsync, isEmptyMessage, messagesCountLoading } = useCreatePersonalChatMessage(messageInput, chat, meId, companionId);

    if (messagesCountLoading) {
        return (<></>);
    }

    return (
        <div>
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

export default memo(PersonalChatMessageInput);