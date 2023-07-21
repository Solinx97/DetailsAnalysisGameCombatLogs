import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useFriendSearchByUserIdQuery } from '../store/api/Friend.api';
import { useGetCustomersQuery } from '../store/api/UserApi';
import { useCreateInviteAsyncMutation } from '../store/api/InviteToCommunity.api';

import '../styles/addPeople.scss';

const AddPeople = ({ customer, community, communityUsersId, setShowAddPeople }) => {
    const { data: friends, isLoading: friendsIsLoading } = useFriendSearchByUserIdQuery(customer?.id);
    const { data: allPeople, isLoading: peopleIsLoading } = useGetCustomersQuery();
    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

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

    const createInviteForAnotherPeopleAsync = async (userId) => {
        const newInviteToCommunity = {
            communityId: community.id,
            toCustomerId: userId,
            when: new Date(),
            result: 0,
            ownerId: customer?.id
        }

        await createInviteAsync(newInviteToCommunity);
    }

    const createInviteForFriendAsync = async (friend) => {
        const newInviteToCommunity = {
            communityId: community.id,
            toCustomerId: friend.whoFriendId === customer.id ? friend.forWhomId : friend.whoFriendId,
            when: new Date(),
            result: 0,
            ownerId: customer?.id
        }

        await createInviteAsync(newInviteToCommunity);
    }

    const createInviteAsync = async (newInviteToCommunity) => {
        const res = await createInviteAsyncMut(newInviteToCommunity);
        if (res.data !== undefined) {
            setShowAddPeople(false);
        }
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
                <div>Search</div>
                <input type="text" />
            </div>
            <div>
                <div>
                    <div>Friends</div>
                    <ul>
                        {
                            friends?.map((item) => (
                                <li key={item.id} className="friend">
                                    <div>{item.username}</div>
                                    <FontAwesomeIcon icon={faUserPlus} title="Send invite to community" onClick={async () => await createInviteForFriendAsync(item)} />
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
                                    <FontAwesomeIcon icon={faUserPlus} title="Send invite to community" onClick={async () => await createInviteForAnotherPeopleAsync(item.id)} />
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