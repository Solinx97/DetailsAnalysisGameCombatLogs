import { useGetGroupChatByIdQuery } from '../../../store/api/communication/chats/GroupChat.api';
import { useFindGroupChatMessageCountQuery } from '../../../store/api/communication/chats/GroupChatMessagCount.api';

const GroupChatListItem = ({ chatId, groupChatUserId, setSelectedGroupChat }) => {
    const { data: chat, isLoading } = useGetGroupChatByIdQuery(chatId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindGroupChatMessageCountQuery({ chatId: chatId, userId: groupChatUserId });

    if (isLoading || messagesCountLoading) {
        return (<></>);
    }

    return (
        <span className="chat-card" onClick={() => setSelectedGroupChat({ type: "group", chat: chat })}>
            <div className="username">{chat?.name}</div>
            {chat.lastMessage.length > 0 &&
                <div className="chat-tooltip">
                    {messagesCount?.count > 0 &&
                        <div className="unread-message-count">{messagesCount?.count > 99 ? "99+" : messagesCount?.count}</div>
                    }
                    <div className="last-message">{chat?.lastMessage}</div>
                </div>
            }
        </span>
    );
}

export default GroupChatListItem;