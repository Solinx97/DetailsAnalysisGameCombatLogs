import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery, useLazyGetCustomerByIdQuery } from '../../store/api/Customer.api';
import { useRemoveFriendAsyncMutation } from '../../store/api/Friend.api';
import UserInformation from './UserInformation';

const User = ({ userId, setUserInformation, allowRemoveFriend }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation();
    const [getCustomerByIdQ] = useLazyGetCustomerByIdQuery();

    const { data: customer, isLoading } = useGetCustomerByIdQuery(userId);

    const removeFriendAsync = async (friendId) => {
        await removeFriendAsyncMut(friendId);
    }

    const openUserInformationAsync = async (targetCustomerId) => {
        const targetCustomer = await getCustomerByIdQ(targetCustomerId);

        setUserInformation(
            <UserInformation
                customer={customer}
                people={targetCustomer.data}
                closeUserInformation={closeUserInformation}
            />
        );
    }

    const closeUserInformation = () => {
        setUserInformation(null);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="special-user">
            <FontAwesomeIcon
                icon={faWindowRestore}
                title={t("ShowDetails")}
                className="details"
                onClick={async () => await openUserInformationAsync(userId)}
            />
            <div className="username">{customer?.username}</div>
            {allowRemoveFriend &&
                <FontAwesomeIcon
                    icon={faCircleXmark}
                    title={t("Remove")}
                    className="remove"
                    onClick={async () => await removeFriendAsync(userId)}
                />
            }
        </div>
    );
}

export default User;