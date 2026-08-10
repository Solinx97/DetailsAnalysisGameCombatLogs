import type { RootState } from '@/app/Store';
import logger from '@/utils/Logger';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import type { AppUserModel } from '../../../../user/types/AppUserModel';
import { useGetCommunityDiscussionByCommunityIdQuery, useLazyGetCommunityDiscussionByCommunityIdQuery } from '../../../api/CommunityDiscussion.api';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';
import type { CommunityModel } from '../../../types/CommunityModel';
import CreateDiscussion from './CreateDiscussion';
import DiscussionList from './DiscussionList';

import './Discussion.scss';

const defaultMaxDiscussions = 5;

interface CommunityDiscussionsProps {
    community: CommunityModel;
    myself: AppUserModel;
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
    setDiscussion: (value: SetStateAction<CommunityDiscussionModel | null>) => void;
    isCommunityMember: boolean;
}

const CommunityDiscussions: React.FC<CommunityDiscussionsProps> = ({ community, myself, setShowDiscussion, setDiscussion, isCommunityMember }) => {
    const { t } = useTranslation("communication/community/discussion");

    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const [showCreateDiscussion, setShowCreateDiscussion] = useState(false);
    const [showAllDiscussions, setShowAllDiscussions] = useState(false);
    const [allDiscussions, setAllDiscussions] = useState<CommunityDiscussionModel[]>([]);

    const { discussions, isLoading } = useGetCommunityDiscussionByCommunityIdQuery({ communityId: community?.id, page: 1, pageSize: 5 }, {
        selectFromResult: ({ data, isLoading }) => ({
            discussions: data?.slice(0, defaultMaxDiscussions),
            isLoading
        }),
    });
    const [getAllDiscussionsAsync] = useLazyGetCommunityDiscussionByCommunityIdQuery();

    const handleDiscussion = (discussion: CommunityDiscussionModel) => {
        setDiscussion(discussion);
        setShowDiscussion((item) => !item);
    }

    const handleShowAllDiscussionsAsync = async () => {
        try {
            const discussions = await getAllDiscussionsAsync({ communityId: community?.id, page: 1, pageSize: 5 }).unwrap();
            setAllDiscussions(discussions);
            setShowAllDiscussions((item) => !item);
        } catch (e) {
            logger.error("Failed to load all discussions", e);
        }
    }

    if (isLoading || !discussions) {
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
                {discussions.map((item) => (
                        <li key={item.id} title={item.title} onClick={() => handleDiscussion(item)}>{item.title}</li>
                    ))
                }
            </ul>
            {discussions.length >= defaultMaxDiscussions &&
                <input type="button" value={t("AllDiscussions")} className="btn btn-outline-success all-discussion" onClick={handleShowAllDiscussionsAsync} />
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
                    discussions={allDiscussions}
                    setShowDiscussion={setShowAllDiscussions}
                    handleDiscussion={handleDiscussion}
                />
            }
        </span>
    );
}

export default CommunityDiscussions;