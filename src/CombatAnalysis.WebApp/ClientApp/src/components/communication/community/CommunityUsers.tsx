import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faUserXmark, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { CommunityUser } from '../../../types/CommunityUser';
import { CommunityUsersProps } from '../../../types/components/communication/community/CommunityUsersProps';
import CommunityUsersItem from './CommunityUsersItem';

import "../../../styles/communication/members.scss";

const CommunityUsers: React.FC<CommunityUsersProps> = ({ me, communityUsers, removeUsersAsync, setShowMembers, isPopup, canRemovePeople }) => {
    const { t } = useTranslation("communication/members");

    const [showRemoveUser, setShowRemoveUser] = useState(false);
    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [usersToRemove, setUsersToRemove] = useState<CommunityUser[]>([]);
    const [searchUsername, setSearchUsername] = useState("");

    const handleShowRemoveUsers = () => {
        setUsersToRemove([]);

        setShowRemoveUser((item) => !item);
    }

    const hidePeopleInspectionMode = () => {
        setUsersToRemove([]);

        isPopup && setShowMembers(false);
        setShowRemoveUser(false);
    }

    const handleSearchUsername = (event: any) => {
        const content = event.target.value;

        setSearchUsername(content);
    }

    const clear = () => {
        setSearchUsername("");
    }

    return (
        <div className={`people-inspection${isPopup ? "__popup" : "__window"} ${isPopup ? "box-shadow" : ""}`}>
            <div className="title">
                <FontAwesomeIcon
                    icon={showSearchPeople ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    title={(showSearchPeople ? t("HideSearchPeople") : t("ShowSearchPeople")) || ""}
                    onClick={() => setShowSearchPeople(prev => !prev)}
                />
                <div>{t("Members")}</div>
                {canRemovePeople() &&
                    <FontAwesomeIcon
                        icon={faUserXmark}
                        className={`remove${showRemoveUser ? "_active" : ""}`}
                        title={t("Remove") || ""}
                        onClick={handleShowRemoveUsers}
                    />
                }
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername") || ""} id="inputUsername" value={searchUsername} onChange={handleSearchUsername} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean") || ""}
                        onClick={clear}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="list">
                {searchUsername === ""
                    ? communityUsers?.map((communityUser: CommunityUser) => (
                        <li className="user-target-community" key={communityUser.id}>
                            <CommunityUsersItem
                                me={me}
                                communityUser={communityUser}
                                usersToRemove={usersToRemove}
                                setUsersToRemove={setUsersToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                    ))
                    : communityUsers?.filter((communityUser: CommunityUser) => communityUser.username.toLowerCase().startsWith(searchUsername.toLowerCase())).map((communityUser: CommunityUser) => (
                        <li className="user-target-community" key={communityUser.id}>
                            <CommunityUsersItem
                                me={me}
                                communityUser={communityUser}
                                usersToRemove={usersToRemove}
                                setUsersToRemove={setUsersToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                    ))}
            </ul>
            <div className="item-result">
                {(canRemovePeople() && showRemoveUser) &&
                    <div className="btn-border-shadow" onClick={async () => await removeUsersAsync(usersToRemove)}>{t("Accept")}</div>
                }
                {isPopup &&
                    <div className="btn-border-shadow" onClick={hidePeopleInspectionMode}>{t("Close")}</div>
                }
            </div>
        </div>
    );
}

export default CommunityUsers;