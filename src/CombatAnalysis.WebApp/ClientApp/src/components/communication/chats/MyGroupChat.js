import { useGetGroupChatByIdQuery } from '../../../store/api/ChatApi';
import { useFindGroupChatMessageCountQuery } from '../../../store/api/GroupChatMessagCount.api';

const MyGroupChat = ({ groupChatId, setSelectedGroupChat, meId }) => {
    const { data: groupChat, isLoading } = useGetGroupChatByIdQuery(groupChatId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindGroupChatMessageCountQuery({ chatId: groupChatId, userId: meId });

    if (isLoading || messagesCountLoading) {
        return <></>;
    }

    return (
        <span className="chat-card" onClick={() => setSelectedGroupChat(groupChat)}>
            <div className="username">{groupChat?.name}</div>
            {groupChat.lastMessage.length > 0 &&
                <div className="chat-tooltip">
                    {messagesCount?.count > 0 &&
                        <div className="unread-message-count">{messagesCount?.count > 99 ? "99+" : messagesCount?.count}</div>
                    }
                    <div className="last-message">{groupChat?.lastMessage}</div>
                </div>
            }
        </span>
    );
}

export default MyGroupChat;