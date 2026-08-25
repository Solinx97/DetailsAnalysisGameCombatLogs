import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import logger from '@/utils/Logger';
import { faBars, faChevronLeft, faChevronRight, faEarthEurope, faEye, faEyeSlash, faShieldHalved, faDeleteLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLazyGetCommunityByIdQuery } from '../../api/Community.api';
import type { CommunityDiscussionModel } from '../../types/CommunityDiscussionModel';
import type { CommunityModel } from '../../types/CommunityModel';
import CreateCommunityPost from '../create/CreateCommunityPost';
import CommunityMembers from './CommunityMembers';
import CommunityDiscussions from './discussion/CommunityDiscussions';
import Discussion from './discussion/Discussion';
import CommunityMenu from './menu/CommunityMenu';
import SelectedCommunityItem from './SelectedCommunityItem';

import './SelectedCommunity.scss';

const SelectedCommunity = () => {
    const { t } = useTranslation('communication/community/selectedCommunity');

    const myself = useSelector((state: RootState) => state.user.value);
    const navigate = useNavigate();
    
    const [lastCheck, setLastCheck] = useState((new Date()).toISOString());
    const [feedVersion, setFeedVersion] = useState(1);

    const [isCommunityMember, setIsCommunityMember] = useState(false);
    const [showDescription, setShowDescription] = useState(true);
    const [showMenu, setShowMenu] = useState(false);
    const [communityId, setCommunityId] = useState(0);
    const [community, setCommunity] = useState<CommunityModel | null>(null);
    const [showDiscussion, setShowDiscussion] = useState(false);
    const [discussion, setDiscussion] = useState<CommunityDiscussionModel | null>(null);
    const [showActions, setShowActions] = useState(true);

    const [getCommunityByIdAsync] = useLazyGetCommunityByIdQuery();

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
                            <div className="btn-shadow select-another-player"
                                onClick={() => navigate(-1)}>
                                <FontAwesomeIcon
                                    icon={faDeleteLeft}
                                />
                                <div>{t("Back")}</div>
                            </div>
                            <div className="title__content">
                                <div className="name" title={community.name}>
                                    <FontAwesomeIcon
                                        icon={community.policyType === 0 ? faEarthEurope : faShieldHalved}
                                        title={community.policyType ? t("Open") : t("Private")}
                                    />
                                    <div>{community.name}</div>
                                </div>
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
                                <div>{t("Description")}</div>
                            </div>
                            <FontAwesomeIcon
                                icon={showDescription ? faEye : faEyeSlash}
                                title={showDescription ? t("Hide") : t("Show")}
                                onClick={() => setShowDescription((item) => !item)}
                            />
                        </div>
                    </div>
                    {showDescription &&
                        <div className="description__content">{community.description}</div>
                    }
                    {(discussion && showDiscussion) &&
                        <Discussion
                            discussionId={discussion.id}
                            communityId={communityId}
                            setShowDiscussion={setShowDiscussion}
                            user={myself}
                        />
                    }
                    <div className="posts">
                        {isCommunityMember &&
                            <CreateCommunityPost
                                user={myself}
                                communityId={community.id}
                                feedVersion={feedVersion}
                                t={t}
                            />
                        }
                        <SelectedCommunityItem
                            user={myself}
                            communityId={communityId}
                            lastCheck={lastCheck}
                            setLastCheck={setLastCheck}
                            feedVersion={feedVersion}
                            setFeedVersion={setFeedVersion}
                            t={t}
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
                                    isCommunityMember={isCommunityMember}
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