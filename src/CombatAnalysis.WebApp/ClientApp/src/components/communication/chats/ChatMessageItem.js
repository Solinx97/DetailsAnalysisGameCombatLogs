import { faClose, faCloudArrowUp, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';

import "../../../styles/communication/chatMessageItem.scss";

const ChatMessageItem = ({ customer, message, updateMessageAsync, deleteMessageAsync }) => {
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
                        title="Edit"
                        className={`message-menu__handler${editModeIsOn && "_active"}`}
                        onClick={() => setEditModeIsOn((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faTrash}
                        title="Delete"
                        className={`message-menu__handler${showDeleteAlert && "_active"}`}
                        onClick={() => setShowDeleteAlert((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faClose}
                        title="Close menu"
                        onClick={() => setOpenMessageMenu((item) => !item)}
                    />
                </div>
            }
            {showDeleteAlert &&
                <div className="delete-message-alert">
                    <div className="delete-message-alert__content">Are you sure?</div>
                    <div className="delete-message-alert__choice">
                        <FontAwesomeIcon
                            icon={faTrash}
                            title="Delete"
                            onClick={async () => await deleteMessageAsync(message.id)}
                        />
                        <FontAwesomeIcon
                            icon={faClose}
                            title="Cancel"
                            onClick={() => setShowDeleteAlert((item) => !item)}
                        />
                    </div>
                </div>
            }
            <div className={`chat-messages__${customer?.username === message?.username ? "right" : "left"}`}>
                {customer?.username !== message?.username &&
                    <div className="username">{message?.username}</div>
                }
                {editModeIsOn && customer?.username === message?.username
                    ? <div className="edit-message">
                        <input className="form-control" defaultValue={message.message} ref={editMessageInput} />
                        <FontAwesomeIcon
                            icon={faCloudArrowUp}
                            title="Save"
                            onClick={async () => await updateMessageHandlerAsync()}
                        />
                      </div>
                    : <div className="message" onClick={() => setOpenMessageMenu((item) => !item)}>{message?.message}</div>
                }
            </div>
        </>
    );
}

export default ChatMessageItem;