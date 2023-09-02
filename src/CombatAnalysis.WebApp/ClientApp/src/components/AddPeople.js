import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus, faUserPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetCustomersQuery } from '../store/api/UserApi';
import { useFriendSearchMyFriendsQuery } from '../store/api/communication/myEnvironment/Friend.api';
import AddFriendItem from './AddFriendItem';

import '../styles/addPeople.scss';

const defaultMaxPeopleItems = 5;
const defaultMaxFriendsItems = 5;

const AddPeople = ({ customer, communityUsersId, peopleToJoin, setPeopleToJoin }) => {
    const { t } = useTranslation("addPeople");

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxPeopleItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxFriendsItems);

    const { friends, isLoading: friendsIsLoading } = useFriendSearchMyFriendsQuery(customer?.id, {
        selectFromResult: ({ data }) => ({
            friends: data?.length <= defaultMaxFriendsItems
                ? data?.filter((item) => !communityUsersId.includes(customer?.id === item.whoFriendId ? item.forWhomId : item.whoFriendId))
                : data?.filter((item) => !communityUsersId.includes(customer?.id === item.whoFriendId ? item.forWhomId : item.whoFriendId))
                    .slice(0, maxFriendsItems)
        }),
    });
    const { people, isLoading: peopleIsLoading } = useGetCustomersQuery(undefined, {
        selectFromResult: ({ data }) => ({
            people: data?.length <= defaultMaxPeopleItems
                ? data?.filter((item) => !communityUsersId.includes(item.id))
                : data?.filter((item) => !communityUsersId.includes(item.id))
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
        const people = selectedPeopleToJoin;
        people.push(user);

        setSelectedPeopleToJoin(people);
        setPeopleToJoin(people);
    }

    const handleRemoveUserFromToJoin = (user) => {
        const people = selectedPeopleToJoin.filter((selectedUser) => selectedUser.id !== user.id);

        setSelectedPeopleToJoin(people);
        setPeopleToJoin(people);
    }

    const handlerSearch = (e) => {
        const filteredPeople = people.filter((item) => item.username.toLowerCase().startsWith(e.target.value.toLowerCase()));
        setFilteredPeople(filteredPeople);
    }

    const cleanSearch = () => {
        filterContent.current.value = "";

        setFilteredPeople([]);
    }

    const arePeopleMoreThenLimit = (limit) => {
        if (people === undefined || filterContent.current === null) {
            return false;
        }

        const count = people.filter((item) => item.username.toLowerCase().startsWith(filterContent.current.value.toLowerCase())).length;
        return count === limit;
    }

    if (friendsIsLoading || peopleIsLoading) {
        return <></>;
    }

    return (
        <div className="add-new-people">
            <div className="add-new-people__title">
                <div>{t("InvitePeople")}</div>
                {showSearchPeople
                    ? <FontAwesomeIcon
                        icon={faMagnifyingGlassMinus}
                        title={t("ShowSearchPeople")}
                        onClick={() => setShowSearchPeople((item) => !item)}
                    />
                    : <FontAwesomeIcon
                        icon={faMagnifyingGlassPlus}
                        title={t("HideSearchPeople")}
                        onClick={() => setShowSearchPeople((item) => !item)}
                    />
                }
            </div>
            <div className={`mb-3 add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <label htmlFor="inputUsername" className="form-label">{t("SearchPeople")}</label>
                <div className="add-new-people__search-input">
                    <input type="text" className="form-control" placeholder={t("TypeUsername")} id="inputUsername"
                        ref={filterContent} onChange={handlerSearch } />
                    <FontAwesomeIcon
                        icon={faXmark}
                        title={t("Clean")}
                        onClick={cleanSearch}
                    />
                </div>
            </div>
            <div>
                <div className="add-new-people__content_active">
                    <div className="add-new-people__content-title">
                        <div>{t("Friends")}</div>
                        {showFriendList
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title={t("Hide")}
                                onClick={() => setShowFriendList((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title={t("Show")}
                                onClick={() => setShowFriendList((item) => !item)}
                            />
                        }
                    </div>
                    <ul className={`add-new-people__list${showFriendList ? "_active" : ""}`}>
                        {friends?.length > 0
                            ? friends.map((item) => (
                                <li key={item.id} className="person">
                                    <AddFriendItem
                                        friendUserId={item.whoFriendId === customer.id ? item.forWhomId : item.whoFriendId}
                                        filterContent={filterContent.current === null ? "" : filterContent.current.value}
                                        addUserIdToList={handleAddUserToJoin}
                                        removeUserIdToList={handleRemoveUserFromToJoin}
                                        peopleIdToJoin={selectedPeopleToJoin}
                                    />
                                </li>
                                ))
                            : <li className="empty">{t("Empty")}</li>
                        }
                    </ul>
                    {showFriendList &&
                        <div className={`add-new-people__more${arePeopleMoreThenLimit(defaultMaxFriendsItems) ? "_active" : ""}`}>
                            {maxFriendsItems > defaultMaxFriendsItems
                                ? <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowLessPeople")}
                                    onClick={() => setMaxFriendsItems(defaultMaxFriendsItems)}
                                  >
                                    {t("Less")}
                                  </button>
                                : <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowMorePeople")}
                                    onClick={() => setMaxFriendsItems(-1)}
                                  >
                                      {t("More")}
                                  </button>
                            }
                        </div>
                    }
                </div>
                <div className={`add-new-people__content${filterContent.current?.value !== "" ? "_active" : ""}`}>
                    <div className="add-new-people__content-title">
                        <div>{t("AnotherPeople")}</div>
                        {showPeopleList
                            ? <FontAwesomeIcon
                                icon={faEye}
                                title={t("Hide")}
                                onClick={() => setShowPeopleList((item) => !item)}
                            />
                            : <FontAwesomeIcon
                                icon={faEyeSlash}
                                title={t("Show")}
                                onClick={() => setShowPeopleList((item) => !item)}
                            />
                        }
                    </div>
                    <ul className={`add-new-people__list${showPeopleList ? "_active" : ""}`}>
                        {filteredPeople.length > 0
                            ? filteredPeople.map((user) => (
                                    <li key={user.id} className="person">
                                    <div>{user.username}</div>
                                        {selectedPeopleToJoin.filter((item) => item.id === user.id).length > 0
                                            ? <FontAwesomeIcon
                                                icon={faUserPlus}
                                                title={t("CancelRequest")}
                                                onClick={() => handleRemoveUserFromToJoin(user)}
                                            />
                                            : <FontAwesomeIcon
                                                icon={faPlus}
                                                title={t("SendInvite")}
                                                onClick={() => handleAddUserToJoin(user)}
                                            />
                                        }
                                    </li>
                                ))
                            : <li className="empty">{t("Empty")}</li>
                        }
                    </ul>
                    {showPeopleList &&
                        <div className={`add-new-people__more${arePeopleMoreThenLimit(defaultMaxPeopleItems) ? "_active" : ""}`}>
                            {maxPeopleItems > defaultMaxPeopleItems
                                ? <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowLessPeople")}
                                    onClick={() => setMaxPeopleItems(defaultMaxPeopleItems)}
                                   >
                                      {t("Less")}
                                   </button>
                                : <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowMorePeople")}
                                    onClick={() => setMaxPeopleItems(-1)}
                                  >
                                     {t("More")}
                                  </button>
                            }
                        </div>
                    }
                </div>
            </div>
        </div>
    );
}

export default AddPeople;