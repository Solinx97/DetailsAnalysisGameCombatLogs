import { useEffect, useState } from 'react';
import { useGetMessagesByGroupChatIdQuery, useLazyGetMoreMessagesByGroupChatIdQuery } from '../store/api/core/Chat.api';
import { useGetGroupChatMessageCountByChatIdQuery } from '../store/api/chat/GroupChatMessage.api';
import { useFindGroupChatUserByChatIdQuery, useFindGroupChatUserQuery } from '../store/api/chat/GroupChatUser.api';

const getGroupChatMessagesInterval = 500;

const useGroupChatData = (chatId, userId, pageSizeRef) => {
    const [groupChatData, setGroupChatData] = useState({
        messages: [],
        count: 0,
        meInChat: null,
        groupChatUsers: [],
        isLoading: true,
    });

    const { data: messages, isLoading: messagesIsLoading } = useGetMessagesByGroupChatIdQuery({
        chatId,
        pageSize: pageSizeRef.current
    }, {
        pollingInterval: getGroupChatMessagesInterval,
        refetchOnMountOrArgChange: true,
    });
    const [getMoreMessagesByGroupChatIdAsync] = useLazyGetMoreMessagesByGroupChatIdQuery();
    const { data: count, isLoading: countIsLoading } = useGetGroupChatMessageCountByChatIdQuery(chatId);
    const { data: meInChat, isLoading: myUsersIsLoading } = useFindGroupChatUserQuery({ chatId, userId });
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