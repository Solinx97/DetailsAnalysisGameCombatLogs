import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/CommunityUser.api';
import CreateCommunity from './CreateCommunity';
import InvitesToCommunity from './InvitesToCommunity';
import MyCommunitiesItem from './MyCommunitiesItem';

import '../../../styles/communication/community/communities.scss';

const MyCommunities = () => {
    const { t } = useTranslation("communication/myEnvironment/myCommunities");

    const customer = useSelector((state) => state.customer.value);

    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    const [showCreateCommunity, setShowCreateCommunity] = useState(false);
    const [showMyCommunities, setShowMyCommunities] = useState(true);

    const [filterContent, setFilterContent] = useState("");

    if (isLoading) {
        return <></>;
    }

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    return (
        <>
            <InvitesToCommunity
                customer={customer}
            />
            <div className="communities__list">
                <div className="title">
                    <button type="button" className="btn btn-success" onClick={() => setShowCreateCommunity((item) => !item)}>{t("Create")}</button>
                    <div className="content">
                        <FontAwesomeIcon
                            icon={faArrowsRotate}
                            title={t("Refresh")}
                        />
                        <div>{t("MyCommunitites")}</div>
                        {showMyCommunities
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title={t("Hide")}
                                onClick={() => setShowMyCommunities((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title={t("Show")}
                                onClick={() => setShowMyCommunities((item) => !item)}
                            />
                        }
                    </div>
                </div>
                {showMyCommunities &&
                    <>
                        <div className="communities__search mb-3">
                            <label htmlFor="inputSearchCommunity" className="form-label">{t("Search")}</label>
                            <input type="text" className="form-control" id="inputSearchCommunity" placeholder={t("TypeCommunityName")} onChange={searchHandler} />
                        </div>
                        <ul>
                            {
                                userCommunities?.map((item) => (
                                    <li key={item.id}>
                                        <MyCommunitiesItem
                                            userCommunity={item}
                                            filterContent={filterContent}
                                        />
                                    </li>
                                ))
                            }
                        </ul>
                    </>
                }
            </div>
            {showCreateCommunity &&
                <CreateCommunity
                    customer={customer}
                    setShowCreateCommunity={setShowCreateCommunity}
                />
            }
        </>
    );
}

export default MyCommunities;