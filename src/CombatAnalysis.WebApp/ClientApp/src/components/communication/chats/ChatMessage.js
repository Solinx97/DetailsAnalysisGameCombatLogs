import { faCloudArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import ChatMessageTitle from './ChatMessageTitle';

import "../../../styles/communication/chats/chatMessage.scss";

const ChatMessage = ({ customer, message, updateMessageAsync, deleteMessageAsync }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef(null);

    const updateMessageHandlerAsync = async () => {
        await updateMessageAsync(Object.assign({}, message), editMessageInput.current.value);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const handleOpenMessageMenu = () => {
        if (customer?.id !== message?.ownerId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    return (
        <div className="chat-messages__content"
            onMouseOver={handleOpenMessageMenu}
            onMouseOut={handleOpenMessageMenu}>
            <ChatMessageTitle
                itIsMe={customer?.id !== message?.ownerId}
                deleteMessageAsync={deleteMessageAsync}
                setEditModeIsOn={setEditModeIsOn}
                openMessageMenu={openMessageMenu}
                editModeIsOn={editModeIsOn}
                message={message}
            />
            {editModeIsOn && customer?.id === message?.ownerId
                ? <div className="edit-message">
                    <input className="form-control" defaultValue={message.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save")}
                        onClick={async () => await updateMessageHandlerAsync()}
                    />
                </div>
                : message?.message.startsWith("http")
                    ? <a className="message link" href={message?.message} target="_blank" rel="noreferrer">{message?.message}</a>
                    : <div className="message">{message?.message}</div>
            }
        </div>
    );
}

export default ChatMessage;