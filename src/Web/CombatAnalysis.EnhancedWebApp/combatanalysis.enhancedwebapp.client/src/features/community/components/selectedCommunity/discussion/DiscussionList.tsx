import { APP_CONFIG } from '@/config/appConfig';
import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { faClose, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCommunityDiscussionByCommunityIdQuery } from '@/features/community/api/CommunityDiscussion.api';
import { useEffect, useRef, useState, type ChangeEvent, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { CommunityDiscussionModel } from '../../../types/CommunityDiscussionModel';

import './DiscussionList.scss';

interface DiscussionListProps {
    setShowDiscussion: (value: SetStateAction<boolean>) => void;
    handleDiscussion: (discussion: CommunityDiscussionModel) => void;
    communityId: number;
    isCommunityMember: boolean;
    emailVerified: boolean;
    setShowCreateDiscussion: (value: SetStateAction<boolean>) => void;
}

const DiscussionList: React.FC<DiscussionListProps> = ({ setShowDiscussion, handleDiscussion, communityId, isCommunityMember, emailVerified, setShowCreateDiscussion }) => {
    const { t } = useTranslation("communication/community/discussion");

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityDiscussionSize ?? 5);

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [filteredContent, setFilteredContent] = useState("");

    const { data: discussions, isLoading } = useGetCommunityDiscussionByCommunityIdQuery({ communityId, page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!discussions) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < discussions.count);
    }, [page, discussions]);

    const hidePeopleInspectionMode = () => {
        setShowDiscussion(false);
    }

    const openDiscussion = (item: CommunityDiscussionModel) => {
        setShowDiscussion(false);

        handleDiscussion(item);
    }

    const filterDiscussionsHandle = (event: ChangeEvent<HTMLInputElement>) => {
        const content = event.target.value;

        setFilteredContent(content);
    }

    const clear = () => {
        setFilteredContent("");
    }

    if (!discussions || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="discussion-inspection">
            <div className="title">
                <FontAwesomeIcon
                    icon={showSearchPeople ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    title={showSearchPeople ? t("HideSearchDiscussion") : t("ShowSearchDiscussion")}
                    onClick={() => setShowSearchPeople(prev => !prev)}
                />
                <div>{t("Discussion")}</div>
                <div className="actions">
                    {(isCommunityMember && emailVerified) &&
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
            <div className={`mb-3 discussion-inspection__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputDiscussion" className="form-label">{t("SearchDiscussion")}</label>
                <div className="discussion-inspection__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeDiscussion")} id="inputDiscussion"
                        onChange={filterDiscussionsHandle} value={filteredContent} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
                        onClick={clear}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="discussion-inspection__content">
                {filteredContent !== ""
                    ? discussions?.discussions.filter(x => x.title.toLowerCase().startsWith(filteredContent.toLowerCase())).map((item) => (
                        <li key={item.id} title={item.title} onClick={() => openDiscussion(item)}>
                            {item.title}
                        </li>
                    ))
                    : discussions?.discussions.map((item) => (
                        <li key={item.id} title={item.title} onClick={() => openDiscussion(item)}>
                            {item.title}
                        </li>
                    ))
                }
            </ul>
            {hasMore &&
                <div onClick={() => setPage(prev => prev + 1)} className="post-comments__load-more">Load more</div>
            }
            <div className="item-result">
                <div className="btn-shadow" onClick={hidePeopleInspectionMode}>
                    <FontAwesomeIcon
                        icon={faClose}
                    />
                    <div>{t("Close")}</div>
                </div>
            </div>
        </div>
    );
}

export default DiscussionList;