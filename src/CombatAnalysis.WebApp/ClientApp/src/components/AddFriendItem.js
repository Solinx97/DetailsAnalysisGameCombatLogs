import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../store/api/Customer.api';

const AddFriendItem = ({ friendUserId, addUserIdToList, filterContent }) => {
    const { t } = useTranslation("addFriendItem");

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
                title={t("SendInvite")}
                onClick={() => addUserIdToList(user?.id)}
            />
        </>
    );
}

export default AddFriendItem;