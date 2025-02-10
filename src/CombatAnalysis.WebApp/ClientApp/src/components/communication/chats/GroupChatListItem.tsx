import { useEffect, useState } from 'react';
import { useGetGroupChatByIdQuery } from '../../../store/api/chat/GroupChat.api';
import { useFindGroupChatMessageCountQuery } from '../../../store/api/chat/GroupChatMessagCount.api';
import { GroupChatListItemProps } from '../../../types/components/communication/chats/GroupChatListItemProps';

const GroupChatListItem: React.FC<GroupChatListItemProps> = ({ chatId, meInChatId, setSelectedGroupChat, subscribeToUnreadGroupMessagesUpdated }) => {
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: chat, isLoading } = useGetGroupChatByIdQuery(chatId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindGroupChatMessageCountQuery({ chatId: chatId, userId: meInChatId });

    useEffect(() => {
        subscribeToUnreadGroupMessagesUpdated(meInChatId, (targetChatId: number, targetMeInChatId: string, count: number) => {
            if (targetChatId === chatId && targetMeInChatId === meInChatId) {
                setUnreadMessageCount(count);
            }
        });
    }, []);

    useEffect(() => {
        if (!messagesCount) {
            return;
        }

        setUnreadMessageCount(messagesCount.count);
    }, [messagesCount]);

    if (isLoading || messagesCountLoading || !chat) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={() => setSelectedGroupChat({ type: "group", chat: chat })}>
            <div className="username">{chat?.name}</div>
            {unreadMessageCount > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{unreadMessageCount > 99 ? "99+" : unreadMessageCount}</div>
                </div>
            }
        </span>
    );
}

export default GroupChatListItem;