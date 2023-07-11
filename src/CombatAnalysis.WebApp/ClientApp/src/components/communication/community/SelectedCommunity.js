import { faArrowRightToBracket, faPlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import Post from '../Post';

import '../../../styles/communication/selectedCommunity.scss';

const SelectedCommunity = ({ community, closeCommunity }) => {
    const customer = useSelector((state) => state.customer.value);

    const [feed, setFeed] = useState(<></>);
    const [members, setMembers] = useState(<></>);
    const [friends, setFriends] = useState(<></>);
    const [people, setPeople] = useState(<></>);
    const [communityUsersId, setCommunityUsersId] = useState([]);
    const [showAddNewPeople, setShowAddNewPeople] = useState(false);

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

    const getCommunityUsersAsync = async () => {
        const members = [];
        const customersId = [];
        const response = await fetch(`/api/v1/CommunityUser/searchByCommunityId/${community.id}`);
        if (response.status !== 200) {
            return;
        }

        const communityUsers = await response.json();
        for (var i = 0; i < communityUsers.length; i++) {
            const customer = await getCustomerByIdAsync(communityUsers[i].customerId);
            members.push(customer);
            customersId.push(customer.id);
        }

        setCommunityUsersId(customersId);
        fillMembers(members);
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

    const fillMembers = (users) => {
        const list = users.map((element) => createMemberCard(element));

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

    const createMemberCard = (customer) => {
        return (<li key={customer.id} className="member">
            {customer.username}
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
                <div className="title">
                    <FontAwesomeIcon icon={faArrowRightToBracket} title="Close" onClick={closeCommunity} />
                    <div title={community.name} onClick={closeCommunity}>{community.name}</div>
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
            </div>
            <ul className="selected-community__topics">
                <li>
                    <div>Discussions</div>
                    <ul></ul>
                </li>
                <li className="members">
                    <div className="title">
                        <div>Members</div>
                        <div>
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