import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const MyPersonalChatItem = ({ personalChat, selectedGroupChatId, setSelectedPersonalChat, companionId }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span className={selectedGroupChatId === personalChat.id ? "active" : ""}
            onClick={() => setSelectedPersonalChat(personalChat)}>
            <div><strong>{user.username}</strong></div>
            <div className="last-message" title={personalChat.lastMessage}>{personalChat.lastMessage}</div>
        </span>
    );
}

export default MyPersonalChatItem;