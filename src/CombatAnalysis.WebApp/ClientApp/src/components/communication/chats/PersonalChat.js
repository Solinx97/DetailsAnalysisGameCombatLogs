import { faPaperPlane, faPersonWalkingArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef } from "react";
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemovePersonalChatAsyncMutation, useUpdatePersonalChatAsyncMutation } from '../../../store/api/PersonalChat.api';
import { useFindPersonalChatMessageCountQuery, useUpdatePersonalChatMessageCountAsyncMutation } from '../../../store/api/PersonalChatMessagCount.api';
import {
    useCreatePersonalChatMessageAsyncMutation, useFindPersonalChatMessageByChatIdQuery, useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/PersonalChatMessage.api';
import ChatMessage from './ChatMessage';

import "../../../styles/communication/chats/personalChat.scss";

const getPersonalChatMessagesInterval = 1000;

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const messageInput = useRef(null);

    const { data: messages, isLoading } = useFindPersonalChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getPersonalChatMessagesInterval
    });
    const { data: companion, isLoading: companionIsLoading } = useGetCustomerByIdQuery(companionId);
    const [createPersonalChatMessageAsync] = useCreatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [removePersonalChatAsync] = useRemovePersonalChatAsyncMutation();
    const [updatePersonalChatAsyncMut] = useUpdatePersonalChatAsyncMutation();
    const [updatePersonalChatMessageCountMut] = useUpdatePersonalChatMessageCountAsyncMutation();
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: chat?.id, userId: companionId });
    const { data: myMessagesCount, isLoading: myMessagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: chat?.id, userId: me?.id });

    const deleteMessageAsync = async (messageId) => {
        await removePersonalChatMessageAsync(messageId);
    }

    const sendMessageAsync = async () => {
        if (messageInput.current?.value.length === 0) {
            return;
        }

        await createChatMessageAsync(messageInput.current.value);
        if (messageInput.current !== null) {
            messageInput.current.value = "";
        }
    }

    const sendMessageByKeyAsync = async (e) => {
        if (messageInput.current.value.length === 0
            || e.code !== "Enter") {
            return;
        }

        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            personalChatId: chat.id,
            ownerId: me?.id,
            status: 0
        };

        const createdMessage = await createPersonalChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined) {
            await updatePersonalChatAsync(message);

            const updateForMessage = Object.assign({}, createdMessage.data);
            updateForMessage.status = 1;

            await updatePersonalChatMessageAsync(updateForMessage);

            const increaseUnreadMessages = 1;
            await updateChatMessagesCountAsync(messagesCount, increaseUnreadMessages);
        }
    }

    const updatePersonalChatAsync = async (message) => {
        chat.lastMessage = message;

        await updatePersonalChatAsyncMut(chat);
    }

    const updateChatMessagesCountAsync = async (messagesCount, count) => {
        const newMessagesCount = Object.assign({}, messagesCount);
        newMessagesCount.count = newMessagesCount.count + count;

        await updatePersonalChatMessageCountMut(newMessagesCount);
    }

    const handleUpdatePersonalChatMessageAsync = async (message, count) => {
        const updated = await updatePersonalChatMessageAsync(message);
        if (updated.data !== undefined && count !== 0) {
            await updateChatMessagesCountAsync(myMessagesCount, count);
        }
    }

    const leaveFromChatAsync = async () => {
        const deletedItem = await removePersonalChatAsync(chat.id);
        if (deletedItem.data !== undefined) {
            setSelectedChat(null);
        }
    }

    if (isLoading || companionIsLoading
        || messagesCountLoading || myMessagesCountLoading) {
        return <></>;
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="name">{companion.username}</div>
                    <FontAwesomeIcon
                        icon={faPersonWalkingArrowRight}
                        title={t("LeaveFromChat")}
                        className="remove-chat-handler"
                        onClick={() => leaveFromChatAsync()}
                    />
                </div>
                <ul className="chat-messages">
                    {
                        messages?.map((item) => (
                            <li key={item.id}>
                                <ChatMessage
                                    me={me}
                                    message={item}
                                    messageStatus={item.status}
                                    updateMessageAsync={handleUpdatePersonalChatMessageAsync}
                                    deleteMessageAsync={deleteMessageAsync}
                                />
                            </li>
                        ))
                    }
                </ul>
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
        </div>
    );
}

export default PersonalChat;