import { faClose, faCloudArrowUp, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import ChatMessageUsername from './ChatMessageUsername';
import { useTranslation } from 'react-i18next';

import "../../../styles/communication/chatMessageItem.scss";

const ChatMessage = ({ customer, message, updateMessageAsync, deleteMessageAsync }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [showDeleteAlert, setShowDeleteAlert] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef(null);

    const updateMessageHandlerAsync = async () => {
        await updateMessageAsync(Object.assign({}, message), editMessageInput.current.value);
        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    return (
        <>
            {openMessageMenu &&
                <div className="message-menu">
                    <FontAwesomeIcon
                        icon={faPen}
                        title={t("Edit")}
                        className={`message-menu__handler${editModeIsOn && "_active"}`}
                        onClick={() => setEditModeIsOn((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faTrash}
                        title={t("Delete")}
                        className={`message-menu__handler${showDeleteAlert && "_active"}`}
                        onClick={() => setShowDeleteAlert((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faClose}
                        title={t("CloseMenu")}
                        onClick={() => setOpenMessageMenu((item) => !item)}
                    />
                </div>
            }
            {showDeleteAlert &&
                <div className="delete-message-alert">
                    <div className="delete-message-alert__content">{t("AreYouSure")}</div>
                    <div className="delete-message-alert__choice">
                        <FontAwesomeIcon
                            icon={faTrash}
                            title={t("Delete")}
                            onClick={async () => await deleteMessageAsync(message.id)}
                        />
                        <FontAwesomeIcon
                            icon={faClose}
                            title={t("Cancel")}
                            onClick={() => setShowDeleteAlert((item) => !item)}
                        />
                    </div>
                </div>
            }
            <div className={`chat-messages__${customer?.id === message?.ownerId ? "right" : "left"}`}>
                {customer?.id !== message?.ownerId &&
                    <ChatMessageUsername
                        messageOwnerId={message?.ownerId}
                    />
                }
                {editModeIsOn && customer?.id === message?.ownerId
                    ? <div className="edit-message">
                        <input className="form-control" defaultValue={message.message} ref={editMessageInput} />
                        <FontAwesomeIcon
                            icon={faCloudArrowUp}
                            title={t("Save")}
                            onClick={async () => await updateMessageHandlerAsync()}
                        />
                      </div>
                    : <div className="message" onClick={() => setOpenMessageMenu((item) => !item)}>{message?.message}</div>
                }
            </div>
        </>
    );
}

export default ChatMessage;