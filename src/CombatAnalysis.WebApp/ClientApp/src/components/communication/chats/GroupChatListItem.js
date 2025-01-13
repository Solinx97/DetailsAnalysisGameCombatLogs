import { useGetGroupChatByIdQuery } from '../../../store/api/chat/GroupChat.api';
import { useFindGroupChatMessageCountQuery } from '../../../store/api/chat/GroupChatMessagCount.api';

const groupChatCountPollingInterval = 2000;
const groupChatPollingInterval = 2000;

const GroupChatListItem = ({ chatId, groupChatUserId, setSelectedGroupChat }) => {
    const { data: chat, isLoading } = useGetGroupChatByIdQuery(chatId, {
        pollingInterval: groupChatPollingInterval,
    });
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindGroupChatMessageCountQuery({ chatId: chatId, userId: groupChatUserId }, {
        pollingInterval: groupChatCountPollingInterval,
    });

    if (isLoading || messagesCountLoading || !chat) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={() => setSelectedGroupChat({ type: "group", chat: chat })}>
            <div className="username">{chat?.name}</div>
            {messagesCount?.count > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{messagesCount?.count > 99 ? "99+" : messagesCount?.count}</div>
                </div>
            }
        </span>
    );
}

export default GroupChatListItem;