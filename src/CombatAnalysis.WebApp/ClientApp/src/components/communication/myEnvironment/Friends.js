import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFriendSearchMyFriendsQuery } from '../../../store/api/communication/myEnvironment/Friend.api';
import Loading from '../../Loading';
import User from '../User';

import '../../../styles/communication/myEnvironment/friends.scss';

const Friends = ({ customer, requestsToConnect, allowRemoveFriend }) => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(customer?.id);
    const [userInformation, setUserInformation] = useState(null);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="friends">
            {requestsToConnect}
            <div>
                <div className="friends__title">{t("Friends")}</div>
            </div>
            <ul>
                {myFriends.length > 0
                    ? myFriends?.map((friend) => (
                        <li key={friend.id} className="friend">
                            <User
                                targetCustomerId={friend.forWhomId === customer?.id ? friend.whoFriendId : friend.forWhomId}
                                setUserInformation={setUserInformation}
                                allowRemoveFriend={allowRemoveFriend}
                                friendId={friend.id}
                            />
                        </li>
                    ))
                    : <div className="friends__empty">{t("Empty")}</div>
                }
            </ul>
            {userInformation}
        </div>
    );
}

export default Friends;