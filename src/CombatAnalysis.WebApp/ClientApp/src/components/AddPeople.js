import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus, faUserPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUsersQuery } from '../store/api/UserApi';
import { useFriendSearchMyFriendsQuery } from '../store/api/communication/myEnvironment/Friend.api';
import AddFriendItem from './AddFriendItem';

import '../styles/addPeople.scss';

const defaultMaxItems = 5;

const AddPeople = ({ user, communityUsersId, peopleToJoin, setPeopleToJoin }) => {
    const { t } = useTranslation("addPeople");

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxItems);

    const { friends, isLoading: friendsIsLoading } = useFriendSearchMyFriendsQuery(user?.id, {
        selectFromResult: ({ data }) => ({
            friends: data?.filter((item) => !communityUsersId.includes(user?.id === item.whoFriendId ? item.forWhomId : item.whoFriendId))
                .slice(0, maxFriendsItems)
        }),
    });
    const { people, isLoading: peopleIsLoading } = useGetUsersQuery(undefined, {
        selectFromResult: ({ data }) => ({
            people: data?.filter((item) => !communityUsersId.includes(item.id))
                .slice(0, maxPeopleItems)
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

    const arePeopleMoreThanLimit = (limit) => {
        if (people === undefined || filterContent.current === null) {
            return false;
        }

        const count = people.filter((item) => item.username.toLowerCase().startsWith(filterContent.current.value.toLowerCase())).length;
        return count === limit;
    }

    if (friendsIsLoading || peopleIsLoading) {
        return <></>;
    }

    const renderList = (items, isFriendList) => {
        return (
            <ul className={`add-new-people__list${isFriendList ? "_active" : showPeopleList ? "_active" : ""}`}>
            {items?.length > 0
                ? items.map((item) => (
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

    const renderMoreButton = (maxItems, setMaxItems, defaultMaxItems) => (
        <div className={`add-new-people__more${arePeopleMoreThanLimit(defaultMaxItems) ? "_active" : ""}`}>
            {maxItems > defaultMaxItems
                ? <button className="btn btn-outline-secondary" title={t("ShowLessPeople")} onClick={() => setMaxItems(defaultMaxItems)}>{t("Less")}</button>
                : <button className="btn btn-outline-secondary" title={t("ShowMorePeople")} onClick={() => setMaxItems(-1)}>{t("More")}</button>
            }
        </div>
    )

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
                    {renderList(friends, true)}
                    {showFriendList && renderMoreButton(maxFriendsItems, setMaxFriendsItems, defaultMaxItems)}
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
                    {renderList(filteredPeople, false)}
                    {showPeopleList && renderMoreButton(maxPeopleItems, setMaxPeopleItems, defaultMaxItems)}
                </div>
            </div>
        </div>
    );
}

export default AddPeople;