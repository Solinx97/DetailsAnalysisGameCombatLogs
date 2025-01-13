import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { useFindPersonalChatMessageCountQuery } from '../../../store/api/chat/PersonalChatMessagCount.api';

const personalChatCountPollingInterval = 2000;

const PersonalChatListItem = ({ chat, setSelectedPersonalChat, companionId, meId }) => {
    const { data: user, isLoading } = useGetUserByIdQuery(companionId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: chat?.id, userId: meId }, {
        pollingInterval: personalChatCountPollingInterval,
    });

    if (isLoading || messagesCountLoading) {
        return (<div className="chat-loading-yet">Loading...</div>)
    }

    return (
        <span className="chat-card" onClick={() => setSelectedPersonalChat({ type: "personal", chat: chat })}>
            <div className="username">{user?.username}</div>
            {messagesCount?.count > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{messagesCount?.count > 99 ? "99+" : messagesCount?.count}</div>
                </div>
            }
        </span>
    );
}

export default PersonalChatListItem;