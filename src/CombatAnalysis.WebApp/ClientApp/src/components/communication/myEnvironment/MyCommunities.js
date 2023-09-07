import { faArrowsRotate, faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazySearchByUserIdAsyncQuery } from '../../../store/api/communication/community/CommunityUser.api';
import InvitesToCommunity from './InvitesToCommunity';
import MyCommunitiesItem from './MyCommunitiesItem';

import '../../../styles/communication/community/communities.scss';

const MyCommunities = () => {
    const { t } = useTranslation("communication/myEnvironment/myCommunities");

    const customer = useSelector((state) => state.customer.value);

    const [showMyCommunities, setShowMyCommunities] = useState(true);
    const [filterContent, setFilterContent] = useState("");
    const [userCommunities, setUserCommunities] = useState(null);

    const [getMyCommunitiesAsync] = useLazySearchByUserIdAsyncQuery();

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    useEffect(() => {
        if (customer === undefined) {
            return;
        }

        const getUserCommunities = async () => {
            await refreshAsync();
        }

        getUserCommunities();
    }, [customer])

    const refreshAsync = async () => {
        const result = await getMyCommunitiesAsync(customer?.id);

        if (result.data !== undefined) {
            setUserCommunities(result.data);
        }
    }

    return (
        <>
            <InvitesToCommunity
                customer={customer}
            />
            <div className="communities__list">
                <div className="title">
                    <div className="content">
                        <FontAwesomeIcon
                            icon={faArrowsRotate}
                            title={t("Refresh")}
                            onClick={async () => await refreshAsync()}
                        />
                        <div>{t("MyCommunitites")}</div>
                        {showMyCommunities
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title={t("Hide")}
                                onClick={() => setShowMyCommunities(false)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title={t("Show")}
                                onClick={() => setShowMyCommunities(true)}
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
                            {userCommunities?.map((item) => (
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
        </>
    );
}

export default MyCommunities;