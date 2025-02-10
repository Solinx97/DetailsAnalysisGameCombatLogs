import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faUserXmark, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { GroupChatUser } from '../../../types/GroupChatUser';
import { GroupChatMembersProps } from '../../../types/components/communication/chats/GroupChatMembersProps';
import GroupChatMembersItem from './GroupChatMembersItem';

import "../../../styles/communication/members.scss";

const GroupChatMembers: React.FC<GroupChatMembersProps> = ({ me, groupChatUsers, removeUsersAsync, setShowMembers, isPopup, canRemovePeople }) => {
    const { t } = useTranslation("communication/members");

    const [showRemoveUser, setShowRemoveUser] = useState(false);
    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [usersToRemove, setUsersToRemove] = useState<GroupChatUser[]>([]);
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

    const handleSearchUsername = (event : any) => {
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
                    ? groupChatUsers?.map((groupChatUser: GroupChatUser) => (
                        <li className="user-target-community" key={groupChatUser.id}>
                            <GroupChatMembersItem
                                me={me}
                                groupChatUser={groupChatUser}
                                usersToRemove={usersToRemove}
                                setUsersToRemove={setUsersToRemove}
                                showRemoveUser={showRemoveUser}
                            />
                        </li>
                    ))
                    : groupChatUsers?.filter((groupChatUser: GroupChatUser) => groupChatUser.username.toLowerCase().startsWith(searchUsername.toLowerCase())).map((groupChatUser: GroupChatUser) => (
                        <li className="user-target-community" key={groupChatUser.id}>
                            <GroupChatMembersItem
                                me={me}
                                groupChatUser={groupChatUser}
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

export default GroupChatMembers;