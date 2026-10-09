import type { RootState } from '@/app/Store';
import { MessageStatus } from '@/shared/helpers/EnumHelper';
import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as signalR from '@microsoft/signalr';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import type { ChatMessagePatch } from '../types/patches/ChatMessagePatch';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

interface DefaultChatMessageProps {
    message: GroupChatMessageModel | PersonalChatMessageModel;
    updateMessageAsync: (targetMessage: ChatMessagePatch) => Promise<void>;
    subscribeToChatMessageHasBeenRead: (callback: (targetMessageId: string) => void) => void;
    hubConnection: signalR.HubConnection | null;
    lastReadMessageId?: string;
}

const DefaultChatMessage: React.FC<DefaultChatMessageProps> = ({ message, updateMessageAsync, subscribeToChatMessageHasBeenRead, hubConnection, lastReadMessageId }) => {
    const { t } = useTranslation('communication/chats/chatMessage');

    const reviewer = useSelector((state: RootState) => state.user.value);

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const editMessageInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        subscribeToChatMessageHasBeenRead((targetMessageId: string) => {
            if (message.id !== targetMessageId) {
                return;
            }

            (async () => {
                if ("groupChatUserId" in message) {
                    await groupChatMessageHasBeenReadHandle();
                }
                else {
                    await personalChatMessageHasBeenReadHandle();
                }
            })();
        });
    }, []);

    useEffect(() => {
        if (!openMessageMenu) {
            setEditModeIsOn(false);
        }
    }, [openMessageMenu]);

    const handleUpdateMessageAsync = async () => {
        const updatedMessage: ChatMessagePatch = {
            id: message.id,
            message: editMessageInput.current?.value,
            chatId: "groupChatId" in message ? message.groupChatId : message.personalChatId
        }

        await updateMessageAsync(updatedMessage);

        setEditModeIsOn(false);
        setOpenMessageMenu(false);
    }

    const updateMessageMarkedTypeAsync = async (type: number) => {
        const updatedMessage: ChatMessagePatch = {
            id: message.id,
            markedType: type,
            chatId: "groupChatId" in message ? message.groupChatId : message.personalChatId
        }

        await updateMessageAsync(updatedMessage);
    }

    const handleOpenMessageMenu = () => {
        if ('appUserId' in message && reviewer?.id !== message.appUserId) {
            return;
        }

        setOpenMessageMenu((item) => !item);
    }

    const personalChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || ('appUserId' in message && reviewer?.id === message.appUserId)
            || message.status === MessageStatus["READ"]) {
            return;
        }

        await hubConnection.invoke("SendMessageHasBeenRead", message.id);
    }

    const groupChatMessageHasBeenReadHandle = async (): Promise<void> => {
        if (!hubConnection || ('appUserId' in message && reviewer?.id === message.appUserId)) {
            return;
        }

        await hubConnection.invoke("SendMessageHasBeenRead", message.id, reviewer?.id);
    }

    const chatMessageHasBeenReadHandle = async () => {
        if ("groupChatUserId" in message) {
            await groupChatMessageHasBeenReadHandle();
        }
        else {
            await personalChatMessageHasBeenReadHandle();
        }
    }

    const getMessageStatus = () => {
        switch (message.status) {
            case MessageStatus["SENDING"]:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
            case MessageStatus["SENT"]:
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                    title={t("Delivered") || ""}
                />;
            case MessageStatus["READ"]:
                return <FontAwesomeIcon
                    icon={faEye}
                    className="status"
                    title={t("Read") || ""}
                />;
            default:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                    title={t("Delivery") || ""}
                />;
        }
    }

    const openLink = (url: string) => {
        window.open(url, "_blink");
    }

    const getMessage = () => {
        let isAlreadyRead = message.status === MessageStatus["READ"];
        if ("appUserId" in message && reviewer?.id !== message.appUserId && lastReadMessageId) {
            isAlreadyRead = message.id <= lastReadMessageId;
        }
        else if ("appUserId" in message && reviewer?.id !== message.appUserId && !lastReadMessageId) {
            isAlreadyRead = false;
        }

        return (
            <div className={`text-of-message${isAlreadyRead ? "__read" : "__unread"} link 
                            ${message.markedType === 1 ? 'not-relevant' : message.markedType === 2 ? 'with-emotions' : ''}
                            ${openMessageMenu ? 'menu' : ''} `}
                            onMouseEnter={chatMessageHasBeenReadHandle}>
                <div className="content" onClick={handleOpenMessageMenu}>{message.message}</div>
                {message.message.startsWith("http") &&
                    <FontAwesomeIcon
                        icon={faUpRightFromSquare}
                        className="open-link"
                        onClick={() => openLink(message.message)}
                        title={t("OpenLink") || ""}
                    />
                }
            </div>
        );
    }

    return (
        <div className={`chat-messages__content${"appUserId" in message && reviewer?.id === message.appUserId ? ' my-message' : ''}`}>
            <ChatMessageTitle
                itIsMe={"appUserId" in message && reviewer?.id !== message.appUserId}
                message={message}
            />
            {editModeIsOn && "appUserId" in message && reviewer?.id === message.appUserId
                ? <div className="edit-message">
                    <input className="form-control" type="text" defaultValue={message.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save") || ""}
                        onClick={handleUpdateMessageAsync}
                    />
                </div>
                : <div className="message">
                    {"appUserId" in message && reviewer?.id === message.appUserId
                        ? getMessageStatus()
                        : message.status === MessageStatus["SENT"] &&
                        <FontAwesomeIcon
                            icon={faCircle}
                            className="status"
                            title={t("Delivered") || ""}
                        />
                    }
                    {getMessage()}
                    {message.markedType === 1 &&
                        <FontAwesomeIcon
                            icon={faXmark}
                            className="not-relevant"
                            title={t("NotRelevant") || ""}
                        />
                    }
                    {message.markedType === 2 &&
                        <FontAwesomeIcon
                            icon={faFaceMeh}
                            className="emotions"
                            title={t("WithEmotions") || ""}
                        />
                    }
                </div>
            }
            {message.isEdited &&
                <div className="chat-messages__edited">{t("Edited") || ""}</div>
            }
            {openMessageMenu &&
                <ChatMessageMenu
                    message={message}
                    setEditModeIsOn={setEditModeIsOn}
                    setOpenMessageMenu={setOpenMessageMenu}
                    updateMessageMarkedTypeAsync={updateMessageMarkedTypeAsync}
                />
            }
        </div>
    );
}

export default DefaultChatMessage;