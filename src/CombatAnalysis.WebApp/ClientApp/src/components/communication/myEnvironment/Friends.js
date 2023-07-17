import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import FriendService from '../../../services/FriendService';
import UserInformation from './../UserInformation';
import RequestsToConnect from './RequestsToConnect';

import '../../../styles/communication/friends.scss';

const Friends = () => {
    const friendService = new FriendService();

    const customer = useSelector((state) => state.customer.value);

    const [friends, setFriends] = useState(<></>);
    const [userInformation, setUserInformation] = useState(<></>);

    useEffect(() => {
        let getFriends = async () => {
            await getFriendsAsync();
        }

        getFriends();
    }, [])

    const getFriendsAsync = async () => {
        const myFriends = await friendService.searchByUserId(customer.id);
        if (myFriends !== null) {
            fillFriends(myFriends);
        }
    }

    const removeFriendAsync = async (friendId) => {
        const deletedItem = await friendService.deleteAsync(friendId);
        if (deletedItem !== null) {
            await getFriendsAsync();
        }
    }

    const fillFriends = (friends) => {
        const list = friends.length !== 0
            ? friends.map((element) => createCard(element))
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

    const createCard = (friedn) => {
        return (
            <li key={friedn.id} className="friend">
                <div className="friend__details">
                    <FontAwesomeIcon
                        icon={faWindowRestore}
                        title="Show details"
                        onClick={() => openUserInformation(friedn)}
                    />
                </div>
                <div>{friedn.username}</div>
                <div className="friend__remove">
                    <FontAwesomeIcon
                        icon={faCircleXmark}
                        title="Remove"
                        onClick={async () => await removeFriendAsync(friedn.id)}
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
                    <div>Friends</div>
                </div>
                <ul>{friends}</ul>
                {userInformation}
            </div>
        );
    }

    return render();
}

export default Friends;