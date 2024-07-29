import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus, faUserPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUsersQuery } from '../store/api/UserApi';
import { useFriendSearchMyFriendsQuery } from '../store/api/communication/myEnvironment/Friend.api';
import AddFriendItem from './AddFriendItem';

import '../styles/addPeople.scss';

const defaultMaxItems = 3;

const AddPeople = ({ user, communityUsersId, peopleToJoin, setPeopleToJoin }) => {
    const { t } = useTranslation("addPeople");

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxItems);

    const { friends, isLoading: friendsIsLoading } = useFriendSearchMyFriendsQuery(user?.id, {
        selectFromResult: ({ data }) => ({
            friends: data?.filter((item) => !communityUsersId.includes(user?.id === item.whoFriendId ? item.forWhomId : item.whoFriendId))
        }),
    });
    const { people, isLoading: peopleIsLoading } = useGetUsersQuery(undefined, {
        selectFromResult: ({ data }) => ({
            people: data?.filter((item) => !communityUsersId.includes(item.id))
        }),
    });

    const [showSearchPeople, setShowSearchPeople] = useState(true);

    const [showFriendList, setShowFriendList] = useState(true);
    const [showPeopleList, setShowPeopleList] = useState(true);

    const [selectedPeopleToJoin, setSelectedPeopleToJoin] = useState(peopleToJoin);
    const [filteredPeople, setFilteredPeople] = useState([]);

    const filterContent = useRef(null);

    const handleAddUserToJoin = (user) => {
        const updatedPeople = [...selectedPeopleToJoin, user];
        setSelectedPeopleToJoin(updatedPeople);
        setPeopleToJoin(updatedPeople);
    }

    const handleRemoveUserFromToJoin = (user) => {
        const updatedPeople = selectedPeopleToJoin.filter((selectedUser) => selectedUser.id !== user.id);
        setSelectedPeopleToJoin(updatedPeople);
        setPeopleToJoin(updatedPeople);
    }

    const handlerSearch = (e) => {
        const searchValue = e.target.value.toLowerCase();
        const filtered = people.filter((item) => item.username.toLowerCase().startsWith(searchValue));
        setFilteredPeople(filtered);
    }

    const cleanSearch = () => {
        filterContent.current.value = "";

        setFilteredPeople([]);
    }

    if (friendsIsLoading || peopleIsLoading) {
        return <></>;
    }

    const renderList = (items, maxItems, isFriendList) => {
        return (
            <ul className={`add-new-people__list${isFriendList ? "_active" : showPeopleList ? "_active" : ""}`}>
            {items?.length > 0
                ? items.slice(0, maxItems).map((item) => (
                    <li key={item.id} className="person">
                        {isFriendList
                            ? <AddFriendItem
                                friendUserId={item.whoFriendId === user.id ? item.forWhomId : item.whoFriendId}
                                filterContent={filterContent.current?.value || ""}
                                addUserIdToList={handleAddUserToJoin}
                                removeUserIdToList={handleRemoveUserFromToJoin}
                                peopleIdToJoin={selectedPeopleToJoin}
                            />
                            : <>
                                <div>{item.username}</div>
                                {selectedPeopleToJoin.some((selected) => selected.id === item.id)
                                    ? <FontAwesomeIcon icon={faUserPlus} title={t("CancelRequest")} onClick={() => handleRemoveUserFromToJoin(item)} />
                                    : <FontAwesomeIcon icon={faPlus} title={t("SendInvite")} onClick={() => handleAddUserToJoin(item)} />
                                }
                            </>
                        }
                    </li>
                ))
                : <li className="empty">{t("Empty")}</li>
            }
            </ul>
        );
    }

    const renderMoreButton = (targetCollectionSize, maxItems, setMaxItems, defaultMaxItems) => {
        return (
            <div className="add-new-people__more">
            {targetCollectionSize > maxItems
                ? <button className="btn btn-outline-secondary" title={t("ShowMorePeople")} onClick={() => setMaxItems(targetCollectionSize)}>{t("More")}</button>
                : <button className="btn btn-outline-secondary" title={t("ShowLessPeople")} onClick={() => setMaxItems(defaultMaxItems)}>{t("Less")}</button>
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
                    title={showSearchPeople ? t("ShowSearchPeople") : t("HideSearchPeople")}
                    onClick={() => setShowSearchPeople(!showSearchPeople)}
                />
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername")} id="inputUsername"
                        ref={filterContent} onChange={handlerSearch} />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
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
                            title={showFriendList ? t("Hide") : t("Show")}
                            onClick={() => setShowFriendList(!showFriendList)}
                        />
                    </div>
                    {showFriendList &&
                        <>
                            {renderList(friends, maxFriendsItems, true)}
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
                            title={showPeopleList ? t("Hide") : t("Show")}
                            onClick={() => setShowPeopleList(!showPeopleList)}
                        />
                    </div>
                    {showPeopleList &&
                        <>
                            {renderList(filteredPeople, maxPeopleItems, false)}
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