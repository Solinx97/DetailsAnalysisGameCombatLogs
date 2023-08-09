import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const GroupChatUser = ({ userId }) => {
    const { data: customer, isLoading } = useGetCustomerByIdQuery(userId);

    if (isLoading) {
        return <></>;
    }

    return (
        <>{customer?.username}</>
    );
}

export default GroupChatUser;