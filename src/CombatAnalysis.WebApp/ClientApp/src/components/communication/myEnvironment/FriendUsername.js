import { useGetUserByIdQuery } from '../../../store/api/Account.api';

const FriendUsername = ({ friendId }) => {
    const { data: customer, isLoading } = useGetUserByIdQuery(friendId);

    if (isLoading) {
        return <></>;
    }

    return (<div>{customer?.username}</div>);
}

export default FriendUsername;