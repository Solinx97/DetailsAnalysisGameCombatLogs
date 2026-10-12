import type { RootState } from '@/app/Store';
import ChatHubContext from '@/context/ChatHubContext';
import { MessageStatus } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { faCircle, faCircleUp, faClock, faCloudArrowUp, faEye, faFaceMeh, faUpRightFromSquare, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useReadGroupChatMessageMutation } from '../api/GroupChatMessage.api';
import { useReadPersonalChatMessageMutation } from '../api/PersonalChatMessage.api';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import type { ChatMessagePatch } from '../types/patches/ChatMessagePatch';
import type { GroupChatMessageReadPatch } from '../types/patches/GroupChatMessageReadPatch';
import type { PersonalChatMessageReadPatch } from '../types/patches/PersonalChatMessageReadPatch';
import ChatMessageMenu from './ChatMessageMenu';
import ChatMessageTitle from './ChatMessageTitle';

interface DefaultChatMessageProps {
    message: GroupChatMessageModel | PersonalChatMessageModel;
    updateMessageAsync: (targetMessage: ChatMessagePatch) => Promise<void>;
    lastReadMessageId?: string;
}

const DefaultChatMessage: React.FC<DefaultChatMessageProps> = ({ message, updateMessageAsync, lastReadMessageId }) => {
    const { t } = useTranslation('communication/chats/chatMessage');

    const groupChatUser = useSelector((state: RootState) => state.groupChatUser.value);
    const reviewer = useSelector((state: RootState) => state.user.value);

    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const [openMessageMenu, setOpenMessageMenu] = useState(false);
    const [editModeIsOn, setEditModeIsOn] = useState(false);

    const [readPersonalChatMessage] = useReadPersonalChatMessageMutation();
    const [readGroupChatMessage] = useReadGroupChatMessageMutation();

    const editMessageInput = useRef<HTMLInputElement | null>(null);

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

    const readPersonalChatMessageHandle = async (message: PersonalChatMessageModel) => {
        if (!reviewer) {
            return;
        }

        try {
            const path: PersonalChatMessageReadPatch = {
                id: message.id,
                chatId: message.personalChatId,
                readerId: reviewer?.id
            }

            await readPersonalChatMessage({ id: message.id, message: path }).unwrap();
        } catch (e) {
            logger.error("Failed read personal chat message", e);
        }
    }

    const readGroupChatMessageHandle = async (message: GroupChatMessageModel) => {
        if (!groupChatUser) {
            return;
        }

        try {
            const path: GroupChatMessageReadPatch = {
                id: message.id,
                chatId: message.groupChatId,
                readerId: groupChatUser.id
            }

            await readGroupChatMessage({ id: message.id, message: path }).unwrap();
        } catch (e) {
            logger.error("Failed read group chat message", e);
        }
    }

    const chatMessageHasBeenReadHandle = async () => {
        if ("groupChatUserId" in message) {
            await readGroupChatMessageHandle(message);
        }
        else {
            await readPersonalChatMessageHandle(message);
        }
    }

    const getMessageStatus = () => {
        switch (message.status) {
            case MessageStatus["SENDING"]:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                />;
            case MessageStatus["SENT"]:
                return <FontAwesomeIcon
                    icon={faCircleUp}
                    className="status"
                />;
            case MessageStatus["READ"]:
                return <FontAwesomeIcon
                    icon={faEye}
                    className="status"
                />;
            default:
                return <FontAwesomeIcon
                    icon={faClock}
                    className="status"
                />;
        }
    }

    const openLink = (url: string) => {
        window.open(url, "_blink");
    }

    const checkIfReviewerMessageOwner = () => {
        const isPersonalChatMessage = "appUserId" in message;
        const isOwner = isPersonalChatMessage
            ? reviewer?.id === message.appUserId
            : groupChatUser?.id === message.groupChatUserId;

        return isOwner;
    }

    const getMessage = () => {
        const isReviewerOwner = checkIfReviewerMessageOwner();

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
                onMouseEnter={isReviewerOwner ? () => { } : chatMessageHasBeenReadHandle}>
                <div className="content" onClick={handleOpenMessageMenu}>{message.message}</div>
                {message.message.startsWith("http") &&
                    <FontAwesomeIcon
                        icon={faUpRightFromSquare}
                        className="open-link"
                        onClick={() => openLink(message.message)}
                    />
                }
            </div>
        );
    }

    return (
        <div className={`chat-messages__content${checkIfReviewerMessageOwner() ? ' my-message' : ''}`}>
            <ChatMessageTitle
                itIsMe={checkIfReviewerMessageOwner()}
                message={message}
            />
            {editModeIsOn && checkIfReviewerMessageOwner()
                ? <div className="edit-message">
                    <input className="form-control" type="text" defaultValue={message.message} ref={editMessageInput} />
                    <FontAwesomeIcon
                        icon={faCloudArrowUp}
                        title={t("Save") || ""}
                        onClick={handleUpdateMessageAsync}
                    />
                </div>
                : <div className="message">
                    {checkIfReviewerMessageOwner()
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