import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useCommunityUserSearchByUserIdQuery } from '../../../store/api/community/CommunityUser.api';
import Loading from '../../Loading';
import VerificationRestriction from '../../common/VerificationRestriction';
import InvitesToCommunity from './InvitesToCommunity';
import MyCommunitiesItem from './MyCommunitiesItem';

import '../../../styles/communication/community/communities.scss';

const MyCommunities = () => {
    const { t } = useTranslation("communication/myEnvironment/myCommunities");

    const me = useSelector((state) => state.user.value);
    const userPrivacy = useSelector((state) => state.userPrivacy.value);

    const navigate = useNavigate();

    const [showMyCommunities, setShowMyCommunities] = useState(true);
    const [filterContent, setFilterContent] = useState("");
    const [showSearchCommunity, setShowSearchCommunity] = useState(false);
    const [skipFetching, setSkipFetching] = useState(true);

    const { data: myCommunities, isLoading } = useCommunityUserSearchByUserIdQuery(me?.id, {
        skip: skipFetching
    });

    const navigateToCreateCommunity = () => navigate("/communities/create");

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    useEffect(() => {
        me !== null ? setSkipFetching(false) : setSkipFetching(true);
    }, [me]);

    if (isLoading || !userPrivacy) {
        return (<Loading />);
    }

    return (
        <>
            <InvitesToCommunity
                user={me}
            />
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
                        <div>{t("MyCommunitites")}</div>
                        {userPrivacy.emailVerified 
                            ? <div className="btn-shadow create-new-community" onClick={navigateToCreateCommunity}>
                                <FontAwesomeIcon
                                    icon={faPlus}
                                />
                                <div>{t("CreateNew")}</div>
                            </div>
                            : <VerificationRestriction
                                contentText={t("CreateNew")}
                                infoText={t("VerificationCreateCommunity")}
                            />
                        }
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
                        {showSearchCommunity &&
                            <div className="communities__search mb-3">
                                <label htmlFor="inputSearchCommunity" className="form-label">{t("Search")}</label>
                                <input type="text" className="form-control" id="inputSearchCommunity" placeholder={t("TypeCommunityName")} onChange={searchHandler} />
                            </div>
                        }
                        <ul>
                            {myCommunities?.map((item) => (
                                    <li key={item.id} className="community">
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