import { APP_CONFIG } from '@/config/appConfig';
import type { RootState } from '@/app/Store';
import { faPlus, faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import type { AppUserModel } from '../../../../user/types/AppUserModel';
import { useGetShortListCommunityDiscussionByCommunityIdQuery } from '../../../api/CommunityDiscussion.api';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';
import type { CommunityModel } from '../../../types/CommunityModel';
import CreateDiscussion from './CreateDiscussion';
import DiscussionList from './DiscussionList';

import './Discussion.scss';

interface CommunityDiscussionsProps {
    community: CommunityModel;
    myself: AppUserModel;
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
    setDiscussion: (value: SetStateAction<CommunityDiscussionModel | null>) => void;
    isCommunityMember: boolean;
}

const CommunityDiscussions: React.FC<CommunityDiscussionsProps> = ({ community, myself, setShowDiscussion, setDiscussion, isCommunityMember }) => {
    const { t } = useTranslation('communication/community/discussion');

    const defaultMaxDiscussions = 5;

    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityDiscussionSize ?? defaultMaxDiscussions);

    const [showCreateDiscussion, setShowCreateDiscussion] = useState(false);
    const [showAllDiscussions, setShowAllDiscussions] = useState(false);

    const { data: discussions, isLoading } = useGetShortListCommunityDiscussionByCommunityIdQuery({ communityId: community.id, pageSize: pageSizeRef.current });

    const handleDiscussion = (discussion: CommunityDiscussionModel) => {
        setDiscussion(discussion);
        setShowDiscussion((item) => !item);
    }

    if (!discussions || isLoading) {
        return (<></>);
    }

    return (
        <span className="discussion">
            <div className="discussion__title">
                <div className="actions">
                    <div>{t("Discussion")}</div>
                    {(isCommunityMember && userPrivacy?.emailVerified) &&
                        <div className="tool">
                            <FontAwesomeIcon
                                icon={faPlus}
                                title={t("CreateDiscussion")}
                                onClick={() => setShowCreateDiscussion((item) => !item)}
                            />
                        </div>
                    }
                </div>
            </div>
            <ul className="discussion__content">
                {discussions.discussions.map((item) => (
                    <li key={item.id} title={item.title} onClick={() => handleDiscussion(item)}>{item.title}</li>
                ))
                }
            </ul>
            {discussions.count > pageSizeRef.current &&
                <div className="btn-shadow" onClick={() => setShowAllDiscussions((item) => !item)}>
                    <FontAwesomeIcon
                        icon={faBars}
                    />
                    <div>{t("AllDiscussions")}</div>
                </div>
            }
            {showCreateDiscussion &&
                <CreateDiscussion
                    community={community}
                    myself={myself}
                    setShowDiscussion={setShowCreateDiscussion}
                />
            }
            {showAllDiscussions &&
                <DiscussionList
                    setShowDiscussion={setShowAllDiscussions}
                    handleDiscussion={handleDiscussion}
                    communityId={community.id}
                    isCommunityMember={isCommunityMember}
                    emailVerified={userPrivacy ? userPrivacy.emailVerified : false}
                    setShowCreateDiscussion={setShowCreateDiscussion}
                />
            }
        </span>
    );
}

export default CommunityDiscussions;