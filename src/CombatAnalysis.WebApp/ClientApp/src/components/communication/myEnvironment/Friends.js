import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useFriendSearchMyFriendsQuery, useRemoveFriendAsyncMutation } from '../../../store/api/Friend.api';
import UserInformation from './../UserInformation';
import FriendUsername from './FriendUsername';
import RequestsToConnect from './RequestsToConnect';

import '../../../styles/communication/myEnvironment/friends.scss';

const Friends = () => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const customer = useSelector((state) => state.customer.value);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(customer?.id);
    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation(customer?.id);
    const [getCustomerByIdQ] = useLazyGetCustomerByIdQuery();

    const [userInformation, setUserInformation] = useState(null);

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
        <div className="friends">
            <RequestsToConnect />
            <div>
                <div>{t("Friends")}</div>
            </div>
            <ul>
                {
                    myFriends?.map((item) => (
                        <li key={item.id} className="friend">
                            <div className="friend__details">
                                <FontAwesomeIcon
                                    icon={faWindowRestore}
                                    title={t("ShowDetails")}
                                    onClick={async () => await openUserInformationAsync(item.forWhomId === customer?.id ? item.whoFriendId : item.forWhomId)}
                                />
                            </div>
                            <FriendUsername
                                friendId={item.forWhomId === customer?.id ? item.whoFriendId : item.forWhomId}
                            />
                            <div className="friend__remove">
                                <FontAwesomeIcon
                                    icon={faCircleXmark}
                                    title={t("Remove")}
                                    onClick={async () => await removeFriendAsync(item.id)}
                                />
                            </div>
                        </li>
                    ))
                }
            </ul>
            {userInformation}
        </div>
    );
}

export default Friends;