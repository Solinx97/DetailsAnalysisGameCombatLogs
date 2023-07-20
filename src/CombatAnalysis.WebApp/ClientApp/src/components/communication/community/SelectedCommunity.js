import { faArrowRightToBracket, faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from "react";
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import { usePostSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useRemoveCommunityAsyncMutation } from '../../../store/api/Community.api';
import { useCreateCommunityPostAsyncMutation } from '../../../store/api/CommunityPost.api';
import { useLazySearchByUserIdAsyncQuery, useRemoveCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useCreatePostAsyncMutation } from '../../../store/api/Post.api';
import Post from '../Post';
import CommunityMembers from './CommunityMembers';

import '../../../styles/communication/selectedCommunity.scss';

const SelectedCommunity = ({ community, closeCommunity }) => {
    const [, customer] = useAuthentificationAsync();

    const [showLeaveFromCommunityAlert, setShowLeaveFromCommunityAlert] = useState(false);
    const [showDescription, setShowDescription] = useState(true);
    const [showCreatePost, setShowCreatePost] = useState(false);

    const postContentRef = useRef(null);

    const { data: communityPosts, isLoading } = usePostSearchByCommunityIdAsyncQuery(community.id);
    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewCommunityPostAsync] = useCreateCommunityPostAsyncMutation();
    const [removeCommunityAsync] = useRemoveCommunityAsyncMutation();
    const [searchByUserIdAsync] = useLazySearchByUserIdAsyncQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();

    const createPostAsync = async () => {
        const newPost = {
            content: postContentRef.current.value,
            when: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            ownerId: customer.id
        }

        const createdPost = await createNewPostAsync(newPost);
        if (createdPost.data !== undefined) {
            const isCreated = await createCommunityPostAsync(createdPost.data?.id);
            return isCreated;
        }

        return false;
    }

    const createCommunityPostAsync = async (postId) => {
        const newComunityPost = {
            communityId: community.id,
            postId: postId
        }

        const createdUserPost = await createNewCommunityPostAsync(newComunityPost);
        return createdUserPost.data === undefined ? false : true;
    }

    const leaveFromCommunityAsync = async () => {
        const myCommunityUserId = await searchByUserIdAsync(customer.id);
        const deletedItemCount = await removeCommunityUserAsync(myCommunityUserId);
        if (deletedItemCount.data !== undefined) {
            closeCommunity();
        }
    }

    const ownerLeaveFromCommunityAsync = async () => {
        const deletedItemCount = await removeCommunityAsync(community.id);
        if (deletedItemCount.data !== undefined) {
            closeCommunity();
        }
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
                <div>
                    <div className="create-post">
                        <div>
                            <div className="create-post__tool" style={{ display: !showCreatePost ? "flex" : "none" }}>
                                <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" />
                                <button type="button" className="btn btn-outline-info" onClick={() => setShowCreatePost((item) => !item)}>New post</button>
                            </div>
                            <div style={{ display: showCreatePost ? "flex" : "none" }} className="create-post__create-tool">
                                <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" />
                                <button type="button" className="btn btn-outline-warning" onClick={() => setShowCreatePost((item) => !item)}>Cancel</button>
                                <button type="button" className="btn btn-outline-success" onClick={async () => await createPostAsync()}>Create</button>
                            </div>
                        </div>
                        <textarea rows="5" cols="100" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
                    </div>
                    <ul>
                        {
                            communityPosts.map((item) => (
                                <li key={item?.id}>
                                    <Post
                                        key={item?.id}
                                        customer={customer}
                                        targetPostType={item}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                </div>
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
                    <CommunityMembers community={community} customer={customer} />
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

    if (isLoading) {
        return <>Loading...</>;
    }

    return render();
}

export default SelectedCommunity;