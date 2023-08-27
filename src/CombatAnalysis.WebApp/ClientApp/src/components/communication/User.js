import { faCircleXmark, faUser } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery, useLazyGetCustomerByIdQuery } from '../../store/api/Customer.api';
import { useRemoveFriendAsyncMutation } from '../../store/api/Friend.api';
import UserInformation from './UserInformation';

const User = ({ me, itIsMe, targetCustomerId, setUserInformation, allowRemoveFriend }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation();
    const [getCustomerByIdQ] = useLazyGetCustomerByIdQuery();
    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(targetCustomerId);

    const removeFriendAsync = async () => {
        await removeFriendAsyncMut(targetCustomerId);
    }

    const openUserInformationAsync = async () => {
        const targetCustomer = await getCustomerByIdQ(targetCustomerId);

        setUserInformation(
            <UserInformation
                me={me}
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
        <div className={`special-user${itIsMe ? "__me" : "__another"}`}>
            <FontAwesomeIcon
                icon={faUser}
                title={t("ShowDetails")}
                className="details"
                onClick={async () => await openUserInformationAsync()}
            />
            <div className="username">{targetCustomer?.username}</div>
            {allowRemoveFriend &&
                <FontAwesomeIcon
                    icon={faCircleXmark}
                    title={t("Remove")}
                    className="remove"
                    onClick={async () => await removeFriendAsync()}
                />
            }
        </div>
    );
}

export default User;