import { useEffect, useState } from "react";
import { useUpdatePersonalChatAsyncMutation } from '../store/api/communication/chats/PersonalChat.api';
import { useFindPersonalChatMessageCountQuery, useUpdatePersonalChatMessageCountAsyncMutation } from '../store/api/communication/chats/PersonalChatMessagCount.api';
import {
    useCreatePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../store/api/communication/chats/PersonalChatMessage.api';

const useCreatePersonalChatMessage = (messageInput, chat, meId, companionId) => {
    const [createPersonalChatMessageAsync] = useCreatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatAsyncMut] = useUpdatePersonalChatAsyncMutation();
    const [updatePersonalChatMessageCountMut] = useUpdatePersonalChatMessageCountAsyncMutation();

    const { data: messagesCount, isLoading: messagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: chat?.id, userId: companionId });

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);
    const [emptyMessageTimeout, setEmptyMessageTimeout] = useState(null);

    const emptyMessageTimeoutNotification = 3000;

    useEffect(() => {
        return () => {
            clearTimeout(emptyMessageTimeout);
        }
    }, [emptyMessageTimeout]);

    const updatePersonalChatAsync = async (message) => {
        const unblockedObject = Object.assign({}, chat);
        unblockedObject.lastMessage = message;

        await updatePersonalChatAsyncMut(unblockedObject);
    }

    const updateChatMessagesCountAsync = async (messagesCount, count) => {
        const unblockedObject = Object.assign({}, messagesCount);
        unblockedObject.count = unblockedObject.count + count;

        await updatePersonalChatMessageCountMut(unblockedObject);
    }

    const sendMessageAsync = async () => {
        clearTimeout(emptyMessageTimeout);
        setIsEmptyMessage(false);

        if (messageInput?.current.value.length === 0) {
            setIsEmptyMessage(true);
            const timeout = setTimeout(() => {
                setIsEmptyMessage(false);
            }, emptyMessageTimeoutNotification);

            setEmptyMessageTimeout(timeout);

            return;
        }

        await createChatMessageAsync(messageInput?.current.value);
        if (messageInput?.current !== null) {
            messageInput.current.value = "";
        }
    }

    const sendMessageByKeyAsync = async (e) => {
        clearTimeout(emptyMessageTimeout);
        setIsEmptyMessage(false);

        if (messageInput?.current.value.length === 0 && e.code === "Enter") {
            setIsEmptyMessage(true);
            const timeout = setTimeout(() => {
                setIsEmptyMessage(false);
            }, emptyMessageTimeoutNotification);

            setEmptyMessageTimeout(timeout);

            return;
        }

        if (messageInput?.current.value.length >= 0 && e.code !== "Enter") {
            return;
        }

        await createChatMessageAsync(messageInput?.current.value);
        messageInput.current.value = "";
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: 0,
            personalChatId: chat.id,
            appUserId: meId
        };

        const createdMessage = await createPersonalChatMessageAsync(newMessage);
        if (createdMessage.data === undefined) {
            return;
        }

        await updatePersonalChatAsync(message);

        const unblockedObject = Object.assign({}, createdMessage.data);
        unblockedObject.status = 1;

        await updatePersonalChatMessageAsync(unblockedObject);

        const increaseUnreadMessages = 1;
        await updateChatMessagesCountAsync(messagesCount, increaseUnreadMessages);
    }

    return {
        sendMessageAsync,
        sendMessageByKeyAsync,
        isEmptyMessage,
        messagesCountLoading,
    }
}

export default useCreatePersonalChatMessage;