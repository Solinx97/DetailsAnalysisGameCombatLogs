import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useFindPersonalChatMessageCountQuery } from '../../../store/api/communication/chats/PersonalChatMessagCount.api';

const personalChatCountPollingInterval = 2000;

const PersonalChatListItem = ({ chat, setSelectedPersonalChat, companionId, meId }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: chat?.id, userId: meId }, {
        pollingInterval: personalChatCountPollingInterval,
    });

    if (isLoading || messagesCountLoading) {
        return (<div>Loading...</div>)
    }

    return (
        <span className="chat-card" onClick={() => setSelectedPersonalChat({ type: "personal", chat: chat })}>
            <div className="username">{user?.username}</div>
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

export default PersonalChatListItem;