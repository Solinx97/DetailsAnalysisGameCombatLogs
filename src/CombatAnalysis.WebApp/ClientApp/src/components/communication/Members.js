import { faUserXmark, faXmark, faMagnifyingGlassMinus, faMagnifyingGlassPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import MembersItem from './MembersItem';

import "../../styles/communication/members.scss";

const Members = ({ me, users, communityItem, removeUsersAsync, setShowMembers, isPopup = false, canRemovePeople = false }) => {
    const { t } = useTranslation("communication/members");

    const [showRemoveUser, setShowRemoveUser] = useState(false);
    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [peopleToRemove, setPeopleToRemove] = useState([]);
    const [searchUsername, setSearchUsername] = useState("");

    const handleShowRemoveUsers = () => {
        setShowRemoveUser((item) => !item);

        setPeopleToRemove([]);
    }

    const hidePeopleInspectionMode = () => {
        setPeopleToRemove([]);

        isPopup && setShowMembers(false);
        setShowRemoveUser(false);
    }

    const handleSearchUsername = (event) => {
        const content = event.target.value;

        setSearchUsername(content);
    }

    const clear = () => {
        setSearchUsername("");
    }

    return (
        <div className={`people-inspection${isPopup ? "__popup" : "__window"} ${isPopup ? "box-shadow" : ""}`}>
            <div className="title">
                {showSearchPeople
                    ? <FontAwesomeIcon
                        icon={faMagnifyingGlassMinus}
                        title={t("HideSearchPeople")}
                        onClick={() => setShowSearchPeople(false)}
                    />
                    : <FontAwesomeIcon
                        icon={faMagnifyingGlassPlus}
                        title={t("ShowSearchPeople")}
                        onClick={() => setShowSearchPeople(true)}
                    />
                }
                <div>{t("Members")}</div>
                {canRemovePeople &&
                    <FontAwesomeIcon
                        icon={faUserXmark}
                        className={`remove${showRemoveUser ? "_active" : ""}`}
                        title={t("Remove")}
                        onClick={handleShowRemoveUsers}
                    />
                }
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername")} id="inputUsername" value={searchUsername} onChange={handleSearchUsername} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
                        onClick={clear}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="list">
                {searchUsername === ""
                    ? users?.map((item) => (
                        <li className="user-target-community" key={item.id}>
                            <MembersItem
                                me={me}
                                user={item}
                                communityItem={communityItem}
                                peopleToRemove={peopleToRemove}
                                setPeopleToRemove={setPeopleToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                    ))
                    : users?.filter(x => x.username.toLowerCase().startsWith(searchUsername.toLowerCase())).map((item) => (
                        <li className="user-target-community" key={item.id}>
                            <MembersItem
                                me={me}
                                user={item}
                                communityItem={communityItem}
                                peopleToRemove={peopleToRemove}
                                setPeopleToRemove={setPeopleToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                ))}
            </ul>
            <div className="item-result">
                {(canRemovePeople && showRemoveUser) &&
                    <div className="btn-border-shadow" onClick={async () => await removeUsersAsync(peopleToRemove)}>{t("Accept")}</div>
                }
                {isPopup &&
                    <div className="btn-border-shadow" onClick={hidePeopleInspectionMode}>{t("Close")}</div>
                }
            </div>
        </div>
    );
}

export default Members;