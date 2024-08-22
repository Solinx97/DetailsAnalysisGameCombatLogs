import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef } from "react";
import useCreatePersonalChatMessage from '../../../hooks/useCreatePersonalChatMessage';

const PersonalChatMessageInput = ({ chat, meId, companionId, setAreLoadingOldMessages, t }) => {
    const messageInput = useRef(null);

    const { sendMessageAsync, isEmptyMessage, messagesCountLoading } = useCreatePersonalChatMessage(messageInput, chat, meId, companionId);

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

    if (messagesCountLoading) {
        return (<></>);
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

export default memo(PersonalChatMessageInput);