import { faArrowRightToBracket, faEye, faEyeSlash, faGear, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import CommunityPostService from '../../../services/CommunityPostService';
import CommunityUserService from '../../../services/CommunityUserService';
import CustomerService from '../../../services/CustomerService';
import InviteToCommunityService from '../../../services/InviteToCommunityService';
import PostService from '../../../services/PostService';
import AddPeople from '../../AddPeople';
import Alert from '../../Alert';
import Post from '../Post';

import CommunityService from '../../../services/CommunityService';
import '../../../styles/communication/selectedCommunity.scss';

const SelectedCommunity = ({ community, closeCommunity }) => {
    const communityUserService = new CommunityUserService();
    const communityPostService = new CommunityPostService();
    const customerService = new CustomerService();
    const postService = new PostService();
    const inviteToCommunityService = new InviteToCommunityService();

    const customer = useSelector((state) => state.customer.value);

    const [myCommunityUserId, setMyCommunityUserId] = useState(0);
    const [members, setMembers] = useState([]);
    const [communityUsersId, setCommunityUsersId] = useState([]);
    const [feed, setFeed] = useState(<></>);
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [showRemovePeople, setShowRemovePeople] = useState(false);
    const [showRemovePeopleAlert, setShowRemovePeopleAlert] = useState(false);
    const [showLeaveFromCommunityAlert, setShowLeaveFromCommunityAlert] = useState(false);
    const [showDescription, setShowDescription] = useState(true);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(true);
    const [showAddedUserAlert, setShowAddedUserAlert] = useState(false);
    const [memberForRemove, setMemberForRemove] = useState(null);

    const checkAuthentificationAsync = useAuthentificationAsync();

    useEffect(() => {
        if (customer === null) {
            return;
        }

        let checkAuthentification = async () => {
            await checkAuthentificationAsync();
        }

        let getPosts = async () => {
            await getCommunityPostsAsync();
            await getCommunityUsersAsync();
        }

        checkAuthentification();
        getPosts();
    }, [])

    const getCommunityUsersAsync = async () => {
        const members = [];
        const customersId = [];
        const communityUsers = await communityUserService.searchByCommunityIdAsync(community.id);
        for (let i = 0; i < communityUsers.length; i++) {
            const customerById = await getCustomerByIdAsync(communityUsers[i].customerId);
            customerById.communityUserId = communityUsers[i].id;

            if (communityUsers[i].customerId === customer.id) {
                setMyCommunityUserId(communityUsers[i].id);
            }

            members.push(customerById);
            customersId.push(customerById.id);
        }

        setCommunityUsersId(customersId);
        setMembers(members);
    }

    const getCustomerByIdAsync = async (id) => {
        const user = await customerService.getByIdAsync(id);
        return user;
    }

    const getCommunityPostsAsync = async () => {
        const communityPosts = await communityPostService.searchByCommunityIdAsync(community.id);
        setFeed(<Post
            selectedPostsType={communityPosts}
            createPostAsync={createPostAsync}
            updatePostsAsync={getCommunityPostsAsync}
            setShowUpdatingAlert={setShowUpdatingAlert}
        />);
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

        const createdPost = await postService.createAsync(newPost);
        const isCreated = await createCommunityPostAsync(createdPost.id);

        await getCommunityPostsAsync();

        return isCreated;
    }

    const createCommunityPostAsync = async (postId) => {
        const newUserPost = {
            id: 0,
            communityId: community.id,
            postId: postId
        }

        const createdCommunityPost = await communityPostService.createAsync(newUserPost);
        if (createdCommunityPost !== null) {
            return true;
        }

        return false;
    }

    const removePeopleAsync = async () => {
        const deletedItemCount = await communityUserService.deleteAsync(memberForRemove.communityUserId);
        if (deletedItemCount != null) {
            await getCommunityUsersAsync(false);
            openRemovePeople(null);
        }
    }

    const leaveFromCommunityAsync = async () => {
        const deletedItemCount = await communityUserService.deleteAsync(myCommunityUserId);
        if (deletedItemCount != null) {
            closeCommunity();
        }
    }

    const ownerLeaveFromCommunityAsync = async () => {
        const deletedItemCount = await CommunityService.deleteAsync(community.id);
        if (deletedItemCount != null) {
            closeCommunity();
        }
    }

    const createInviteAsync = async (userId) => {
        const newInviteToCommunity = {
            id: 0,
            communityId: community.id,
            toCustomerId: userId,
            when: new Date(),
            result: 0,
            ownerId: customer.id
        }

        const createdItem = await inviteToCommunityService.createAsync(newInviteToCommunity);
        if (createdItem !== null) {
            setShowAddedUserAlert(true);
        }
    }

    const openRemovePeople = (member) => {
        setMemberForRemove(member);
        setShowRemovePeopleAlert(!showRemovePeopleAlert);
    }

    const fillMembers = (setShowRemovePeople) => {
        const list = members.map((element) => createMemberCard(element, setShowRemovePeople));

        return list;
    }

    const createMemberCard = (member) => {
        return (<li key={member.id} className="member">
            {showRemovePeople && member.id !== community.ownerId && customer.id !== member.id
                ? <FontAwesomeIcon icon={faTrash} title="Remove" onClick={() => openRemovePeople(member)} />
                : null
            }
            <div className="member__username">{member.username}</div>
        </li>);
    }

    const getCommunityPolicyType = () => {
        switch (community.policyType) {
            case 1:
                return <div className="title__policy" title="Any users can search and connect to this community free">Open</div>
            case 2:
                return <div className="title__policy" title="This is community is hide and allow only invite another users from members">Close</div>
            case 3:
                return <div className="title__policy" title="This is community is hide and allow invite another users from members or from special link">Close (use link)</div>
            default:
                return <div className="title__policy" title="Any users can search and connect to this community free">Open</div>
        }
    }

    const render = () => {
        return (<div className="selected-community">
            <div className="selected-community__content">
                <Alert
                    isVisible={showUpdatingAlert}
                    content="Updating..."
                />
                <Alert
                    isVisible={showAddedUserAlert}
                    typeOfAlert={2}
                    content="Created invite for user"
                    timeout={2000}
                />
                <div className="header">
                    <div className="title">
                        <FontAwesomeIcon
                            icon={faArrowRightToBracket}
                            title="Close"
                            onClick={closeCommunity}
                        />
                        <div className="title__name" title={community.name} onClick={closeCommunity}>{community.name}</div>
                        {getCommunityPolicyType()}
                    </div>
                    <div className="leave">
                        <button className="btn btn-outline-danger" onClick={() => setShowLeaveFromCommunityAlert((item) => !item)}>Leave</button>
                    </div>
                </div>
                <div className="description">
                    <div className="description__title">
                        <div>Description</div>
                        {showDescription
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title="Hide"
                                onClick={() => setShowDescription((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title="Show"
                                onClick={() => setShowDescription((item) => !item)}
                            />
                        }
                    </div>
                    {showDescription
                        ? <div className="description__content">{community.description}</div>
                        : null
                    }
                </div>
                {feed}
                {showAddPeople
                    ? <AddPeople
                        communityUsersId={communityUsersId}
                        createInviteAsync={createInviteAsync}
                        setShowAddPeople={setShowAddPeople}
                    />
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
                            <button className="btn btn-outline-success" onClick={() => setShowRemovePeopleAlert((item) => !item)}>Cancel</button>
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
                                <div className="alert alert-danger" role="alert">
                                    DANGER: You are owner of this community and if you leave this community will be removed. All users and content will be removed!!!
                                </div>
                                <div>
                                    <button className="btn btn-outline-danger" onClick={async () => await ownerLeaveFromCommunityAsync()}>Leave</button>
                                    <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunityAlert((item) => !item)}>Cancel</button>
                                </div>
                              </>
                            : <div>
                                <button className="btn btn-outline-danger" onClick={async () => await leaveFromCommunityAsync()}>Leave</button>
                                <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunityAlert((item) => !item)}>Cancel</button>
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
                            <FontAwesomeIcon
                                icon={faGear}
                                title="Settings"
                                onClick={() => setShowRemovePeople((item) => !item)}
                            />
                            <FontAwesomeIcon
                                icon={faPlus}
                                title="Add a new people"
                                onClick={() => setShowAddPeople((item) => !item)}
                            />
                        </div>
                    </div>
                    <ul>
                        {fillMembers()}
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