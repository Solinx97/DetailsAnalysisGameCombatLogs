import { faPlus, faRectangleXmark, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useLazySearchByCommunityIdAsyncQuery, useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useRemoveCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/communication/community/InviteToCommunity.api';
import AddPeople from '../../AddPeople';
import Members from '../Members';
import CommunityMemberItem from './CommunityMemberItem';

const defaultMaxPeople = 5;

const CommunityMembers = ({ community, user, setIsCommunityMember }) => {
    const { t } = useTranslation("communication/community/communityMembers");

    let communityUsersId = [];

    const [showAllPeople, setShowAllPeople] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState([]);
    const [allCommunityUsers, setAllCommunityUsers] = useState([]);
    const [showAddPeople, setShowAddPeople] = useState(false);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id, {
        selectFromResult: ({ data }) => {
            const idList = [];
            for (let i = 0; i < data?.length; i++) {
                idList.push(data[i].appUserId);
            }

            communityUsersId = idList;

            return {
                communityUsers: data?.slice(0, defaultMaxPeople)
            };
        }
    });
    const [getAllCommunityUsersAsync] = useLazySearchByCommunityIdAsyncQuery();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();

    useEffect(() => {
        if (communityUsersId.length === 0) {
            return;
        }

        setIsCommunityMember(communityUsersId.includes(user?.id));
    }, [communityUsersId])

    const checkIfRequestExistAsync = async (peopleId, communityId) => {
        const arg = {
            peopleId: peopleId,
            communityId: communityId
        };

        const isExist = await isInviteExistAsync(arg);
        if (isExist.error !== undefined) {
            return true;
        }

        return isExist.data;
    }

    const createInviteAsync = async () => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const isExist = await checkIfRequestExistAsync(peopleToJoin[i].id, community.id);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity = {
                communityId: community.id,
                toAppUserId: peopleToJoin[i].id,
                when: new Date(),
                appUserId: user?.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        handleShowAddPeople();
    }

    const removeUsersAsync = async (peopleToRemove) => {
        for (let i = 0; i < peopleToRemove.length; i++) {
            await removeCommunityUserAsync(peopleToRemove[i].id);
        }

        setShowAllPeople(false);
    }

    const clearListOfInvites = () => {
        setPeopleToJoin([]);
        handleShowAddPeople();
    }

    const handleShowAddPeople = () => {
        setShowAddPeople((item) => !item);
    }

    const handleShowAllPeopleAsync = async () => {
        const users = await getAllCommunityUsersAsync(community?.id);
        if (users.data !== undefined) {
            setAllCommunityUsers(users.data);
            setShowAllPeople((item) => !item);
        }
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <span className="members">
            <div className="members__title">
                <div className="actions">
                    <div>{t("Members")}</div>
                    <div className="tool">
                        {community.appUserId === user?.id &&
                            <FontAwesomeIcon
                                icon={faRectangleXmark}
                                title={t("RemovePeople")}
                                onClick={handleShowAllPeopleAsync}
                            />
                        }
                        {communityUsersId.includes(user?.id) &&
                            <FontAwesomeIcon
                                icon={faPlus}
                                title={t("AddNewPeople")}
                                onClick={clearListOfInvites}
                            />
                        }
                    </div>
                </div>
            </div>
            <ul className="members__content">
                {communityUsers?.map((item) => (
                        <li key={item.id }>
                            <CommunityMemberItem
                                comunityUser={item}
                            />
                        </li>
                    ))
                }
            </ul>
            {communityUsers?.length >= defaultMaxPeople &&
                <input type="button" value={t("AllMembers")} className="btn btn-outline-success all-people" onClick={handleShowAllPeopleAsync} />
            }
            {showAddPeople &&
                <div className="add-people-to-community box-shadow">
                    <div className="add-people-to-community__menu"> 
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            title={t("Close")}
                            onClick={clearListOfInvites}
                        />
                    </div>
                    <AddPeople
                        user={user}
                        communityUsersId={communityUsersId}
                        peopleToJoin={peopleToJoin}
                        setPeopleToJoin={setPeopleToJoin}
                    />
                    <div className="item-result">
                        <div className="btn-shadow invite" onClick={async () => await createInviteAsync()}>{t("Invite")}</div>
                        <div className="btn-shadow" onClick={clearListOfInvites}>{t("Cancel")}</div>
                    </div>
                </div>
            }
            {showAllPeople &&
                <Members
                    me={user}
                    users={allCommunityUsers}
                    communityItem={community}
                    removeUsersAsync={removeUsersAsync}
                    setShowMembers={setShowAllPeople}
                    isPopup={true}
                    canRemovePeople={user?.id === community?.appUserId}
                />
            }
        </span>
    );
}

export default memo(CommunityMembers);