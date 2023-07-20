import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import CustomerService from '../services/CustomerService';
import FriendService from '../services/FriendService';

import '../styles/addPeople.scss';

const AddPeople = ({ customer, communityUsersId, createInviteAsync, setShowAddPeople }) => {
    const friendService = new FriendService();
    const customerService = new CustomerService();

    const [friends, setFriends] = useState(<></>);
    const [friendsId, setFriendsId] = useState([]);
    const [people, setPeople] = useState(<></>);

    useEffect(() => {
        let getFriends = async () => {
            await getFriendsAsync();
        }

        getFriends();
    }, [])

    useEffect(() => {
        if (friendsId.length === 0) {
            return;
        }

        let getPeople = async () => {
            await getPeopleAsync();
        }

        getPeople();
    }, [friendsId])

    const getFriendsAsync = async () => {
        const friendsId = [];

        const friends = await friendService.searchByUserId(customer.id);
        if (friends === null) {
            return;
        }

        for (let i = 0; i < friends.length; i++) {
            friends[i].whoFriendId === customer.id
                ? friendsId.push(friends[i].forWhomId)
                : friendsId.push(friends[i].whoFriendId);
        }

        setFriendsId(friendsId);
        fillFriends(friends);
    }

    const getPeopleAsync = async () => {
        const newPeople = [];

        const people = await customerService.getAllAsync();
        if (people === null) {
            return;
        }

        for (let i = 0; i < people.length; i++) {
            if (communityUsersId.indexOf(people[i].id) === -1
                && friendsId.indexOf(people[i].id) === -1) {
                newPeople.push(people[i]);
            }
        }

        fillPeople(newPeople);
    }

    const fillFriends = (friends) => {
        const list = friends.length !== 0
            ? friends.map((element) => createUserCard(element))
            : (<div className="friends__empty">You don't have any friends</div>);

        setFriends(list);
    }

    const fillPeople = (people) => {
        const list = people.map((element) => createUserCard(element));

        setPeople(list);
    }

    const createUserCard = (user) => {
        return (
            <li key={user.id} className="friend">
                <div>{user.username}</div>
                <FontAwesomeIcon icon={faUserPlus} title="Send invite to community" onClick={async () => await createInviteAsync(user.id)} />
            </li>
        );
    }

    const handleAddNewPeopleVisibility = () => {
        setShowAddPeople(false);
    }

    const render = () => {
        return (
            <div className="add-new-people">
                <div>Add new people</div>
                <div>
                    <div>Search</div>
                    <input type="text" />
                </div>
                <div>
                    <div>
                        <div>Friends</div>
                        <ul>
                            {friends}
                        </ul>
                    </div>
                    <div>
                        <div>Another people</div>
                        <ul>
                            {people}
                        </ul>
                    </div>
                </div>
                <button className="btn btn-outline-success" onClick={handleAddNewPeopleVisibility}>Close</button>
            </div>
        );
    }

    return render();
}

export default AddPeople;