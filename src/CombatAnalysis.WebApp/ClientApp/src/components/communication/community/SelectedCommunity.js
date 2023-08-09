import { faArrowRightToBracket, faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCommunityByIdQuery, useRemoveCommunityAsyncMutation } from '../../../store/api/Community.api';
import { useCreateCommunityPostAsyncMutation } from '../../../store/api/CommunityPost.api';
import { useLazySearchByUserIdAsyncQuery, useRemoveCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useCreatePostAsyncMutation } from '../../../store/api/Post.api';
import CommunityMembers from './CommunityMembers';
import SelectedCommunityItem from './SelectedCommunityItem';

import '../../../styles/communication/selectedCommunity.scss';

const SelectedCommunity = () => {
    const { t } = useTranslation("communication/community/selectedCommunity");

    const customer = useSelector((state) => state.customer.value);

    const [showLeaveFromCommunityAlert, setShowLeaveFromCommunityAlert] = useState(false);
    const [showDescription, setShowDescription] = useState(true);
    const [showCreatePost, setShowCreatePost] = useState(false);
    const [communityId, setCommunityId] = useState(0);
    const [community, setCommunity] = useState(null);

    const postContentRef = useRef(null);

    const navigate = useNavigate();

    const [getCommunityByIdAsync] = useLazyGetCommunityByIdQuery();
    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewCommunityPostAsync] = useCreateCommunityPostAsyncMutation();
    const [removeCommunityAsync] = useRemoveCommunityAsyncMutation();
    const [searchByUserIdAsync] = useLazySearchByUserIdAsyncQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCommunityId(+queryParams.get("id"));
    }, [])

    useEffect(() => {
        let searchByCommunityId = async () => {
            const community = await getCommunityByIdAsync(communityId);
            setCommunity(community.data);
        }

        communityId > 0 && searchByCommunityId();
    }, [communityId])

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
            postContentRef.current.value = "";
            setShowCreatePost(false);

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
            navigate('/communication');
        }
    }

    const ownerLeaveFromCommunityAsync = async () => {
        const deletedItemCount = await removeCommunityAsync(community.id);
        if (deletedItemCount.data !== undefined) {
            navigate('/communication');
        }
    }

    const getCommunityPolicyType = () => {
        switch (community.policyType) {
            case 1:
                return <div className="title__policy" title={t("OpenExplain")}>{t("Open")}</div>
            case 2:
                return <div className="title__policy" title={t("PrivateExplain")}>{t("Private")}</div>
            case 3:
                return <div className="title__policy" title={t("PrivateWithLinkExplain")}>{t("PrivateWithLink")}</div>
            default:
                return <div className="title__policy" title={t("OpenExplain")}>{t("Open")}</div>
        }
    }

    if (community === null) {
        return <></>;
    }

    return (
        <div className="selected-community">
            <div className="selected-community__content">
                <div className="header">
                    <div className="title">
                        <FontAwesomeIcon
                            icon={faArrowRightToBracket}
                            title={t("Close")}
                            onClick={() => navigate('/communication')}
                        />
                        <div className="title__name" title={community.name} onClick={() => navigate('/communication')}>{community.name}</div>
                        {getCommunityPolicyType()}
                    </div>
                    <div className="leave">
                        <button className="btn btn-outline-danger" onClick={() => setShowLeaveFromCommunityAlert((item) => !item)}>{t("Leave")}</button>
                    </div>
                </div>
                <div className="description">
                    <div className="description__title">
                        <div>{t("Description")}</div>
                        {showDescription
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title={t("Hide")}
                                onClick={() => setShowDescription((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title={t("Show")}
                                onClick={() => setShowDescription((item) => !item)}
                            />
                        }
                    </div>
                    {showDescription && 
                        <div className="description__content">{community.description}</div>
                    }
                </div>
                <div>
                    <div className="create-post">
                        <div>
                            <div className="create-post__tool" style={{ display: !showCreatePost ? "flex" : "none" }}>
                                <FontAwesomeIcon
                                    icon={faArrowsRotate}
                                    title={t("Refresh")}
                                />
                                <button type="button" className="btn btn-outline-info" onClick={() => setShowCreatePost((item) => !item)}>{t("NewPost")}</button>
                            </div>
                            <div style={{ display: showCreatePost ? "flex" : "none" }} className="create-post__create-tool">
                                <FontAwesomeIcon
                                    icon={faArrowsRotate}
                                    title={t("Refresh")}
                                />
                                <button type="button" className="btn btn-outline-warning" onClick={() => setShowCreatePost((item) => !item)}>{t("Cancel")}</button>
                                <button type="button" className="btn btn-outline-success" onClick={async () => await createPostAsync()}>{t("Create")}</button>
                            </div>
                        </div>
                        <textarea rows="5" cols="100" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
                    </div>
                    <SelectedCommunityItem
                        customer={customer}
                        communityId={communityId}
                    />
                </div>
                {showLeaveFromCommunityAlert &&
                    <div className="leave-from-community">
                        <div>{t("LeaveAlert")}</div>
                        <div>
                            <div>{t("LeaveConfirm")} <strong>'{community.name}'</strong>?</div>
                        </div>
                        {customer.id === community.ownerId
                            ? <>
                                <div className="alert alert-danger" role="alert">
                                    {t("LeaveOwnerConfirm")}
                                </div>
                                <div>
                                    <button className="btn btn-outline-danger" onClick={async () => await ownerLeaveFromCommunityAsync()}>{t("Leave")}</button>
                                    <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunityAlert((item) => !item)}>{t("Cancel")}</button>
                                </div>
                            </>
                            : <div>
                                <button className="btn btn-outline-danger" onClick={async () => await leaveFromCommunityAsync()}>{t("Leave")}</button>
                                <button className="btn btn-outline-success" onClick={() => setShowLeaveFromCommunityAlert((item) => !item)}>{t("Cancel")}</button>
                            </div>
                        }
                    </div>
                }
            </div>
            <ul className="selected-community__topics">
                <li>
                    <div>{t("Discussions")}</div>
                    <ul></ul>
                </li>
                <li className="members">
                    <CommunityMembers
                        community={community}
                        customer={customer}
                    />
                </li>
                <li>
                    <div>{t("Friends")}</div>
                    <ul></ul>
                </li>
                <li>
                    <div>{t("Contacts")}</div>
                    <ul></ul>
                </li>
            </ul>
        </div>
    )
}

export default SelectedCommunity;