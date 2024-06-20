import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import {
    useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useCreateGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useCreateUnreadGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';

const SendGroupChatMessage = ({ chat, me, groupChatUsers, messageType }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [createUnreadGroupChatMessageAsyncMut] = useCreateUnreadGroupChatMessageAsyncMutation();

    const [isEmptyMessage, setIsEmptyMessage] = useState(false);

    const messageInput = useRef(null);

    const createMessageAsync = async (message, type) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: type,
            groupChatId: chat.id,
            customerId: me?.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined && type !== messageType["system"]) {
            await messageSentSuccessfulAsync(createdMessage.data);
        }
    }

    const updateGroupChatLastMessageAsync = async (message) => {
        chat.lastMessage = message;

        await updateGroupChatAsyncMut(chat);
    }

    const updateGroupChatMessagesCountAsync = async (count) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === me?.id) {
                continue;
            }

            const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: groupChatUsers[i].id });
            if (messagesCount.data !== undefined) {
                const newMessagesCount = Object.assign({}, messagesCount.data);
                newMessagesCount.count = newMessagesCount.count + count;

                await updateGroupChatMessageCountMut(newMessagesCount);
            }
        }
    }

    const createUnreadMessageAsync = async (messageId) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === me?.id) {
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

        const increaseUnreadMessages = 1;
        await updateGroupChatMessagesCountAsync(increaseUnreadMessages);

        await createUnreadMessageAsync(createdMessage.id);
    }

    const sendMessageAsync = useCallback(async () => {
        if (messageInput.current.value.length === 0) {
            setIsEmptyMessage(true);
            setTimeout(() => {
                setIsEmptyMessage(false);
            }, 3000);

            return;
        }

        await createMessageAsync(messageInput.current.value, messageType["default"]);
        messageInput.current.value = "";
    }, [messageInput]);

    const sendMessageByKeyAsync = async (e) => {
        if (messageInput.current.value.length === 0 && e.code === "Enter") {
            setIsEmptyMessage(true);
            setTimeout(() => {
                setIsEmptyMessage(false);
            }, 3000);

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

    return (
        <div className="send-message">
            <div className={`empty-message${isEmptyMessage ? "_show" : ""}`}>You can not send empty message</div>
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
    );
}

export default SendGroupChatMessage;