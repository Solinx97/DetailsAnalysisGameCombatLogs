import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCommunityDiscussionByCommunityIdQuery, useLazyGetCommunityDiscussionByCommunityIdQuery } from '../../../store/api/communication/community/CommunityDiscussion.api';
import CreateDiscussion from './CreateDiscussion';
import DiscussionList from './DiscussionList';
import { useSelector } from 'react-redux';

import '../../../styles/communication/community/discussion.scss';

const defaultMaxDiscussions = 5;

const CommunityDiscussions = ({ community, customer, setShowDiscussion, setDiscussion, isCommunityMember }) => {
    const { t } = useTranslation("communication/community/discussion");

    const userPrivacy = useSelector((state) => state.userPrivacy.value);

    const [showCreateDiscussion, setShowCreateDiscussion] = useState(false);
    const [showAllDiscussions, setShowAllDiscussions] = useState(false);
    const [allDiscussions, setAllDiscussions] = useState([]);

    const { discussions, isLoading } = useGetCommunityDiscussionByCommunityIdQuery(community?.id, {
        selectFromResult: ({ data }) => ({
            discussions: data?.slice(0, defaultMaxDiscussions)
        }),
    });
    const [getAllDiscussionsAsync] = useLazyGetCommunityDiscussionByCommunityIdQuery();

    const handleDiscussion = (discussions) => {
        setDiscussion(discussions);
        setShowDiscussion((item) => !item);
    }

    const handleShowAllDiscussionsAsync = async () => {
        const discussions = await getAllDiscussionsAsync(community?.id);
        if (discussions.data !== undefined) {
            setAllDiscussions(discussions.data);
            setShowAllDiscussions((item) => !item);
        }
    }

    if (isLoading) {
        return <></>;
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
                {discussions?.map((item) => (
                        <li key={item.id} title={item.title} onClick={() => handleDiscussion(item)}>{item.title}</li>
                    ))
                }
            </ul>
            {discussions?.length >= defaultMaxDiscussions &&
                <input type="button" value={t("AllDiscussions")} className="btn btn-outline-success all-discussion" onClick={handleShowAllDiscussionsAsync} />
            }
            {showCreateDiscussion &&
                <CreateDiscussion
                    community={community}
                    customer={customer}
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