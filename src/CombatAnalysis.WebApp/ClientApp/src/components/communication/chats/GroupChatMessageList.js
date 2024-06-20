import { memo, useEffect, useState } from 'react';
import {
    useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useFindGroupChatUserQuery
} from '../../../store/api/communication/chats/GroupChatUser.api';
import {
    useLazyFindUnreadGroupChatMessageQuery,
    useRemoveUnreadGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';
import ChatMessage from './ChatMessage';

const GroupChatMessageList = ({ chat, messages, me }) => {
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeUnreadGroupChatMessageAsyncMut] = useRemoveUnreadGroupChatMessageAsyncMutation();
    const [findUnreadGroupChatMessageQ] = useLazyFindUnreadGroupChatMessageQuery();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();
    const [getMyMessageCountAsync] = useLazyFindGroupChatMessageCountQuery();

    const { data: meInChat, isLoading } = useFindGroupChatUserQuery({ chatId: chat?.id, userId: me?.id });

    const [myMessagesCount, setMyMessagesCount] = useState(null);

    useEffect(() => {
        const getMyMessageCount = async () => {
            const countObject = await getMyMessageCountAsync({ chatId: chat?.id, userId: meInChat?.id });
            if (countObject.data !== undefined) {
                setMyMessagesCount(countObject.data);
            }
        }

        getMyMessageCount();
    }, [meInChat]);

    const updateMyChatMessagesCountAsync = async (count) => {
        const newMessagesCount = Object.assign({}, myMessagesCount);
        newMessagesCount.count = newMessagesCount.count + count;

        await updateGroupChatMessageCountMut(newMessagesCount);
    }

    const removeUnreadMessageAsync = async (message) => {
        const unreadMessage = await findUnreadGroupChatMessageQ({ messageId: message.id, groupChatUserId: meInChat?.id });
        if (unreadMessage.data !== undefined && unreadMessage.data !== null) {
            await removeUnreadGroupChatMessageAsyncMut(unreadMessage.data.id);
        }
    }

    const handleUpdateGroupChatMessageAsync = async (message, count) => {
        const updated = await updateGroupChatMessageAsync(message);
        if (updated.data !== undefined && count !== 0) {
            await updateMyChatMessagesCountAsync(count);
            await removeUnreadMessageAsync(message);
        }
    }

    const deleteMessageAsync = async (messageId) => {
        await removeGroupChatMessageAsync(messageId);
    }

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <ul className="chat-messages">
            {messages?.map((message) => (
                <li key={message.id}>
                    <ChatMessage
                        me={me}
                        message={message}
                        messageStatus={message.status}
                        updateMessageAsync={handleUpdateGroupChatMessageAsync}
                        deleteMessageAsync={deleteMessageAsync}
                    />
                </li>
            ))}
        </ul>
    );
}

export default memo(GroupChatMessageList);