import { faMagnifyingGlassMinus, faMagnifyingGlassPlus, faUserXmark, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState, type ChangeEvent, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { CommunityUserModel } from '../../../community/types/CommunityUserModel';
import { useFindAllChatUsersQuery } from '../../api/GroupChatUser.api';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import GroupChatMembersItem from './GroupChatMembersItem';

import './GroupChatMembers.scss';

interface GroupChatMembersProps {
    chatId: number;
    isPopup: boolean;
    removeUsersAsync(peopleToRemove: (GroupChatUserModel | CommunityUserModel)[]): Promise<void>;
    setShowMembers?: (value: SetStateAction<boolean>) => void;
    canRemovePeople: () => boolean;
}

const GroupChatMembers: React.FC<GroupChatMembersProps> = ({ chatId, isPopup, removeUsersAsync, setShowMembers, canRemovePeople }) => {
    const { t } = useTranslation('communication/members');

    const [showRemoveUser, setShowRemoveUser] = useState(false);
    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [usersToRemove, setUsersToRemove] = useState<(GroupChatUserModel | CommunityUserModel)[]>([]);
    const [searchUsername, setSearchUsername] = useState("");

    const { data: chatMembers, refetch, isLoading } = useFindAllChatUsersQuery(chatId);

    useEffect(() => {
        refetch();
    }, []);

    const showRemoveUsersHandle = () => {
        setUsersToRemove([]);

        setShowRemoveUser((item) => !item);
    }

    const hidePeopleInspectionMode = () => {
        setUsersToRemove([]);

        if (isPopup && setShowMembers) {
            setShowMembers(false);
        }

        setShowRemoveUser(false);
    }

    const searchUsernameHandle = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        const content = e?.target.value;

        setSearchUsername(content ?? "");
    }

    const clear = () => {
        setSearchUsername("");
    }

    const getUsers = () => {
        const existUsers = searchUsername === ""
            ? chatMembers
            : chatMembers?.filter(user => user.username.toLowerCase().startsWith(searchUsername.toLowerCase()));

        return existUsers?.map(user => (
            <li className="user-target-community" key={user.id}>
                <GroupChatMembersItem
                    user={user}
                    usersToRemove={usersToRemove}
                    setUsersToRemove={setUsersToRemove}
                    showRemoveUser={showRemoveUser}
                />
            </li>
        ))
    }

    if (!chatMembers || isLoading) {
        return (<div>Loading...</div>);
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
                        onClick={showRemoveUsersHandle}
                    />
                }
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername") || ""} id="inputUsername" value={searchUsername} onChange={searchUsernameHandle} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean") || ""}
                        onClick={clear}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <ul className="list">
                {getUsers()}
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