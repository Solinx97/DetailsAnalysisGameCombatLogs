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

    const handleShowRemoveUsers = () => {
        setShowRemoveUser((item) => !item);

        setPeopleToRemove([]);
    }

    const hidePeopleInspectionMode = () => {
        setPeopleToRemove([]);

        isPopup && setShowMembers(false);
        setShowRemoveUser(false);
    }

    return (
        <div className={`people-inspection${isPopup ? "__popup" : "__window"}`}>
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
                    <input type="text" className="form-control" placeholder={t("TypeUsername")} id="inputUsername"/>
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="list">
                {users?.map((item) => (
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
                }
            </ul>
            <div className="item-result">
                {canRemovePeople &&
                    <input type="button" value={t("Accept")} className="btn btn-success" onClick={async () => await removeUsersAsync(peopleToRemove)} />
                }
                {isPopup &&
                    <input type="button" value={t("Close")} className="btn btn-secondary" onClick={hidePeopleInspectionMode} />
                }
            </div>
        </div>
    );
}

export default Members;