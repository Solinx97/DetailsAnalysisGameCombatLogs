import { faArrowsRotate, faBars, faEarthEurope, faEye, faEyeSlash, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityPostAsyncMutation } from '../../../store/api/communication/community/CommunityPost.api';
import { useCreatePostAsyncMutation } from '../../../store/api/communication/Post.api';
import Communication from '../Communication';
import CommunityMembers from './CommunityMembers';
import CommunityMenu from './CommunityMenu';
import SelectedCommunityItem from './SelectedCommunityItem';

import '../../../styles/communication/community/selectedCommunity.scss';

const SelectedCommunity = () => {
    const { t } = useTranslation("communication/community/selectedCommunity");

    const customer = useSelector((state) => state.customer.value);

    const [showDescription, setShowDescription] = useState(true);
    const [showCreatePost, setShowCreatePost] = useState(false);
    const [showMenu, setShowMenu] = useState(false);
    const [communityId, setCommunityId] = useState(0);
    const [community, setCommunity] = useState(null);
    const [showAddPeople, setShowAddPeople] = useState(false);

    const postContentRef = useRef(null);

    const [getCommunityByIdAsync] = useLazyGetCommunityByIdQuery();
    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewCommunityPostAsync] = useCreateCommunityPostAsyncMutation();

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

    const handleShowAddPeople = () => {
        setShowAddPeople((item) => !item);
    }

    if (community === null) {
        return <></>;
    }

    return (
        <div className="communication">
            <Communication
                currentMenuItem={3}
                selectedCommunityName={community.name}
            />
            <div className="communication__content selected-community">
                <div className="selected-community__content">
                    <div className="header">
                        <div className="title">
                            {community.policyType === 1
                                ? <FontAwesomeIcon
                                    icon={faEarthEurope}
                                    title={t("Open")}
                                />
                                : <FontAwesomeIcon
                                    icon={faShieldHalved}
                                    title={t("Private")}
                                />
                            }
                            <div className="title__name" title={community.name}>{community.name}</div>
                            <FontAwesomeIcon
                                icon={faBars}
                                title={t("Menu")}
                                onClick={() => setShowMenu((item) => !item)}
                            />
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
                </div>
                <ul className="selected-community__actions">
                    <li>
                        <div>{t("Discussions")}</div>
                        <ul></ul>
                    </li>
                    <li>
                        <CommunityMembers
                            community={community}
                            customer={customer}
                            handleShowAddPeople={handleShowAddPeople}
                            showAddPeople={showAddPeople}
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
            {showMenu &&
                <CommunityMenu
                    setShowMenu={setShowMenu}
                    customer={customer}
                    community={community}
                />
            }
        </div>
    )
}

export default SelectedCommunity;