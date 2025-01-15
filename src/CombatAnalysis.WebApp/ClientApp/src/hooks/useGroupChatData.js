import { useEffect, useState } from 'react';
import { useGetGroupChatMessageCountByChatIdQuery } from '../store/api/chat/GroupChatMessage.api';
import { useFindGroupChatUserByChatIdQuery, useFindMeInChatQuery } from '../store/api/chat/GroupChatUser.api';
import { useGetMessagesByGroupChatIdQuery, useLazyGetMoreMessagesByGroupChatIdQuery } from '../store/api/core/Chat.api';

const useGroupChatData = (chatId, appUserId, pageSizeRef) => {
    const [groupChatData, setGroupChatData] = useState({
        messages: null,
        count: 0,
        meInChat: null,
        groupChatUsers: [],
        isLoading: true,
    });

    const { data: messages, isLoading: messagesIsLoading } = useGetMessagesByGroupChatIdQuery({
        chatId,
        pageSize: pageSizeRef.current
    });
    const [getMoreMessagesByGroupChatIdAsync] = useLazyGetMoreMessagesByGroupChatIdQuery();
    const { data: count, isLoading: countIsLoading } = useGetGroupChatMessageCountByChatIdQuery(chatId);
    const { data: meInChat, isLoading: myUsersIsLoading } = useFindMeInChatQuery({ chatId, appUserId });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUserByChatIdQuery(chatId);

    useEffect(() => {
        if (!messagesIsLoading && !countIsLoading && !myUsersIsLoading && !usersIsLoading) {
            setGroupChatData(prevState => ({
                messages,
                count,
                meInChat,
                groupChatUsers,
                isLoading: false,
            }));
        }
    }, [messages, count, meInChat, groupChatUsers, messagesIsLoading, countIsLoading, myUsersIsLoading, usersIsLoading]);

    const getMoreMessagesAsync = async (offset) => {
        const arg = {
            chatId,
            offset,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreMessagesByGroupChatIdAsync(arg);
        if (response.data) {
            return response.data;
        }

        return [];
    }

    return { groupChatData, getMoreMessagesAsync };
}

export default useGroupChatData;