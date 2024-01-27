import { faBars, faCloudArrowUp, faEarthEurope, faEye, faEyeSlash, faPen, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyGetCommunityByIdQuery, useUpdateCommunityAsyncMutation } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityPostAsyncMutation } from '../../../store/api/communication/community/CommunityPost.api';
import CommunicationMenu from '../CommunicationMenu';
import CommunityDiscussions from './CommunityDiscussions';
import CommunityMembers from './CommunityMembers';
import CommunityMenu from './CommunityMenu';
import Discussion from './Discussion';
import SelectedCommunityItem from './SelectedCommunityItem';

import '../../../styles/communication/community/selectedCommunity.scss';
import CreatePost from '../CreatePost';

const SelectedCommunity = () => {
    const { t } = useTranslation("communication/community/selectedCommunity");

    const customer = useSelector((state) => state.customer.value);

    const [isCommunityMember, setIsCommunityMember] = useState(false);
    const [showDescription, setShowDescription] = useState(true);
    const [showMenu, setShowMenu] = useState(false);
    const [communityId, setCommunityId] = useState(0);
    const [community, setCommunity] = useState(null);
    const [editNameOn, setEditNameOn] = useState(false);
    const [editDescriptionOn, setEditDescriptionOn] = useState(false);
    const [showDiscussion, setShowDiscussion] = useState(false);
    const [discussion, setDiscussion] = useState(null);

    const communityNameInput = useRef(null);
    const communityDescriptionInput = useRef(null);

    const [getCommunityByIdAsync] = useLazyGetCommunityByIdQuery();
    const [updateCommunityAsync] = useUpdateCommunityAsyncMutation();
    const [createNewCommunityPostAsync] = useCreateCommunityPostAsyncMutation();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCommunityId(+queryParams.get("id"));
    }, [])

    useEffect(() => {
        if (communityId === 0) {
            return;
        }

        let searchByCommunityId = async () => {
            const community = await getCommunityByIdAsync(communityId);
            setCommunity(community.data);
        }

        searchByCommunityId();
    }, [communityId])

    const createCommunityPostAsync = async (postId) => {
        const newComunityPost = {
            communityId: community.id,
            postId: postId
        }

        const createdUserPost = await createNewCommunityPostAsync(newComunityPost);
        return createdUserPost.data === undefined ? false : true;
    }

    const updateCommunityNameAsync = async () => {
        setEditNameOn(false);

        const communityForUpdate = Object.assign({}, community);
        communityForUpdate.name = communityNameInput.current.value;

        const updated = await updateCommunityAsync(communityForUpdate);
        if (updated.data !== undefined) {
            setCommunity(communityForUpdate);
        }
    }

    const updateCommunityDescriptionAsync = async () => {
        setEditDescriptionOn(false);

        const communityForUpdate = Object.assign({}, community);
        communityForUpdate.description = communityDescriptionInput.current.value;

        const updated = await updateCommunityAsync(communityForUpdate);
        if (updated.data !== undefined) {
            setCommunity(communityForUpdate);
        }
    }

    if (community === null) {
        return (
            <CommunicationMenu
                currentMenuItem={3}
                selectedCommunityName={community?.name}
            />
        );
    }

    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={3}
                selectedCommunityName={community?.name}
            />
            <div className="communication__content selected-community">
                <div className="selected-community__content">
                    <div className="header">
                        <div className="title">
                            <div className="title__content">
                                {community?.customerId === customer?.id &&
                                    <FontAwesomeIcon
                                        icon={faPen}
                                        title={t("EditName")}
                                        className={`edit${editNameOn ? "_active" : ""}`}
                                        onClick={() => setEditNameOn((item) => !item)}
                                    />
                                }
                                {editNameOn
                                    ? <>
                                        <input className="form-control" type="text" defaultValue={community?.name} ref={communityNameInput} />
                                        <FontAwesomeIcon
                                            icon={faCloudArrowUp}
                                            title={t("Save")}
                                            onClick={async () => await updateCommunityNameAsync()}
                                        />
                                    </>
                                    : <div className="name" title={community?.name}>
                                        {community?.policyType === 0
                                            ? <FontAwesomeIcon
                                                icon={faEarthEurope}
                                                title={t("Open")}
                                            />
                                            : <FontAwesomeIcon
                                                icon={faShieldHalved}
                                                title={t("Private")}
                                            />
                                        }
                                        <div>{community?.name}</div>
                                    </div>
                                }
                            </div>
                            {isCommunityMember &&
                                <FontAwesomeIcon
                                    icon={faBars}
                                    title={t("Menu")}
                                    onClick={() => setShowMenu((item) => !item)}
                                />
                            }
                        </div>
                    </div>
                    <div className="description">
                        <div className="title">
                            <div className="title__content">
                                {community?.customerId === customer?.id &&
                                    <FontAwesomeIcon
                                        icon={faPen}
                                        title={t("EditDescription")}
                                        className={`edit${editDescriptionOn ? "_active" : ""}`}
                                        onClick={() => setEditDescriptionOn((item) => !item)}
                                    />
                                }
                                <div>{t("Description")}</div>
                                {editDescriptionOn &&
                                    <FontAwesomeIcon
                                        icon={faCloudArrowUp}
                                        title={t("Save")}
                                        onClick={async () => await updateCommunityDescriptionAsync()}
                                    />
                                }
                            </div>
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
                        {showDescription
                            ? editDescriptionOn
                                ? <textarea className="form-control" rows="4" cols="50" ref={communityDescriptionInput} defaultValue={community?.description} />
                                : <div className="description__content">{community?.description}</div>
                            : null
                        }
                    </div>
                    {showDiscussion &&
                        <Discussion
                            discussionId={discussion?.id}
                            setShowDiscussion={setShowDiscussion}
                            customer={customer}
                        />
                    }
                    <div>
                        {isCommunityMember &&
                            <CreatePost
                                customer={customer}
                                owner={community.name}
                                postTypeName="community"
                                createTypeOfPostFunc={createCommunityPostAsync}
                            />
                        }
                        <SelectedCommunityItem
                            customer={customer}
                            communityId={communityId}
                        />
                    </div>
                </div>
                <ul className="selected-community__actions">
                    <li>
                        <CommunityMembers
                            community={community}
                            customer={customer}
                            setIsCommunityMember={setIsCommunityMember}
                        />
                    </li>
                    <li>
                        <CommunityDiscussions
                            community={community}
                            customer={customer}
                            setShowDiscussion={setShowDiscussion}
                            setDiscussion={setDiscussion}
                            isCommunityMember={isCommunityMember}
                        />
                    </li>
                </ul>
            </div>
            {showMenu &&
                <CommunityMenu
                    setShowMenu={setShowMenu}
                    customer={customer}
                    community={community}
                    setCommunity={setCommunity}
                />
            }
        </div>
    )
}

export default memo(SelectedCommunity);