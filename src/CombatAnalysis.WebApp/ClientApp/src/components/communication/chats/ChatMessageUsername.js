import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const ChatMessageUsername = ({ messageOwnerId }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(messageOwnerId);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="username" title={user?.usernam}>{user?.username}</div>
    );
}

export default ChatMessageUsername;