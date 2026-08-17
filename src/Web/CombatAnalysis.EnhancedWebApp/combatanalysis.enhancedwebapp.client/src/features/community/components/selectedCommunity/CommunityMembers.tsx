import { APP_CONFIG } from '@/config/appConfig';
import AddPeople from '@/shared/components/AddPeople';
import logger from '@/utils/Logger';
import { faCircleXmark, faPlus, faRectangleXmark, faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useGetShortListUsersByCommunityIdQuery, useRemoveCommunityUserMutation } from '../../api/CommunityUser.api';
import { useCreateInviteAsyncMutation } from '../../api/InviteToCommunity.api';
import type { CommunityModel } from '../../types/CommunityModel';
import type { CommunityUserModel } from '../../types/CommunityUserModel';
import type { InviteToCommunityModel } from '../../types/InviteToCommunityModel';
import CommunityMemberItem from './CommunityMemberItem';
import CommunityUsers from './CommunityUsers';

interface CommunityMembersProps {
    community: CommunityModel;
    myself: AppUserModel;
    isCommunityMember?: boolean;
    setIsCommunityMember?(value: SetStateAction<boolean>): void;
}

const CommunityMembers: React.FC<CommunityMembersProps> = ({ community, myself, isCommunityMember, setIsCommunityMember }) => {
    const defaultMaxPeople = 5;
    const communityUsersId: string[] = [];

    const { t } = useTranslation('communication/community/communityMembers');

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityUserSize ?? defaultMaxPeople);

    const [showAllPeople, setShowAllPeople] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState<AppUserModel[]>([]);
    const [showAddPeople, setShowAddPeople] = useState(false);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { data: communityUsers, isLoading } = useGetShortListUsersByCommunityIdQuery({ communityId: community.id, pageSize: pageSizeRef.current });
    const [removeCommunityUserAsync] = useRemoveCommunityUserMutation();

    useEffect(() => {
        if (!setIsCommunityMember || !communityUsers) {
            return;
        }

        setIsCommunityMember(communityUsers.users.find(x => x.appUserId === myself.id) !== null);
    }, [communityUsers]);

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

    if (!communityUsers || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="members">
            <div className="members__title">
                <div className="actions">
                    <div>{t("Members")}</div>
                    <div className="tool">
                        {community.appUserId === myself.id &&
                            <FontAwesomeIcon
                                icon={faRectangleXmark}
                                title={t("RemovePeople") || ""}
                                onClick={() => setShowAllPeople(prev => !prev)}
                            />
                        }
                        {isCommunityMember &&
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
                {communityUsers.users.map((user: CommunityUserModel) => (
                    <li key={user.id}>
                        <CommunityMemberItem
                            comunityUser={user}
                        />
                    </li>
                ))
                }
            </ul>
            {communityUsers.count >= defaultMaxPeople &&
                <div className="btn-shadow" onClick={() => setShowAllPeople(prev => !prev)}>
                    <FontAwesomeIcon
                        icon={faBars}
                    />
                    <div>{t("AllMembers")}</div>
                </div>
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
                    removeUsersAsync={removeUsersAsync}
                    setShowMembers={setShowAllPeople}
                    isPopup={true}
                    canRemovePeople={() => myself.id === community.appUserId}
                    communityId={community.id}
                />
            }
        </div>
    );
}

export default memo(CommunityMembers);