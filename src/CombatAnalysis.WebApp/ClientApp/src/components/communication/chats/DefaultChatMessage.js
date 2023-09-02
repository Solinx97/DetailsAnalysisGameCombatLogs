import { faCircleUp, faClock, faCloudArrowUp, faEye, faCircle } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import ChatMessageTitle from './ChatMessageTitle';

import "../../../styles/communication/chats/chatMessage.scss";

const status = {
    "delivery": 0,
    "delivered": 1,
    "read": 2
};

const DefaultChatMessage = ({ me, message, messageStatus, updateMessageAsync, deleteMessageAsync }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef(null);

    const handleUpdateMessageAsync = async () => {
        const updateForMessage = Object.assign({}, message);
        updateForMessage.message = editMessageInput.current.value;

        await updateMessageAsync(updateForMessage, 0);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const updateMessageStatusAsync = async () => {
        if (message.customerId === me?.id || messageStatus === status["read"]) {
            return;
        }

        const updateForMessage = Object.assign({}, message);
        updateForMessage.status = 2;

        await updateMessageAsync(updateForMessage, -1);
    }

    const handleOpenMessageMenu = () => {
        if (me?.id !== message?.customerId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    const getMessageStatus = () => {
        switch (message.status) {
            case status["delivery"]:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery")}
                />;
            case status["delivered"]:
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                    title={t("Delivered")}
                />;
            case status["read"]:
                return <FontAwesomeIcon
                    icon={faEye}
                    className="status"
                    title={t("Read")}
                />;
            default:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery")}
                />;
        }
    }

    return (
        <div className="chat-messages__content"
            onMouseOver={handleOpenMessageMenu}
            onMouseOut={handleOpenMessageMenu}>
            <ChatMessageTitle
                me={me}
                itIsMe={me?.id !== message?.customerId}
                deleteMessageAsync={deleteMessageAsync}
                setEditModeIsOn={setEditModeIsOn}
                openMessageMenu={openMessageMenu}
                editModeIsOn={editModeIsOn}
                message={message}
            />
            {editModeIsOn && me?.id === message?.customerId
                ? <div className="edit-message">
                    <input className="form-control" defaultValue={message.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save")}
                        onClick={async () => await handleUpdateMessageAsync()}
                    />
                </div>
                : <div className="message">
                    {message?.customerId === me?.id
                        ? getMessageStatus()
                        : messageStatus === status["delivered"] &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered")}
                        />
                    }
                    {message?.message.startsWith("http")
                        ? <a className="text-of-message link" href={message?.message} target="_blank"
                            rel="noreferrer" onMouseOver={async () => await updateMessageStatusAsync()}>{message?.message}</a>
                        : <div className={`text-of-message${messageStatus === status["delivered"] ? "__unread" : "__read"}`}
                            onMouseOver={async () => await updateMessageStatusAsync()}>{message?.message}</div>
                    }
                </div>
            }
        </div>
    );
}

export default DefaultChatMessage;