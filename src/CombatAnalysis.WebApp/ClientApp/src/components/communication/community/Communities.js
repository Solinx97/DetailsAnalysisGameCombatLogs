import { faArrowsRotate, faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetCommunitiesQuery } from '../../../store/api/CommunityApi';
import CommunityList from './CommunityList';

import '../../../styles/communication/community/communities.scss';

const Communities = ({ showCommunitiesAtStart = false }) => {
    const { t } = useTranslation("communication/community/communities");

    const [showCommunities, setShowCommunities] = useState(showCommunitiesAtStart);
    const [filterContent, setFilterContent] = useState("");
    const [communities, setCommunities] = useState(null);
    const [showSearchCommunity, setShowSearchCommunity] = useState(false);

    const [getCommunitiesAsync] = useLazyGetCommunitiesQuery();

    useEffect(() => {
        const getCommunities = async () => {
            await refreshAsync();
        }

        getCommunities();
    }, [])

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    const refreshAsync = async () => {
        const result = await getCommunitiesAsync();

        if (result.data !== undefined) {
            setCommunities(result.data);
        }
    }

    return (
        <div className="communities__list">
            <div className="title">
                <div className="content">
                    {showSearchCommunity
                        ? <FontAwesomeIcon
                            icon={faMagnifyingGlassMinus}
                            title={t("HideSearchCommunity")}
                            onClick={() => setShowSearchCommunity(false)}
                        />
                        : <FontAwesomeIcon
                            icon={faMagnifyingGlassPlus}
                            title={t("ShowSearchCommunity")}
                            onClick={() => setShowSearchCommunity(true)}
                        />
                    }
                    <FontAwesomeIcon
                        icon={faArrowsRotate}
                        title={t("Refresh")}
                        onClick={async () => await refreshAsync()}
                    />
                    <div>{t("Communities")}</div>
                    {showCommunities
                        ? <FontAwesomeIcon
                            icon={faEye}
                            title={t("Hide")}
                            onClick={() => setShowCommunities((item) => !item)}
                        />
                        : <FontAwesomeIcon
                            icon={faEyeSlash}
                            title={t("Show")}
                            onClick={() => setShowCommunities((item) => !item)}
                        />
                    }
                </div>
            </div>
            {showCommunities &&
                <>
                    {showSearchCommunity &&
                        <div className="communities__search mb-3">
                            <label htmlFor="inputSearchCommunity" className="form-label">{t("Search")}</label>
                            <input type="text" className="form-control" id="inputSearchCommunity" placeholder={t("TypeCommunityName")} onChange={searchHandler} />
                        </div>
                    }
                    <CommunityList
                        filterContent={filterContent}
                        communities={communities}
                    />
                </>
            }
        </div>
    );
}

export default Communities;