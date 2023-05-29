import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import "../../../styles/communication/friends.scss";

const Friends = () => {
    const user = useSelector((state) => state.user.value);

    const [friends, setFriends] = useState(<></>);

    useEffect(() => {
        let getFriends = async () => {
            await getFriendsAsync();
        }

        getFriends();
    }, [])

    const getFriendsAsync = async () => {
        const response = await fetch(`/api/v1/Friend/searchByForWhomId/${user.id}`);
        const status = response.status;

        if (status === 200) {
            const allFriends = await response.json();
            fillFriends(allFriends);
        }
    }

    const fillFriends = (allRequests) => {
        const list = allRequests.map((element) => createCard(element));

        setFriends(list);
    }

    const createCard = (element) => {
        return (<li key={element.id}>
            <div>{element.username}</div>
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