import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCustomerByIdQuery } from '../store/api/Customer.api';

const AddFriendItem = ({ friendUserId, createInviteAsync, filterContent }) => {
    const { data: user, isLoading } = useGetCustomerByIdQuery(friendUserId);

    if (isLoading) {
        return <></>;
    }

    return (
        user?.username.toLowerCase().startsWith(filterContent.toLowerCase()) &&
        <>
            <div>{user?.username}</div>
            <FontAwesomeIcon
                icon={faUserPlus}
                title="Send invite"
                onClick={async () => await createInviteAsync(user?.username, friendUserId)}
            />
        </>
    );
}

export default AddFriendItem;