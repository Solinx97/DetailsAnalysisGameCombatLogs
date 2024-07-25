import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const GroupChatUser = ({ userId }) => {
    const { data: customer, isLoading } = useGetCustomerByIdQuery(userId);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="group-chat-user">
            {customer?.username}
        </div>
    );
}

export default GroupChatUser;