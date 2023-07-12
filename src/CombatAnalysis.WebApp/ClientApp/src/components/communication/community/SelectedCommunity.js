import { faArrowRightToBracket, faGear, faTrash, faPlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import Post from '../Post';

import '../../../styles/communication/selectedCommunity.scss';

const SelectedCommunity = ({ community, closeCommunity }) => {
    const customer = useSelector((state) => state.customer.value);

    const [myCommunityUserId, setMyCommunityUserId] = useState(0);
    const [feed, setFeed] = useState(<></>);
    const [members, setMembers] = useState(<></>);
    const [friends, setFriends] = useState(<></>);
    const [people, setPeople] = useState(<></>);
    const [communityUsersId, setCommunityUsersId] = useState([]);
    const [showAddNewPeople, setShowAddNewPeople] = useState(false);
    const [showRemovePeople, setShowRemovePeople] = useState(false);
    const [showRemovePeopleAlert, setShowRemovePeopleAlert] = useState(false);
    const [memberForRemove, setMemberForRemove] = useState(null);
    const [showLeaveFromCommunityAlert, setShowLeaveFromCommunityAlert] = useState(false);

    useEffect(() => {
        if (customer === null) {
            return;
        }

        let getPosts = async () => {
            await getCommunityUsersAsync();
            await getPostsAsync();
        }

        getPosts();
    }, [])

    const getCommunityUsersAsync = async (setShowRemovePeople = false) => {
        const members = [];
        const customersId = [];
        const response = await fetch(`/api/v1/CommunityUser/searchByCommunityId/${community.id}`);
        if (response.status !== 200) {
            return;
        }

        const communityUsers = await response.json();
        for (var i = 0; i < communityUsers.length; i++) {
            const customerById = await getCustomerByIdAsync(communityUsers[i].customerId);
            customerById.communityUserId = communityUsers[i].id;

            if (communityUsers[i].customerId === customer.id) {
                setMyCommunityUserId(communityUsers[i].id);
            }

            members.push(customerById);
            customersId.push(customerById.id);
        }

        setCommunityUsersId(customersId);
        fillMembers(members, setShowRemovePeople);
    }

    const getCustomerByIdAsync = async (userId) => {
        const response = await fetch(`/api/v1/Customer/${userId}`);
        if (response.status !== 200) {
            return null;
        }

        const user = await response.json();
        return user;
    }

    const getCommunityPostsAsync = async () => {
        const response = await fetch(`/api/v1/CommunityPost/searchByCommunityId/${community.id}`);
        if (response.status !== 200) {
            return [];
        }

        const communityPosts = await response.json();

        return communityPosts;
    }

    const getPostsAsync = async () => {
        const communityPosts = await getCommunityPostsAsync();
        setFeed(<Post selectedPosts={communityPosts} createPostAsync={createPostAsync} />);
    }

    const createPostAsync = async (postContent) => {
        const newPost = {
            id: 0,
            content: postContent,
            when: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            ownerId: customer.id
        }

        const response = await fetch("/api/v1/Post", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPost)
        });

        if (response.status === 200) {
            const createdPost = await response.json();
            const isCreated = await createCommunityPostAsync(createdPost.id);
            return isCreated;
        }

        return false;
    }

    const createCommunityPostAsync = async (postId) => {
        const newUserPost = {
            id: 0,
            communityId: community.id,
            postId: postId
        }

        const response = await fetch("/api/v1/CommunityPost", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newUserPost)
        });

        if (response.status === 200) {
            return true;
        }

        return false;
    }

    const getFriendsAsync = async () => {
        const response = await fetch(`/api/v1/Friend/searchByUserId/${customer.id}`);
        if (response.status !== 200) {
            return;
        }

        const myFriends = await response.json();

        fillFriends(myFriends);
    }

    const getPeopleAsync = async () => {
        const newPeople = [];
        const response = await fetch("/api/v1/Customer");

        if (response.status !== 200) {
            return;
        }

        const people = await response.json();
        for (let i = 0; i < people.length; i++) {
            if (communityUsersId.indexOf(people[i].id) === -1) {
                newPeople.push(people[i]);
            }
        }

        fillPeople(newPeople);
    }

    const removePeopleAsync = async () => {
        const response = await fetch(`/api/v1/CommunityUser/${memberForRemove.communityUserId}`, {
            method: 'DELETE'
        });

        if (response.status === 200) {
            await getCommunityUsersAsync(false);
            openRemovePeople(null);
        }
    }

    const leaveFromCommunityAsync = async () => {
        const response = await fetch(`/api/v1/CommunityUser/${myCommunityUserId}`, {
            method: 'DELETE'
        });

        if (response.status === 200) {
            closeCommunity();
        }
    }

    const ownerLeaveFromCommunityAsync = async () => {
        const response = await fetch(`/api/v1/Community/${community.id}`, {
            method: 'DELETE'
        });

        if (response.status === 200) {
            closeCommunity();
        }
    }

    const openRemovePeople = (member) => {
        setMemberForRemove(member);
        setShowRemovePeopleAlert(!showRemovePeopleAlert);
    }

    const openLeaveFromCommunity = () => {
        setShowLeaveFromCommunityAlert(!showLeaveFromCommunityAlert);
    }

    const fillMembers = (users, setShowRemovePeople) => {
        const list = users.map((element) => createMemberCard(element, setShowRemovePeople));

        setMembers(list);
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

    const createMemberCard = (member, showRemovePeople) => {
        return (<li key={member.id} className="member">
            {showRemovePeople && member.id !== community.ownerId && customer.id !== member.id
                ? <FontAwesomeIcon icon={faTrash} title="Remove" onClick={() => openRemovePeople(member)} />
                : null
            }
            <div className="member__username">{member.username}</div>
        </li>);
    }

    const createUserCard = (customer) => {
        return (<li key={customer.id} className="friend">
            <div>{customer.username}</div>
            <FontAwesomeIcon icon={faUserPlus} title="Send invite to community" onClick={async () => await createInviteToCommunityAsync(customer.id)} />
        </li>);
    }

    const openAddNewPeopleAsync = async () => {
        await getFriendsAsync();
        await getPeopleAsync();

        setShowAddNewPeople(!showAddNewPeople);
    }

    const openMembersSettingsAsync = async () => {
        setShowRemovePeople(!showRemovePeople);
        await getCommunityUsersAsync(!showRemovePeople);
    }

    const createInviteToCommunityAsync = async (userId) => {
        const newInviteToCommunity = {
            id: 0,
            communityId: community.id,
            toCustomerId: userId,
            when: new Date(),
            result: 0,
            ownerId: customer.id
        }

        const response = await fetch("/api/v1/InviteToCommunity", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newInviteToCommunity)
        });

        if (response.status === 200) {
            console.log("created");
        }
    }

    const render = () => {
        return (<div className="selected-community">
            <div className="selected-community__content">
                <div className="header">
                    <div className="title">
                        <FontAwesomeIcon icon={faArrowRightToBracket} title="Close" onClick={closeCommunity} />
                        <div title={community.name} onClick={closeCommunity}>{community.name}</div>
                    </div>
                    <div className="leave">
                        <button className="btn btn-outline-danger" onClick={openLeaveFromCommunity}>Leave</button>
                    </div>
                </div>
                <div>
                    <p>Description</p>
                    <div>{community.description}</div>
                </div>
                <div>
                    {feed}
                </div>
                {showAddNewPeople
                    ? <div className="add-new-people">
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
                        <button className="btn btn-outline-success" onClick={() => setShowAddNewPeople(!showAddNewPeople)}>Close</button>
                      </div>
                    : null
                }
                {showRemovePeopleAlert
                    ? <div className="remove-people">
                        <div>Remove people</div>
                        <div>
                            <div>You sure, that you want to remove <strong>'{memberForRemove.username}'</strong> from community <strong>'{community.name}'</strong>?</div>
                        </div>
                        <div>
                            <button className="btn btn-outline-warning" onClick={async () => await removePeopleAsync()}>Remove</button>
                            <button className="btn btn-outline-success" onClick={() => setShowRemovePeopleAlert(!showRemovePeopleAlert)}>Cancel</button>
                        </div>
                    </div>
                    : null
                }
                {showLeaveFromCommunityAlert
                    ? <div className="leave-from-community">
                        <div>Leave</div>
                        <div>
                            <div>You sure, that you want to leave from community <strong>'{community.name}'</strong>?</div>
                        </div>
                        {customer.id === community.ownerId
                            ? <>
                                <div class="alert alert-danger" role="alert">
                                    DANGER: You are owner of this community and if you leave this community will be removed. All users and content will be removed!!!
                                </div>
                                <div>
                                    <button className="btn btn-outline-danger" onClick={async () => await ownerLeaveFromCommunityAsync()}>Leave</button>
                                    <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunityAlert(!showLeaveFromCommunityAlert)}>Cancel</button>
                                </div>
                              </>
                            : <div>
                                <button className="btn btn-outline-danger" onClick={async () => await leaveFromCommunityAsync()}>Leave</button>
                                <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunityAlert(!showLeaveFromCommunityAlert)}>Cancel</button>
                              </div>
                        }
                    </div>
                    : null
                }
            </div>
            <ul className="selected-community__topics">
                <li>
                    <div>Discussions</div>
                    <ul></ul>
                </li>
                <li className="members">
                    <div className="title">
                        <div>Members</div>
                        <div className="tool">
                            <FontAwesomeIcon icon={faGear} title="Settings" onClick={async () => await openMembersSettingsAsync()} />
                            <FontAwesomeIcon icon={faPlus} title="Add a new people" onClick={async () => await openAddNewPeopleAsync()} />
                        </div>
                    </div>
                    <ul>
                        {members}
                    </ul>
                </li>
                <li>
                    <div>Friends</div>
                    <ul></ul>
                </li>
                <li>
                    <div>Contacts</div>
                    <ul></ul>
                </li>
            </ul>
        </div>)
    }

    return render();
}

export default SelectedCommunity;