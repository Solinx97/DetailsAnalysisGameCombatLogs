import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import CommunityList from './CommunityList';

import '../../../styles/communication/community/communities.scss';

const Communities = () => {
    const { t } = useTranslation("communication/community/Communities");

    const [showCommunities, setShowCommunities] = useState(false);
    const [filterContent, setFilterContent] = useState("");

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    return (
        <div className="communities__list">
            <div className="title">
                <div className="content">
                    <FontAwesomeIcon
                        icon={faArrowsRotate}
                        title={t("Refresh")}
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
                    <div className="communities__search mb-3">
                        <label htmlFor="inputSearchCommunity" className="form-label">{t("Search")}</label>
                        <input type="text" className="form-control" id="inputSearchCommunity" placeholder={t("TypeCommunityName")} onChange={searchHandler} />
                    </div>
                    <CommunityList
                        filterContent={filterContent}
                    />
                </>
            }
        </div>
    );
}

export default Communities;