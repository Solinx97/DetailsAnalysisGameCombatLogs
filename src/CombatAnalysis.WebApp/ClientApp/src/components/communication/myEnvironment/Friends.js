import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useRemoveFriendAsyncMutation } from '../../../store/api/Friend.api';
import { useFriendSearchByUserIdQuery } from '../../../store/api/UserApi';
import UserInformation from './../UserInformation';
import RequestsToConnect from './RequestsToConnect';

import '../../../styles/communication/friends.scss';

const Friends = () => {
    const { t } = useTranslation("communication/myEnvironment/friends");

    const [, customer] = useAuthentificationAsync();
    const { data: myFriends, isLoading } = useFriendSearchByUserIdQuery(customer?.id);
    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation(customer?.id);

    const [userInformation, setUserInformation] = useState(null);

    const removeFriendAsync = async (friendId) => {
        await removeFriendAsyncMut(friendId);
    }

    const openUserInformation = (customer) => {
        setUserInformation(<UserInformation
            customer={customer}
            closeUserInformation={closeUserInformation}
        />);
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
                <div><strong>{t("Friends")}</strong></div>
            </div>
            <ul>
                {
                    myFriends?.map((item) => (
                        <li key={item.id} className="friend">
                            <div className="friend__details">
                                <FontAwesomeIcon
                                    icon={faWindowRestore}
                                    title={t("ShowDetails")}
                                    onClick={() => openUserInformation(item)}
                                />
                            </div>
                            <div>{item.username}</div>
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