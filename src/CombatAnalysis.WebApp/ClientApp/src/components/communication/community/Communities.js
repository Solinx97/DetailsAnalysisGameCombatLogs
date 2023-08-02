import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { NavLink, useNavigate } from 'react-router-dom';
import { useGetCommunitiesQuery } from '../../../store/api/ChatApi';
import MyCommunities from '../myEnvironment/MyCommunities';

import '../../../styles/communication/communities.scss';

const Communities = () => {
    const { t } = useTranslation("communication/community/Communities");

    const [showCommunities, setShowCommunities] = useState(true);

    const { data: communities, isLoading } = useGetCommunitiesQuery();

    const navigate = useNavigate();

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="communities">
            <MyCommunities/>
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
                    <ul>
                        {
                            communities?.map((item) => (
                                <li key={item.id} className="community">
                                    <div className="card">
                                        <div className="card-body">
                                            <h5 className="card-title">{item.name}</h5>
                                            <p className="card-text">{item.description}</p>
                                            <NavLink
                                                className="card-link"
                                                onClick={() => navigate(`/community?id=${item.id}`)}
                                            >
                                                {t("Open")}
                                            </NavLink>
                                            <NavLink
                                                className="card-link"
                                            >
                                                {t("MoreDetails")}
                                            </NavLink>
                                        </div>
                                    </div>
                                </li>
                            ))
                        }
                    </ul>
                }
            </div>
        </div>
    );
}

export default Communities;