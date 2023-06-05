import { faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import "../../../styles/communication/friends.scss";

const Friends = () => {
    const customer = useSelector((state) => state.customer.value);

    const [friends, setFriends] = useState(<></>);

    useEffect(() => {
        let getFriends = async () => {
            await getFriendsAsync();
        }

        getFriends();
    }, [])

    const getFriendsAsync = async () => {
        let response = await fetch(`/api/v1/Friend/searchByForWhomId/${customer.id}`);
        if (response.status !== 200) {
            console.log("problem");

            return;
        }

        const myFriendsByForWhom = await response.json();

        response = await fetch(`/api/v1/Friend/searchByWhoFriendId/${customer.id}`);
        if (response.status !== 200) {
            console.log("problem");

            return;
        }

        const myFriendsByWhoFriend = await response.json();

        const myFriends = myFriendsByForWhom.concat(myFriendsByWhoFriend);
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
        const list = myFriends.map((element) => createCard(element));

        setFriends(list);
    }

    const createCard = (element) => {
        return (<li key={element.id} className="friend">
            <div>{element.username}</div>
            <div className="friend__remove"><FontAwesomeIcon icon={faCircleXmark} title="Remove"
                onClick={async () => await removeFriendAsync(element.id)} /></div>
        </li>);
    }

    const render = () => {
        return (<div className="friends">
            <ul>{friends}</ul>
        </div>);
    }

    return render();
}

export default Friends;