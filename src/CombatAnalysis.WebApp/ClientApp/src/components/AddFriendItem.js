import { faPlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../store/api/Customer.api';

const AddFriendItem = ({ friendUserId, addUserIdToList, removeUserIdToList, filterContent, peopleIdToJoin }) => {
    const { t } = useTranslation("addFriendItem");

    const { data: user, isLoading } = useGetCustomerByIdQuery(friendUserId);

    if (isLoading) {
        return <></>;
    }

    return (
        user?.username.toLowerCase().startsWith(filterContent.toLowerCase()) &&
        <>
            <div>{user?.username}</div>
            {peopleIdToJoin.includes(user.id)
                ? <FontAwesomeIcon
                    icon={faUserPlus}
                    title={t("CancelRequest")}
                    onClick={() => removeUserIdToList(user?.id)}
                />
                : <FontAwesomeIcon
                    icon={faPlus}
                    title={t("SendInvite")}
                    onClick={() => addUserIdToList(user?.id)}
                />
            }

        </>
    );
}

export default AddFriendItem;