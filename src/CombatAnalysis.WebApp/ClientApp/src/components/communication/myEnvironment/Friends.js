import { faCircleXmark, faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import UserInformation from './../UserInformation';

import "../../../styles/communication/friends.scss";

const Friends = () => {
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
        const response = await fetch(`/api/v1/Friend/searchByUserId/${customer.id}`);
        if (response.status !== 200) {
            console.log("problem");

            return;
        }

        const myFriends = await response.json();

        fillFriends(myFriends);
    }

    const removeFriendAsync = async (friendId) => {
        let response = await fetch(`/api/v1/Friend/${friendId}`, {
            method: 'DELETE',
        });

        if (response.status !== 200) {
            console.log("problem");

            return;
        }

        await getFriendsAsync();
    }

    const fillFriends = (myFriends) => {
        const list = myFriends.length !== 0
            ? myFriends.map((element) => createCard(element))
            : (<div className="friends__empty">You don't have any friends</div>);

        setFriends(list);
    }

    const openUserInformation = (customer) => {
        setUserInformation(<UserInformation customer={customer} closeUserInformation={closeUserInformation} />);
    }

    const closeUserInformation = () => {
        setUserInformation(<></>);
    }

    const createCard = (element) => {
        return (<li key={element.id} className="friend">
            <div className="friend__details">
                <FontAwesomeIcon icon={faWindowRestore} title="Show details" onClick={() => openUserInformation(element)} />
            </div>
            <div>{element.username}</div>
            <div className="friend__remove">
                <FontAwesomeIcon icon={faCircleXmark} title="Remove" onClick={async () => await removeFriendAsync(element.id)} />
            </div>
        </li>);
    }

    const render = () => {
        return (<div className="friends">
            <div>
                <div>Friends</div>
            </div>
            <ul>{friends}</ul>
            {userInformation}
        </div>);
    }

    return render();
}

export default Friends;