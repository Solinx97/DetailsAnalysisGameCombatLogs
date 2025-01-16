import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import ChatMessageTitle from './ChatMessageTitle';

import "../../../styles/communication/chats/chatMessage.scss";

const status = {
    delivery: 0,
    delivered: 1,
    read: 2
};

const DefaultChatMessage = ({ me, reviewerId, messageOwnerId, message, messageStatus, updateChatMessageAsync, deleteMessageAsync, hubConnection, unreadMessageHubConnection }) => {
    const { t } = useTranslation("communication/chats/chatMessage");

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef(null);

    useEffect(() => {
        if (!hubConnection || !unreadMessageHubConnection) {
            return;
        }

        hubConnection.on("ReceiveMessageHasBeenRead", async () => {
            await unreadMessageHubConnection?.invoke("RequestUnreadMessages", message.chatId, reviewerId);
        });

        return () => {
            if (hubConnection) {
                hubConnection.off("ReceiveMessageHasBeenRead");
            }
        }
    }, [hubConnection]);

    const handleUpdateMessageAsync = async () => {
        const updateForMessage = Object.assign({}, message);
        updateForMessage.message = editMessageInput.current.value;

        await updateChatMessageAsync(updateForMessage);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const updateMessageStatusAsync = async () => {
        if (reviewerId === messageOwnerId || messageStatus === status["read"]) {
            return;
        }

        await hubConnection?.invoke("SendMessageHasBeenRead", message.id, reviewerId);
    }

    const handleOpenMessageMenu = () => {
        if (reviewerId !== messageOwnerId) {
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
        <div className={`chat-messages__content${reviewerId === messageOwnerId ? ' my-message' : ''}`}>
            <ChatMessageTitle
                me={me}
                itIsMe={reviewerId !== messageOwnerId}
                deleteMessageAsync={deleteMessageAsync}
                setEditModeIsOn={setEditModeIsOn}
                openMessageMenu={openMessageMenu}
                editModeIsOn={editModeIsOn}
                message={message}
            />
            {editModeIsOn && reviewerId === messageOwnerId
                ? <div className="edit-message">
                    <input className="form-control" type="text" defaultValue={message.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save")}
                        onClick={handleUpdateMessageAsync}
                    />
                </div>
                : <div className="message"
                    onClick={handleOpenMessageMenu}>
                    {reviewerId === messageOwnerId
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
                            rel="noreferrer" onMouseOver={updateMessageStatusAsync}>{message?.message}</a>
                        : <div className={`text-of-message${messageStatus === status["delivered"] ? "__unread" : "__read"}`}
                            onMouseOver={updateMessageStatusAsync}>{message?.message}</div>
                    }
                </div>
            }
        </div>
    );
}

export default DefaultChatMessage;