import { useEffect, useState } from 'react';
import { useFindGroupChatMessageByChatIdQuery } from '../store/api/ChatApi';
import { useFindGroupChatUserByChatIdQuery, useFindGroupChatUserQuery } from '../store/api/communication/chats/GroupChatUser.api';

const getGroupChatMessagesInterval = 1000;

const useGroupChatData = (chatId, userId) => {
    const [groupChatData, setGroupChatData] = useState({
        messages: [],
        meInChat: null,
        groupChatUsers: [],
        isLoading: true,
    });

    const { data: messages, isLoading: messagesLoading } = useFindGroupChatMessageByChatIdQuery(chatId, {
        pollingInterval: getGroupChatMessagesInterval,
    });
    const { data: meInChat, isLoading: myUsersIsLoading } = useFindGroupChatUserQuery({ chatId, userId });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUserByChatIdQuery(chatId);

    useEffect(() => {
        if (!messagesLoading && !myUsersIsLoading && !usersIsLoading) {
            setGroupChatData({
                messages,
                meInChat,
                groupChatUsers,
                isLoading: false,
            });
        }
    }, [messages, meInChat, groupChatUsers, messagesLoading, myUsersIsLoading, usersIsLoading]);

    return groupChatData;
}

export default useGroupChatData;