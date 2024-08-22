import { useEffect, useState } from 'react';
import { useGetMessagesByGroupChatIdQuery } from '../store/api/ChatApi';
import { useGetGroupChatMessageCountByChatIdQuery } from '../store/api/communication/chats/GroupChatMessage.api';
import { useFindGroupChatUserByChatIdQuery, useFindGroupChatUserQuery } from '../store/api/communication/chats/GroupChatUser.api';

const getGroupChatMessagesInterval = 500;

const useGroupChatData = (chatId, userId) => {
    const pageSize = 10;

    const [groupChatData, setGroupChatData] = useState({
        messages: [],
        count: 0,
        meInChat: null,
        groupChatUsers: [],
        isLoading: true,
    });

    const { data: messages, isLoading: messagesIsLoading } = useGetMessagesByGroupChatIdQuery({
        chatId,
        pageSize
    }, {
        pollingInterval: getGroupChatMessagesInterval,
        refetchOnMountOrArgChange: true,
    });
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

    return groupChatData;
}

export default useGroupChatData;