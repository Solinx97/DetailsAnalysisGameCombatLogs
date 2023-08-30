import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFriendSearchMyFriendsQuery } from '../../../store/api/communication/myEnvironment/Friend.api';
import User from '../User';

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
                            <User
                                targetCustomerId={item.forWhomId === customer?.id ? item.whoFriendId : item.forWhomId}
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