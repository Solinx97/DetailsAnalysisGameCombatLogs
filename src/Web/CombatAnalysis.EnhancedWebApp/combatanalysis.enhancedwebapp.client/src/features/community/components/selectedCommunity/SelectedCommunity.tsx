import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import logger from '@/utils/Logger';
import { faBars, faChevronLeft, faChevronRight, faCloudArrowUp, faEarthEurope, faEye, faEyeSlash, faPen, faShieldHalved } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyGetCommunityByIdQuery, useUpdateCommunityAsyncMutation } from '../../api/Community.api';
import type { CommunityDiscussionModel } from '../../types/CommunityDiscussionModel';
import type { CommunityModel } from '../../types/CommunityModel';
import CreateCommunityPost from '../create/CreateCommunityPost';
import CommunityMembers from './CommunityMembers';
import CommunityMenu from './CommunityMenu';
import CommunityDiscussions from './discussion/CommunityDiscussions';
import Discussion from './discussion/Discussion';
import SelectedCommunityItem from './SelectedCommunityItem';

import './SelectedCommunity.scss';

const SelectedCommunity = () => {
    const { t } = useTranslation('communication/community/selectedCommunity');

    const myself = useSelector((state: RootState) => state.user.value);

    const [isCommunityMember, setIsCommunityMember] = useState(false);
    const [showDescription, setShowDescription] = useState(true);
    const [showMenu, setShowMenu] = useState(false);
    const [communityId, setCommunityId] = useState(0);
    const [community, setCommunity] = useState<CommunityModel | null>(null);
    const [editNameOn, setEditNameOn] = useState(false);
    const [editDescriptionOn, setEditDescriptionOn] = useState(false);
    const [showDiscussion, setShowDiscussion] = useState(false);
    const [discussion, setDiscussion] = useState<CommunityDiscussionModel | null>(null);
    const [showActions, setShowActions] = useState(true);

    const communityNameInput = useRef<HTMLInputElement | null>(null);
    const communityDescriptionInput = useRef<HTMLTextAreaElement | null>(null);

    const [getCommunityByIdAsync] = useLazyGetCommunityByIdQuery();
    const [updateCommunityAsync] = useUpdateCommunityAsyncMutation();

    useEffect(() => {
        const queryParams = new URLSearchParams(window.location.search);
        setCommunityId(parseInt(queryParams.get("id") ?? "0"));
    }, []);

    useEffect(() => {
        if (communityId === 0) {
            return;
        }

        const searchByCommunityId = async () => {
            try {
                const community = await getCommunityByIdAsync(communityId).unwrap();
                setCommunity(community);
            } catch (e) {
                logger.error("Could not find community by provided community id", e);
            }
        }

        searchByCommunityId();
    }, [communityId]);

    const updateCommunityNameAsync = async () => {
        if (!communityNameInput.current) {
            return;
        }

        setEditNameOn(false);

        const communityForUpdate = Object.assign({}, community);
        communityForUpdate.name = communityNameInput.current.value;

        const updated = await updateCommunityAsync(communityForUpdate);
        if (updated.data) {
            setCommunity(communityForUpdate);
        }
    }

    const updateCommunityDescriptionAsync = async () => {
        if (!communityDescriptionInput.current) {
            return;
        }

        setEditDescriptionOn(false);

        const communityForUpdate = Object.assign({}, community);
        communityForUpdate.description = communityDescriptionInput.current.value;

        const updated = await updateCommunityAsync(communityForUpdate);
        if (updated.data) {
            setCommunity(communityForUpdate);
        }
    }

    if (!community || !myself) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={3}
                />
                <Loading />
            </>
        );
    }

    return (
        <div className="communication">
            <CommunicationMenu
                currentMenuItem={3}
            />
            <div className="communication-content selected-community">
                <div className="selected-community__content">
                    <div className="header">
                        <div className="title">
                            <div className="title__content">
                                {community?.appUserId === myself?.id &&
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
                                            onClick={updateCommunityNameAsync}
                                        />
                                    </>
                                    : <div className="name" title={community?.name}>
                                        <FontAwesomeIcon
                                            icon={community?.policyType ? faEarthEurope : faShieldHalved}
                                            title={community?.policyType ? t("Open") : t("Private")}
                                        />
                                        <div>{community?.name}</div>
                                    </div>
                                }
                            </div>
                            {(isCommunityMember && community.appUserId === myself?.id) &&
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
                                {community?.appUserId === myself?.id &&
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
                                        onClick={updateCommunityDescriptionAsync}
                                    />
                                }
                            </div>
                            <FontAwesomeIcon
                                icon={showDescription ? faEye : faEyeSlash}
                                title={showDescription ? t("Hide") : t("Show")}
                                onClick={() => setShowDescription((item) => !item)}
                            />
                        </div>
                        {showDescription
                            ? editDescriptionOn
                                ? <textarea className="form-control" rows={2} cols={50} ref={communityDescriptionInput} defaultValue={community?.description} />
                                : <div className="description__content">{community?.description}</div>
                            : null
                        }
                    </div>
                    {(discussion && showDiscussion) &&
                        <Discussion
                            discussionId={discussion.id}
                            setShowDiscussion={setShowDiscussion}
                            user={myself}
                        />
                    }
                    <div className="posts">
                        {isCommunityMember &&
                            <CreateCommunityPost
                                user={myself}
                                communityName={community.name}
                                communityId={community.id}
                                t={t}
                            />
                        }
                        <SelectedCommunityItem
                            myselfId={myself.id}
                            communityId={communityId}
                        />
                    </div>
                </div>
                <div className="selected-community__actions-container">
                    <FontAwesomeIcon
                        icon={showActions ? faChevronRight : faChevronLeft}
                        title={showActions ? t("HideActions") : t("ShowActions")}
                        onClick={() => setShowActions((item) => !item)}
                    />
                    {showActions &&
                        <div className="selected-community__actions">
                            <div>
                                <CommunityMembers
                                    community={community}
                                    myself={myself}
                                    setIsCommunityMember={setIsCommunityMember}
                                />
                            </div>
                            <div>
                                <CommunityDiscussions
                                    community={community}
                                    myself={myself}
                                    setShowDiscussion={setShowDiscussion}
                                    setDiscussion={setDiscussion}
                                    isCommunityMember={isCommunityMember}
                                />
                            </div>
                        </div>
                    }
                </div>
            </div>
            {showMenu &&
                <CommunityMenu
                    setShowMenu={setShowMenu}
                    user={myself}
                    community={community}
                    setCommunity={setCommunity}
                />
            }
        </div>
    )
}

export default memo(SelectedCommunity);