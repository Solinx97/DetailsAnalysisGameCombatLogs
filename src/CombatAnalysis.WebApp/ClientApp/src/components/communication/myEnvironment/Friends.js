import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { useFriendSearchByUserIdQuery, useRemoveFriendAsyncMutation } from '../../../store/api/Friend.api';
import UserInformation from './../UserInformation';
import RequestsToConnect from './RequestsToConnect';

import '../../../styles/communication/friends.scss';

const Friends = () => {
    const [, customer] = useAuthentificationAsync();
    const { data: myFriends, isLoading } = useFriendSearchByUserIdQuery(customer?.id);
    const [removeFriendAsyncMut] = useRemoveFriendAsyncMutation(customer?.id);

    const [friends, setFriends] = useState(<></>);
    const [userInformation, setUserInformation] = useState(<></>);

    useEffect(() => {
        !isLoading && fillFriends();
    }, [isLoading])

    const removeFriendAsync = async (friendId) => {
        await removeFriendAsyncMut(friendId);
    }

    const fillFriends = () => {
        const list = myFriends.length !== 0
            ? myFriends.map((element) => createCard(element))
            : (<div className="friends__empty">You don't have any friends</div>);

        setFriends(list);
    }

    const openUserInformation = (customer) => {
        setUserInformation(<UserInformation
            customer={customer}
            closeUserInformation={closeUserInformation}
        />);
    }

    const closeUserInformation = () => {
        setUserInformation(<></>);
    }

    const createCard = (friend) => {
        return (
            <li key={friend.id} className="friend">
                <div className="friend__details">
                    <FontAwesomeIcon
                        icon={faWindowRestore}
                        title="Show details"
                        onClick={() => openUserInformation(friend)}
                    />
                </div>
                <div>{friend.username}</div>
                <div className="friend__remove">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title="Remove"
                        onClick={async () => await removeFriendAsync(friend.id)}
                    />
                </div>
            </li>
        );
    }

    const render = () => {
        return (
            <div className="friends">
                <RequestsToConnect />
                <div>
                    <div><strong>Friends</strong></div>
                </div>
                <ul>{friends}</ul>
                {userInformation}
            </div>
        );
    }

    return render();
}

export default Friends;