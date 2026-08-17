import { useFindFriendByUserIdQuery } from '@/features/user/api/Friend.api';
import { useGetUsersQuery } from '@/features/user/api/User.api';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import type { FriendModel } from '@/features/user/types/FriendModel';
import { faEye, faEyeSlash, faMagnifyingGlassMinus, faMagnifyingGlassPlus, faPlus, faUserPlus, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import type { RootState } from '@/app/Store';
import { useRef, useState, type ChangeEvent, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import AddFriendItem from './AddFriendItem';

import './AddPeople.scss';

interface AddPeopleProps {
    usersId: string[];
    peopleToJoin: AppUserModel[];
    setPeopleToJoin: (value: SetStateAction<AppUserModel[]>) => void;
}

const AddPeople: React.FC<AddPeopleProps> = ({ usersId, peopleToJoin, setPeopleToJoin }) => {
    const defaultMaxItems = 3;

    const { t } = useTranslation('addPeople');

    const myself = useSelector((state: RootState) => state.user.value);

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxItems);

    const { friends } = useFindFriendByUserIdQuery(myself?.id ?? "", {
        selectFromResult: ({ data, isLoading }) => ({
            friends: data?.filter((item) => !usersId.includes(myself?.id === item.whoFriendId ? item.forWhomId : item.whoFriendId)),
            isLoading,
        }),
    });
    const { people } = useGetUsersQuery(undefined, {
        selectFromResult: ({ data, isLoading }) => ({
            people: data?.filter((item) => !usersId.includes(item.id)),
            isLoading
        }),
    });

    const [showSearchPeople, setShowSearchPeople] = useState(true);

    const [showFriendList, setShowFriendList] = useState(true);
    const [showPeopleList, setShowPeopleList] = useState(true);

    const [selectedPeopleToJoin, setSelectedPeopleToJoin] = useState(peopleToJoin);
    const [filteredPeople, setFilteredPeople] = useState<AppUserModel[]>([]);

    const filterContent = useRef<HTMLInputElement | null>(null);

    const handleAddUserToJoin = (user: AppUserModel) => {
        const updatedPeople = [...selectedPeopleToJoin, user];
        setSelectedPeopleToJoin(updatedPeople);
        setPeopleToJoin(updatedPeople);
    }

    const handleRemoveUserFromToJoin = (user: AppUserModel) => {
        const updatedPeople = selectedPeopleToJoin.filter((selectedUser) => selectedUser.id !== user.id);
        setSelectedPeopleToJoin(updatedPeople);
        setPeopleToJoin(updatedPeople);
    }

    const handlerSearch = (e: ChangeEvent<HTMLInputElement> | undefined) => {
        if (!people) {
            return;
        }

        const searchValue = e ? e?.target.value.toLowerCase() : "";
        const foundPeople = people.filter((user: AppUserModel) => user.username.toLowerCase().startsWith(searchValue));
        setFilteredPeople(foundPeople);
    }

    const cleanSearch = () => {
        if (filterContent.current !== null) {
            filterContent.current.value = "";
        }

        setFilteredPeople([]);
    }

    const renderFriendsList = (items: FriendModel[], maxItems: number) => {
        return (
            <ul className="add-new-people__list_active">
                {items?.length > 0
                    ? items.slice(0, maxItems).map((item: FriendModel) => (
                        <li key={item.id} className="person">
                            <AddFriendItem
                                friendUserId={item.whoFriendId === myself?.id ? item.forWhomId : item.whoFriendId}
                                filterContent={filterContent.current?.value || ""}
                                addUserToList={handleAddUserToJoin}
                                removeUserToList={handleRemoveUserFromToJoin}
                                peopleIdToJoin={selectedPeopleToJoin}
                            />
                        </li>
                    ))
                    : <li className="empty">{t("Empty")}</li>
                }
            </ul>
        );
    }

    const renderUserList = (items: AppUserModel[], maxItems: number) => {
        return (
            <ul className={`add-new-people__list${showPeopleList ? "_active" : ""}`}>
                {items?.length > 0
                    ? items.slice(0, maxItems).map((item: AppUserModel) => (
                        <li key={item.id} className="person">
                            <>
                                <div>{item.username}</div>
                                {selectedPeopleToJoin.some((user: AppUserModel) => user.id === item.id)
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

    const renderMoreButton = (targetCollectionSize: number, maxItems: number, setMaxItems: (value: SetStateAction<number>) => void, defaultMaxItems: number) => {
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
                    {(showFriendList && friends) &&
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