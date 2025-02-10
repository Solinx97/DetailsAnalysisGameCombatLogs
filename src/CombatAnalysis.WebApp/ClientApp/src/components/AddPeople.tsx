import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus, faUserPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUsersQuery } from '../store/api/core/User.api';
import { useFriendSearchMyFriendsQuery } from '../store/api/user/Friend.api';
import { AppUser } from '../types/AppUser';
import { Friend } from '../types/Friend';
import { AddPeopleProps } from '../types/components/AddPeopleProps';
import AddFriendItem from './AddFriendItem';

import '../styles/addPeople.scss';

const defaultMaxItems = 3;

const AddPeople: React.FC<AddPeopleProps> = ({ user, communityUsersId, peopleToJoin, setPeopleToJoin }) => {
    const { t } = useTranslation("addPeople");

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxItems);

    const { friends, isLoading: friendsIsLoading } = useFriendSearchMyFriendsQuery(user?.id, {
        selectFromResult: ({ data }: { data: Friend[] }) => ({
            friends: data?.filter((item) => !communityUsersId.includes(user?.id === item.whoFriendId ? item.forWhomId : item.whoFriendId))
        }),
    });
    const { people, isLoading: peopleIsLoading } = useGetUsersQuery(undefined, {
        selectFromResult: ({ data }: { data: AppUser[] }) => ({
            people: data?.filter((item) => !communityUsersId.includes(item.id))
        }),
    });

    const [showSearchPeople, setShowSearchPeople] = useState(true);

    const [showFriendList, setShowFriendList] = useState(true);
    const [showPeopleList, setShowPeopleList] = useState(true);

    const [selectedPeopleToJoin, setSelectedPeopleToJoin] = useState(peopleToJoin);
    const [filteredPeople, setFilteredPeople] = useState([]);

    const filterContent = useRef<HTMLInputElement | null>(null);

    const handleAddUserToJoin = (user: AppUser) => {
        const updatedPeople = [...selectedPeopleToJoin, user];
        setSelectedPeopleToJoin(updatedPeople);
        setPeopleToJoin(updatedPeople);
    }

    const handleRemoveUserFromToJoin = (user: AppUser) => {
        const updatedPeople = selectedPeopleToJoin.filter((selectedUser) => selectedUser.id !== user.id);
        setSelectedPeopleToJoin(updatedPeople);
        setPeopleToJoin(updatedPeople);
    }

    const handlerSearch = (e: any) => {
        const searchValue = e.target.value.toLowerCase();
        const filtered = people.filter((user: AppUser) => user.username.toLowerCase().startsWith(searchValue));
        setFilteredPeople(filtered);
    }

    const cleanSearch = () => {
        if (filterContent.current !== null) {
            filterContent.current.value = "";
        }

        setFilteredPeople([]);
    }

    if (friendsIsLoading || peopleIsLoading) {
        return (<></>);
    }

    const renderFriendsList = (items: Friend[], maxItems: number) => {
        return (
            <ul className="add-new-people__list_active">
                {items?.length > 0
                    ? items.slice(0, maxItems).map((item: Friend) => (
                        <li key={item.id} className="person">
                            <AddFriendItem
                                friendUserId={item.whoFriendId === user.id ? item.forWhomId : item.whoFriendId}
                                filterContent={filterContent.current?.value || ""}
                                addUserIdToList={handleAddUserToJoin}
                                removeUserIdToList={handleRemoveUserFromToJoin}
                                peopleIdToJoin={selectedPeopleToJoin}
                            />
                        </li>
                    ))
                    : <li className="empty">{t("Empty")}</li>
                }
            </ul>
        );
    }


    const renderUserList = (items: AppUser[], maxItems: number) => {
        return (
            <ul className={`add-new-people__list${showPeopleList ? "_active" : ""}`}>
                {items?.length > 0
                    ? items.slice(0, maxItems).map((item: AppUser) => (
                        <li key={item.id} className="person">
                            <>
                                <div>{item.username}</div>
                                {selectedPeopleToJoin.some((user: AppUser) => user.id === item.id)
                                    ? <FontAwesomeIcon icon={faUserPlus} title={t("CancelRequest") || ""} onClick={() => handleRemoveUserFromToJoin(item)} />
                                    : <FontAwesomeIcon icon={faPlus} title={t("SendInvite") || ""} onClick={() => handleAddUserToJoin(item)} />
                                }
                            </>
                        </li>
                    ))
                    : <li className="empty">{t("Empty")}</li>
                }
            </ul>
        );
    }

    const renderMoreButton = (targetCollectionSize: number, maxItems: number, setMaxItems: any, defaultMaxItems: number) => {
        return (
            <div className="add-new-people__more">
                {targetCollectionSize > maxItems
                    ? <button className="btn btn-outline-secondary" title={t("ShowMorePeople") || ""} onClick={() => setMaxItems(targetCollectionSize)}>{t("More")}</button>
                    : <button className="btn btn-outline-secondary" title={t("ShowLessPeople") || ""} onClick={() => setMaxItems(defaultMaxItems)}>{t("Less")}</button>
                }
            </div>
        )
    };

    return (
        <div className="add-new-people">
            <div className="add-new-people__title">
                <div>{t("InvitePeople")}</div>
                <FontAwesomeIcon
                    icon={showSearchPeople ? faMagnifyingGlassMinus : faMagnifyingGlassPlus}
                    title={(showSearchPeople ? t("ShowSearchPeople") : t("HideSearchPeople")) || ""}
                    onClick={() => setShowSearchPeople(!showSearchPeople)}
                />
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername") || ""} id="inputUsername"
                        ref={filterContent} onChange={handlerSearch} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean") || ""}
                        onClick={cleanSearch}
                    />
                </div>
            </div>
            <div className="divide"></div>
            <div>
                <div className="add-new-people__content_active">
                    <div className="add-new-people__content-title">
                        <div>{t("Friends")}</div>
                        <FontAwesomeIcon
                            icon={showFriendList ? faEye : faEyeSlash}
                            title={(showFriendList ? t("Hide") : t("Show")) || ""}
                            onClick={() => setShowFriendList(!showFriendList)}
                        />
                    </div>
                    {showFriendList &&
                        <>
                            {renderFriendsList(friends, maxFriendsItems)}
                            {friends?.length > defaultMaxItems &&
                                renderMoreButton(friends.length, maxFriendsItems, setMaxFriendsItems, defaultMaxItems)
                            }
                        </>
                    }
                </div>
                <div className={`add-new-people__content${filterContent.current?.value !== "" ? "_active" : ""}`}>
                    <div className="add-new-people__content-title">
                        <div>{t("AnotherPeople")}</div>
                        <FontAwesomeIcon
                            icon={showPeopleList ? faEye : faEyeSlash}
                            title={(showPeopleList ? t("Hide") : t("Show")) || ""}
                            onClick={() => setShowPeopleList(!showPeopleList)}
                        />
                    </div>
                    {showPeopleList &&
                        <>
                            {renderUserList(filteredPeople, maxPeopleItems)}
                            {filteredPeople?.length > defaultMaxItems &&
                                renderMoreButton(filteredPeople.length, maxPeopleItems, setMaxPeopleItems, defaultMaxItems)
                            }
                        </>
                    }
                </div>
            </div>
        </div>
    );
}

export default AddPeople;