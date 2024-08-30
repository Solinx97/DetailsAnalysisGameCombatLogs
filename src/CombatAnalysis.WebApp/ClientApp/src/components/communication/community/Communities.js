import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import CommunityList from './CommunityList';

import '../../../styles/communication/community/communities.scss';

const Communities = ({ showCommunitiesAtStart = false }) => {
    const { t } = useTranslation("communication/community/communities");

    const [showCommunities, setShowCommunities] = useState(showCommunitiesAtStart);
    const [filterContent, setFilterContent] = useState("");
    const [showSearchCommunity, setShowSearchCommunity] = useState(false);

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    return (
        <div className="communities__list">
            <div className="title">
                <div className="content">
                    <FontAwesomeIcon
                        icon={showSearchCommunity ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                        title={showSearchCommunity ? t("HideSearchCommunity") : t("ShowSearchCommunity")}
                        onClick={() => setShowSearchCommunity((item) => !item)}
                    />
                    <div>{t("Communities")}</div>
                    <FontAwesomeIcon
                        icon={showCommunities ? faEye : faEyeSlash}
                        title={showCommunities ? t("Hide") : t("Show")}
                        onClick={() => setShowCommunities((item) => !item)}
                    />
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
                    />
                </>
            }
        </div>
    );
}

export default Communities;