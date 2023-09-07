import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCommunityDiscussionByCommunityIdQuery } from '../../../store/api/communication/community/CommunityDiscussion.api';
import CreateDiscussion from './CreateDiscussion';

import '../../../styles/communication/community/discussion.scss';

const CommunityDiscussions = ({ community, customer, setShowDiscussion, setDiscussion }) => {
    const { t } = useTranslation("communication/community/discussion");

    const [showCreateDiscussion, setShowCreateDiscussion] = useState(false);

    const { data: discussions, isLoading } = useGetCommunityDiscussionByCommunityIdQuery(community?.id);

    const handleDiscussion = (discussions) => {
        setDiscussion(discussions);
        setShowDiscussion((item) => !item);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="discussion">
            <div className="discussion__title">
                <div className="actions">
                    <div>{t("Discussion")}</div>
                    <div className="tool">
                        <FontAwesomeIcon
                            icon={faPlus}
                            title={t("CreateDiscussion")}
                            onClick={() => setShowCreateDiscussion((item) => !item) }
                        />
                    </div>
                </div>
            </div>
            <ul className="discussion__content">
                {discussions?.map((item) => (
                        <li key={item.id} title={item.title} onClick={() => handleDiscussion(item)}>{item.title}</li>
                    ))
                }
            </ul>
            {showCreateDiscussion &&
                <CreateDiscussion
                    community={community}
                    customer={customer}
                    setShowDiscussion={setShowCreateDiscussion}
                />
            }
        </span>
    );
}

export default CommunityDiscussions;