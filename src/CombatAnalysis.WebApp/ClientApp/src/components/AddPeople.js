import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFriendSearchByUserIdQuery, useGetCustomersQuery } from '../store/api/UserApi';
import AddFriendItem from './AddFriendItem';

import '../styles/addPeople.scss';

const defaultMaxPeopleItems = 5;
const defaultMaxFriendsItems = 5;

const AddPeople = ({ customer, communityUsersId, peopleToJoin, setPeopleToJoin }) => {
    const { t } = useTranslation("addPeople");

    const { friends, isLoading: friendsIsLoading } = useFriendSearchByUserIdQuery(customer?.id, {
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

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxPeopleItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxFriendsItems);
    const [showSearchPeople, setShowSearchPeople] = useState(false);
    const [filterContent, setFilterContent] = useState("");
    const [showFriendList, setShowFriendList] = useState(true);
    const [showPeopleList, setShowPeopleList] = useState(true);

    const [peopleIdToJoin, setPeopleIdToJoin] = useState(peopleToJoin);

    const handleAddUserIdToList = (id) => {
        const people = peopleIdToJoin;
        people.push(id);

        setPeopleIdToJoin(people);
        setPeopleToJoin(people);
    }

    const handleRemoveUserIdToList = (id) => {
        const people = peopleIdToJoin.filter((element) => element !== id);

        setPeopleIdToJoin(people);
        setPeopleToJoin(people);
    }

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    const arePeopleMoreThenLimit = (limit) => {
        if (people === undefined) {
            return false;
        }

        const count = people.filter((item) => item.username.toLowerCase().startsWith(filterContent.toLowerCase())).length;
        return count === limit;
    }

    if (friendsIsLoading || peopleIsLoading) {
        return <></>;
    }

    return (
        <div className="add-new-people">
            <div className="add-new-people__title">
                <div>Invite people</div>
                {showSearchPeople
                    ? <FontAwesomeIcon
                        icon={faMagnifyingGlassMinus}
                        title="Search people"
                        onClick={() => setShowSearchPeople((item) => !item)}
                    />
                    : <FontAwesomeIcon
                        icon={faMagnifyingGlassPlus}
                        title="Search people"
                        onClick={() => setShowSearchPeople((item) => !item)}
                    />
                }
            </div>
            <div className={`add-new-people__search${showSearchPeople ? "_active" : ""}`}>
                <div className="add-new-people__search-title">{t("SearchPeople")}</div>
                <input type="text" onChange={searchHandler} />
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
                                        filterContent={filterContent}
                                    />
                                </li>
                                ))
                            : <li className="empty">Empty</li>
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
                <div className={`add-new-people__content${filterContent !== "" ? "_active" : ""}`}>
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
                        {people?.length > 0
                            ? people.filter((item) => item.username.toLowerCase().startsWith(filterContent.toLowerCase()))
                                .map((item) => (
                                    <li key={item.id} className="person">
                                        <div>{item.username}</div>
                                        {peopleIdToJoin.includes(item.id)
                                            ? <FontAwesomeIcon
                                                icon={faUserPlus}
                                                title="Cancel request"
                                                onClick={() => handleRemoveUserIdToList(item.id)}
                                            />
                                            : <FontAwesomeIcon
                                                icon={faPlus}
                                                title={t("SendInvite")}
                                                onClick={() => handleAddUserIdToList(item.id)}
                                            />
                                        }
                                    </li>
                                ))
                            : <li className="empty">Empty</li>
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