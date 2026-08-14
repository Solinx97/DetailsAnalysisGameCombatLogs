import AddPeople from '@/shared/components/AddPeople';
import logger from '@/utils/Logger';
import { faCircleXmark, faPlus, faRectangleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useGetUsersByCommunityIdQuery, useLazyGetUsersByCommunityIdQuery, useRemoveCommunityUserMutation } from '../../api/CommunityUser.api';
import { useCreateInviteAsyncMutation } from '../../api/InviteToCommunity.api';
import type { CommunityModel } from '../../types/CommunityModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';
import type { InviteToCommunityModel } from '../../types/InviteToCommunityModel';
import CommunityMemberItem from './CommunityMemberItem';
import CommunityUsers from './CommunityUsers';

const defaultMaxPeople = 5;

interface CommunityMembersProps {
    community: CommunityModel;
    myself: AppUserModel;
    setIsCommunityMember?(value: SetStateAction<boolean>): void;
}

const CommunityMembers: React.FC<CommunityMembersProps> = ({ community, myself, setIsCommunityMember }) => {
    const { t } = useTranslation("communication/community/communityMembers");

    const communityUsersId: string[] = [];

    const [showAllPeople, setShowAllPeople] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState<AppUserModel[]>([]);
    const [allCommunityUsers, setAllCommunityUsers] = useState<CommunityUserModel[]>([]);
    const [showAddPeople, setShowAddPeople] = useState(false);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { communityUsers, isLoading } = useGetUsersByCommunityIdQuery(community.id, {
        selectFromResult: ({ data, isLoading }) => {
            if (!data) {
                return {
                    communityUsers: [],
                    isLoading
                }
            }

            for (let i = 0; i < data.length; i++) {
                communityUsersId.push(data[i].appUserId);
            }

            return {
                communityUsers: data?.slice(0, defaultMaxPeople),
                isLoading
            }
        }
    });
    const [getAllCommunityUsersAsync] = useLazyGetUsersByCommunityIdQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserMutation();

    useEffect(() => {
        if (!setIsCommunityMember || communityUsersId.length === 0) {
            return;
        }

        setIsCommunityMember(communityUsersId.includes(myself.id));
    }, [communityUsersId]);

    const createInviteAsync = async () => {
        try {
            for (let i = 0; i < peopleToJoin.length; i++) {
                const newInviteToCommunity: InviteToCommunityModel = {
                    id: 0,
                    communityId: community.id,
                    toAppUserId: peopleToJoin[i].id,
                    when: new Date(),
                    appUserId: myself.id
                }

                await createInviteAsyncMut(newInviteToCommunity).unwrap();
            }

            handleShowAddPeople();
        } catch (error) {
            logger.error("Failed create invite to community", error);
        }
    }

    const removeUsersAsync = async (communityUsersToRemove: CommunityUserModel[]) => {
        try {
            for (let i = 0; i < communityUsersToRemove.length; i++) {
                await removeCommunityUserAsync({ id: communityUsersToRemove[i].id, communityId: community.id }).unwrap();
            }

            setShowAllPeople(false);
        } catch (error) {
            logger.error("Failed remove invite to community", error);
        }
    }

    const clearListOfInvites = () => {
        setPeopleToJoin([]);
        handleShowAddPeople();
    }

    const handleShowAddPeople = () => {
        setShowAddPeople((item) => !item);
    }

    const handleShowAllPeopleAsync = async () => {
        try {
            const communityUsers = await getAllCommunityUsersAsync(community?.id).unwrap();
            setAllCommunityUsers(communityUsers);
            setShowAllPeople(prev => !prev);
        } catch (error) {
            logger.error('API call failed:', error);
        }
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="members">
            <div className="members__title">
                <div className="actions">
                    <div>{t("Members")}</div>
                    <div className="tool">
                        {community.appUserId === myself?.id &&
                            <FontAwesomeIcon
                                icon={faRectangleXmark}
                                title={t("RemovePeople") || ""}
                                onClick={handleShowAllPeopleAsync}
                            />
                        }
                        {communityUsersId.includes(myself?.id) &&
                            <FontAwesomeIcon
                                icon={faPlus}
                                title={t("AddNewPeople") || ""}
                                onClick={clearListOfInvites}
                            />
                        }
                    </div>
                </div>
            </div>
            <ul className="members__content">
                {communityUsers?.map((user: CommunityUserModel) => (
                    <li key={user.id}>
                        <CommunityMemberItem
                            comunityUser={user}
                        />
                    </li>
                ))
                }
            </ul>
            {communityUsers?.length >= defaultMaxPeople &&
                <input type="button" value={t("AllMembers") || ""} className="btn btn-outline-success all-people" onClick={handleShowAllPeopleAsync} />
            }
            {showAddPeople &&
                <div className="add-people-to-community box-shadow">
                    <div className="add-people-to-community__menu">
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            title={t("Close") || ""}
                            onClick={clearListOfInvites}
                        />
                    </div>
                    <AddPeople
                        usersId={communityUsersId}
                        peopleToJoin={peopleToJoin}
                        setPeopleToJoin={setPeopleToJoin}
                    />
                    <div className="item-result">
                        <div className="btn-shadow invite" onClick={createInviteAsync}>{t("Invite")}</div>
                        <div className="btn-shadow" onClick={clearListOfInvites}>{t("Cancel")}</div>
                    </div>
                </div>
            }
            {showAllPeople &&
                <CommunityUsers
                    myself={myself}
                    communityUsers={allCommunityUsers}
                    removeUsersAsync={removeUsersAsync}
                    setShowMembers={setShowAllPeople}
                    isPopup={true}
                    canRemovePeople={() => myself?.id === community?.appUserId}
                />
            }
        </div>
    );
}

export default memo(CommunityMembers);