import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const FriendUsername = ({ friendId }) => {
    const { data: customer, isLoading } = useGetCustomerByIdQuery(friendId);

    if (isLoading) {
        return <></>;
    }

    return (<div>{customer?.username}</div>);
}

export default FriendUsername;