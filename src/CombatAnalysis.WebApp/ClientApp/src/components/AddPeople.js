import { faEye, faEyeSlash, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFriendSearchByUserIdQuery, useGetCustomersQuery } from '../store/api/UserApi';
import AddFriendItem from './AddFriendItem';

import '../styles/addPeople.scss';

const defaultMaxPeopleItems = 5;
const defaultMaxFriendsItems = 5;
const hideInviteAlertTimer = 2000;
let hideInviteAlertTimeout = null;

const AddPeople = ({ customer, createInviteAsync, communityUsersId, setShowAddPeople }) => {
    const { t } = useTranslation("addPeople");

    const [maxPeopleItems, setMaxPeopleItems] = useState(defaultMaxPeopleItems);
    const [maxFriendsItems, setMaxFriendsItems] = useState(defaultMaxFriendsItems);
    const [showInviteAlert, setShowInviteAlert] = useState(false);
    const [invitedUsername, setInvitedUsername] = useState("");

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

    const [filterContent, setFilterContent] = useState("");
    const [showFriendList, setShowFriendList] = useState(true);
    const [showPeopleList, setShowPeopleList] = useState(true);

    const createInviteHandlerAsync = async (username, whomId) => {
        hideInviteAlertTimeout && clearTimeout(hideInviteAlertTimeout);

        const createdInvite = await createInviteAsync(whomId);
        if (createdInvite !== null) {
            setShowInviteAlert(true);
            setInvitedUsername(username);

            hideInviteAlertTimeout = setTimeout(() => {
                setShowInviteAlert(false);
            }, hideInviteAlertTimer);
        }
    }

    const handleAddNewPeopleVisibility = () => {
        setShowAddPeople(false);
    }

    const searchHandler = (e) => {
        setFilterContent(e.target.value);
    }

    if (friendsIsLoading || peopleIsLoading) {
        return <></>;
    }

    return (
        <div className="add-new-people">
            <div className="add-new-people__title"><i><strong>{t("AddNewPeople")}</strong></i></div>
            {showInviteAlert &&
                <div className="alert alert-success add-new-people__invite-alert" role="alert">
                    {t("SentInvite")} <strong>{invitedUsername}</strong>
                </div>
            }
            <input type="text" onChange={searchHandler} />
            <div>
                <div className="add-new-people__content">
                    <div className="add-new-people__content-title">
                        <div><i><strong>{t("Friends")}</strong></i></div>
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
                    <ul className="add-new-people__list">
                        {
                            showFriendList && friends?.map((item) => (
                                <li key={item.id} className="person">
                                    <AddFriendItem
                                        friendUserId={item.whoFriendId === customer.id ? item.forWhomId : item.whoFriendId}
                                        createInviteAsync={createInviteHandlerAsync}
                                        filterContent={filterContent}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                    {showFriendList && 
                        <div className="add-new-people__more">
                            {maxFriendsItems === defaultMaxFriendsItems
                                ? <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowMorePeople")}
                                    onClick={() => setMaxFriendsItems(-1)}
                                  >
                                     {t("More")}
                                  </button>
                                : <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowLessPeople")}
                                    onClick={() => setMaxFriendsItems(defaultMaxFriendsItems)}
                                  >
                                    {t("Less")}
                                  </button>
                            }
                        </div>
                    }
                </div>
                <div className="add-new-people__content">
                    <div className="add-new-people__content-title">
                        <div><i><strong>{t("AnotherPeople")}</strong></i></div>
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
                    <ul className="add-new-people__list">
                        {
                            showPeopleList && people?.filter((item) => item.username.toLowerCase().startsWith(filterContent.toLowerCase()))
                                .map((item) => (
                                <li key={item.id} className="person">
                                    <div>{item.username}</div>
                                    <FontAwesomeIcon
                                        icon={faUserPlus}
                                        title={t("SendInviteToCommunity")}
                                        onClick={async () => await createInviteHandlerAsync(item.username, item.id)}
                                    />
                                </li>
                            ))
                        }
                    </ul>
                    {showPeopleList &&
                        <div className="add-new-people__more">
                            {maxPeopleItems === defaultMaxPeopleItems
                                ? <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowMorePeople")}
                                    onClick={() => setMaxPeopleItems(-1)}
                                  >
                                    {t("More")}
                                  </button>
                                : <button
                                    className="btn btn-outline-secondary"
                                    title={t("ShowLessPeople")}
                                    onClick={() => setMaxPeopleItems(defaultMaxPeopleItems)}
                                   >
                                    {t("Less")}
                                   </button>
                            }
                        </div>
                    }
                </div>
            </div>
            <button className="btn btn-outline-success" onClick={handleAddNewPeopleVisibility}>{t("Close")}</button>
        </div>
    );
}

export default AddPeople;