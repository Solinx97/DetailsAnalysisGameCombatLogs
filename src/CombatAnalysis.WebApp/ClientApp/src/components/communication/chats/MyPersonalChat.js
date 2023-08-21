import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const MyPersonalChat = ({ personalChat, setSelectedPersonalChat, companionId }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(companionId);

    if (isLoading) {
        return <></>;
    }

    return (
        <span onClick={() => setSelectedPersonalChat(personalChat)}>
            <div className="username">{user?.username}</div>
            {personalChat.lastMessage.length > 0 &&
                < div className="last-message">{personalChat.lastMessage}</div>
            }
        </span>
    );
}

export default MyPersonalChat;