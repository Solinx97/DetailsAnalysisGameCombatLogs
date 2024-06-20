import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useFindPersonalChatMessageCountQuery } from '../../../store/api/communication/chats/PersonalChatMessagCount.api';

const MyPersonalChat = ({ personalChat, setSelectedPersonalChat, companionId, meId }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: personalChat?.id, userId: meId });

    if (isLoading || messagesCountLoading) {
        return <div>Loading...</div>;
    }

    return (
        <span className="chat-card" onClick={() => setSelectedPersonalChat(personalChat)}>
            <div className="username">{user?.username}</div>
            {personalChat.lastMessage.length > 0 &&
                <div className="chat-tooltip">
                    {messagesCount?.count > 0 &&
                        <div className="unread-message-count">{messagesCount?.count > 99 ? "99+" : messagesCount?.count}</div>
                    }
                    <div className="last-message">{personalChat?.lastMessage}</div>
                </div>
            }
        </span>
    );
}

export default MyPersonalChat;