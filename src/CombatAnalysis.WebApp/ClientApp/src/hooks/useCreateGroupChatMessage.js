﻿import { useEffect, useState } from 'react';
import { useUpdateGroupChatAsyncMutation } from '../store/api/communication/chats/GroupChat.api';
import {
    useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useCreateGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../store/api/communication/chats/GroupChatMessage.api';
import {
    useCreateUnreadGroupChatMessageAsyncMutation
} from '../store/api/communication/chats/UnreadGroupChatMessage.api';

const useCreateGroupChatMessage = (messageInput, chat, meId, groupChatUsers, messageType) => {
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [createUnreadGroupChatMessageAsyncMut] = useCreateUnreadGroupChatMessageAsyncMutation();

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);
    const [emptyMessageTimeout, setEmptyMessageTimeout] = useState(null);

    const emptyMessageTimeoutNotification = 3000;

    useEffect(() => {
        return () => {
            clearTimeout(emptyMessageTimeout);
        }
    }, [emptyMessageTimeout]);

    const createMessageAsync = async (message, type) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: type,
            groupChatId: chat.id,
            customerId: meId
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined && type !== messageType["system"]) {
            await messageSentSuccessfulAsync(createdMessage.data);
        }
    }

    const updateGroupChatLastMessageAsync = async (message) => {
        const unblockedObject = Object.assign({}, chat);
        unblockedObject.lastMessage = message;

        await updateGroupChatAsyncMut(unblockedObject);
    }

    const increaseGroupChatMessagesCountAsync = async () => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === meId) {
                continue;
            }

            const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: groupChatUsers[i].id });
            if (messagesCount.data !== undefined) {
                const unblockedObject = Object.assign({}, messagesCount.data);
                unblockedObject.count = ++unblockedObject.count;

                await updateGroupChatMessageCountMut(unblockedObject);
            }
        }
    }

    const createUnreadMessageAsync = async (messageId) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === meId) {
                continue;
            }

            const newUnreadMessage = {
                groupChatUserId: groupChatUsers[i].id,
                groupChatMessageId: messageId
            }

            await createUnreadGroupChatMessageAsyncMut(newUnreadMessage);
        }
    }

    const messageSentSuccessfulAsync = async (createdMessage) => {
        await updateGroupChatLastMessageAsync(createdMessage.message);

        const updateForMessage = Object.assign({}, createdMessage);
        updateForMessage.status = 1;

        await updateGroupChatMessageAsync(updateForMessage);

        await increaseGroupChatMessagesCountAsync();

        await createUnreadMessageAsync(createdMessage.id);
    }

    const sendMessageAsync = async () => {
        clearTimeout(emptyMessageTimeout);
        setIsEmptyMessage(false);

        if (messageInput.current.value.length === 0) {
            setIsEmptyMessage(true);
            const timeout = setTimeout(() => {
                setIsEmptyMessage(false);
            }, emptyMessageTimeoutNotification);

            setEmptyMessageTimeout(timeout);

            return;
        }

        await createMessageAsync(messageInput.current.value, messageType["default"]);
        messageInput.current.value = "";
    }

    const sendMessageByKeyAsync = async (e) => {
        clearTimeout(emptyMessageTimeout);
        setIsEmptyMessage(false);

        if (messageInput.current.value.length === 0 && e.code === "Enter") {
            setIsEmptyMessage(true);
            const timeout = setTimeout(() => {
                setIsEmptyMessage(false);
            }, emptyMessageTimeoutNotification);

            setEmptyMessageTimeout(timeout);

            return;
        }

        if (messageInput.current.value.length >= 0 && e.code !== "Enter") {
            return;
        }

        await createMessageAsync(messageInput.current.value, messageType["default"]);

        if (messageInput.current !== null) {
            messageInput.current.value = "";
        }
    }

    return {
        sendMessageAsync,
        sendMessageByKeyAsync,
        isEmptyMessage,
    }
}

export default useCreateGroupChatMessage;