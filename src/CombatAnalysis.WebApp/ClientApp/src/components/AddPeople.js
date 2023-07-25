import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useFriendSearchByUserIdQuery } from '../store/api/Friend.api';
import { useGetCustomersQuery } from '../store/api/UserApi';

import '../styles/addPeople.scss';

const AddPeople = ({ customer, createInviteAsync, communityUsersId, setShowAddPeople }) => {
    const { data: friends, isLoading: friendsIsLoading } = useFriendSearchByUserIdQuery(customer?.id);
    const { data: allPeople, isLoading: peopleIsLoading } = useGetCustomersQuery();

    const [friendsId, setFriendsId] = useState([]);
    const [people, setPeople] = useState([]);

    useEffect(() => {
        !friendsIsLoading && getFriends();
    }, [friendsIsLoading])

    useEffect(() => {
        !peopleIsLoading && getPeople();
    }, [peopleIsLoading])

    const getFriends = () => {
        const friendsId = [];
        for (let i = 0; i < friends?.length; i++) {
            friends[i].whoFriendId === customer.id
                ? friendsId.push(friends[i].forWhomId)
                : friendsId.push(friends[i].whoFriendId);
        }

        setFriendsId(friendsId);
    }

    const getPeople = () => {
        const anotherPeople = [];
        for (let i = 0; i < allPeople?.length; i++) {
            if (communityUsersId.indexOf(allPeople[i].id) === -1
                && friendsId.indexOf(allPeople[i].id) === -1
                && allPeople[i].id !== customer?.id) {
                anotherPeople.push(allPeople[i]);
            }
        }

        setPeople(anotherPeople);
    }

    const createInviteHandlerForFriendAsync = async (friend) => {
        const whomId = friend.whoFriendId === customer.id ? friend.forWhomId : friend.whoFriendId;
        await createInviteAsync(whomId);

        setShowAddPeople(false);
    }

    const createInviteHandlerAsync = async (whomId) => {
        await createInviteAsync(whomId);

        setShowAddPeople(false);
    }

    const handleAddNewPeopleVisibility = () => {
        setShowAddPeople(false);
    }

    if (friendsIsLoading || peopleIsLoading) {
        return <></>;
    }

    return (
        <div className="add-new-people">
            <div>Add new people</div>
            <div>
                <div>
                    <div>Friends</div>
                    <ul>
                        {
                            friends?.map((item) => (
                                <li key={item.id} className="friend">
                                    <div>{item.username}</div>
                                    <FontAwesomeIcon
                                        icon={faUserPlus}
                                        title="Send invite to community"
                                        onClick={async () => await createInviteHandlerForFriendAsync(item)}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                </div>
                <div>
                    <div>Another people</div>
                    <ul>
                        {
                            people?.map((item) => (
                                <li key={item.id} className="friend">
                                    <div>{item.username}</div>
                                    <FontAwesomeIcon
                                        icon={faUserPlus}
                                        title="Send invite to community"
                                        onClick={async () => await createInviteHandlerAsync(item.id)}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                </div>
            </div>
            <button className="btn btn-outline-success" onClick={handleAddNewPeopleVisibility}>Close</button>
        </div>
    );
}

export default AddPeople;