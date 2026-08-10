import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type ChangeEvent } from "react";
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetCommunityByUserIdQuery } from '../../api/Community.api';
import CreateCommunity from '../create/CreateCommunity';
import InvitesToCommunity from './InvitesToCommunity';
import UserCommunitiesItem from './UserCommunitiesItem';

import '../Communities.scss';

const UserCommunities: React.FC = () => {
    const { t } = useTranslation('communication/myEnvironment/myCommunities');

    const myself = useSelector((state: RootState) => state.user.value);
    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const [showCreateCommunity, setShowCreateCommunity] = useState(false);
    const [showMyCommunities, setShowMyCommunities] = useState(true);
    const [filterContent, setFilterContent] = useState("");
    const [showSearchCommunity, setShowSearchCommunity] = useState(false);
    const [skipFetching, setSkipFetching] = useState(true);

    const { data: myCommunities, isLoading } = useGetCommunityByUserIdQuery(myself?.id ?? "", {
        skip: skipFetching
    });

    const searchHandler = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        setFilterContent(e?.target.value ?? "");
    }

    useEffect(() => {
        setSkipFetching(myself === null);
    }, [myself]);

    if (isLoading || !userPrivacy) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={7}
                    hasSubMenu={true}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <InvitesToCommunity
                user={myself}
            />
            {showCreateCommunity &&
                <CreateCommunity
                    setShowCreateCommunity={setShowCreateCommunity}
                />
            }
            <div className="communities__list">
                <div className="title">
                    <div className="content">
                        <FontAwesomeIcon
                            icon={showSearchCommunity ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                            title={(showSearchCommunity ? t("HideSearchCommunity") : t("ShowSearchCommunity")) || ""}
                            onClick={() => setShowSearchCommunity(!showSearchCommunity)}
                        />
                        <div>{t("MyCommunitites")}</div>
                        {userPrivacy.emailVerified 
                            ? <div className="btn-shadow create-new-community" onClick={() => setShowCreateCommunity(true)}>
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
                        <FontAwesomeIcon
                            icon={showMyCommunities ? faEye : faEyeSlash}
                            title={(showMyCommunities ? t("Hide") : t("Show")) || ""}
                            onClick={() => setShowMyCommunities(prev => !prev)}
                        />
                    </div>
                </div>
                {showMyCommunities &&
                    <>
                        {showSearchCommunity &&
                            <div className="communities__search mb-3">
                                <label htmlFor="inputSearchCommunity" className="form-label">{t("Search")}</label>
                            <input type="text" className="form-control" id="inputSearchCommunity" placeholder={t("TypeCommunityName") || ""}
                                onChange={searchHandler} />
                            </div>
                        }
                        <ul>
                            {myCommunities?.map((item) => (
                                    <li key={item.id} className="community">
                                    <UserCommunitiesItem
                                            myCommunity={item}
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

export default UserCommunities;