import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const MyPersonalChat = ({ personalChat, setSelectedPersonalChat, companionId }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span onClick={() => setSelectedPersonalChat(personalChat)}>
            <div><strong>{user?.username}</strong></div>
            <div className="last-message" title={personalChat.lastMessage}>{personalChat.lastMessage}</div>
        </span>
    );
}

export default MyPersonalChat;