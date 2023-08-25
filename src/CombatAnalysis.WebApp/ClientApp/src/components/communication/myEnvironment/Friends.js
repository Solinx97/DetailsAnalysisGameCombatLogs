import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFriendSearchMyFriendsQuery } from '../../../store/api/Friend.api';
import FriendItem from './FriendItem';

import '../../../styles/communication/myEnvironment/friends.scss';

const Friends = ({ customer, requestsToConnect, allowRemoveFriend }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(customer?.id);
    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="friends">
            {requestsToConnect}
            <div>
                <div>{t("Friends")}</div>
            </div>
            <ul>
                {
                    myFriends?.map((item) => (
                        <li key={item.id} className="friend">
                            <FriendItem
                                customer={customer}
                                friend={item}
                                setUserInformation={setUserInformation}
                                allowRemoveFriend={allowRemoveFriend}
                            />
                        </li>
                    ))
                }
            </ul>
            {userInformation}
        </div>
    );
}

export default Friends;