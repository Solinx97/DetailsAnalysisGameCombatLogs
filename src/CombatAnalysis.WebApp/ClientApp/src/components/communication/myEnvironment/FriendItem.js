import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useLazyGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemoveFriendAsyncMutation } from '../../../store/api/Friend.api';
import UserInformation from './../UserInformation';
import FriendUsername from './FriendUsername';

const FriendItem = ({ friend, customer, setUserInformation, allowRemoveFriend }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation(customer?.id);
    const [getCustomerByIdQ] = useLazyGetCustomerByIdQuery();

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

    return (
        <>
            <div className="friend__details">
                <FontAwesomeIcon
                    icon={faWindowRestore}
                    title={t("ShowDetails")}
                    onClick={async () => await openUserInformationAsync(friend.forWhomId === customer?.id ? friend.whoFriendId : friend.forWhomId)}
                />
            </div>
            <FriendUsername
                friendId={friend.forWhomId === customer?.id ? friend.whoFriendId : friend.forWhomId}
            />
            {allowRemoveFriend &&
                <div className="friend__remove">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title={t("Remove")}
                        onClick={async () => await removeFriendAsync(friend.id)}
                    />
                </div>
            }
        </>
    );
}

export default FriendItem;