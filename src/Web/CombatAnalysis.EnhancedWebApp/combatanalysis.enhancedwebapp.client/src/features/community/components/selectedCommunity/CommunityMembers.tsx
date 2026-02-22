import AddPeople from '@/shared/components/AddPeople';
import logger from '@/utils/Logger';
import { faCircleXmark, faPlus, faRectangleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useCommunityUserFindByCommunityIdQuery, useLazyCommunityUserFindByCommunityIdQuery, useRemoveCommunityUserMutation } from '../../api/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../api/InviteToCommunity.api';
import type { CommunityModel } from '../../types/CommunityModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';
import type { InviteToCommunityModel } from '../../types/InviteToCommunityModel';
import CommunityMemberItem from './CommunityMemberItem';
import CommunityUsers from './CommunityUsers';

const defaultMaxPeople = 5;

interface CommunityMembersProps {
    community: CommunityModel;
    myself: AppUserModel;
    setIsCommunityMember(value: SetStateAction<boolean>): void;
}

const CommunityMembers: React.FC<CommunityMembersProps> = ({ community, myself, setIsCommunityMember }) => {
    const { t } = useTranslation("communication/community/communityMembers");

    const communityUsersId: string[] = [];

    const [showAllPeople, setShowAllPeople] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState<AppUserModel[]>([]);
    const [allCommunityUsers, setAllCommunityUsers] = useState<CommunityUserModel[]>([]);
    const [showAddPeople, setShowAddPeople] = useState(false);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { communityUsers, isLoading } = useCommunityUserFindByCommunityIdQuery(community.id, {
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
    const [getAllCommunityUsersAsync] = useLazyCommunityUserFindByCommunityIdQuery();
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserMutation();

    useEffect(() => {
        if (communityUsersId.length === 0) {
            return;
        }

        setIsCommunityMember(communityUsersId.includes(myself.id));
    }, [communityUsersId]);

    const checkIfRequestExistAsync = async (appUserId: string, communityId: number): Promise<boolean> => {
        try {
            const isExist = await isInviteExistAsync({ appUserId, communityId }).unwrap();

            return isExist;
        } catch (e) {
            logger.error("Failed to check if request to the community already exist", e);

            return true;
        }
    }

    const createInviteAsync = async () => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const isExist = await checkIfRequestExistAsync(peopleToJoin[i].id, community.id);
            if (isExist) {
                continue;
            }

            const newInviteToCommunity: InviteToCommunityModel = {
                id: 0,
                communityId: community.id,
                toAppUserId: peopleToJoin[i].id,
                when: new Date(),
                appUserId: myself.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        handleShowAddPeople();
    }

    const removeUsersAsync = async (communityUsersToRemove: CommunityUserModel[]) => {
        for (let i = 0; i < communityUsersToRemove.length; i++) {
            await removeCommunityUserAsync(communityUsersToRemove[i].id);
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
        try {
            const communityUsers = await getAllCommunityUsersAsync(community?.id).unwrap();
            setAllCommunityUsers(communityUsers);
            setShowAllPeople(prev => !prev);
        } catch (e) {
            console.error('API call failed:', e);
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
        </span>
    );
}

export default memo(CommunityMembers);