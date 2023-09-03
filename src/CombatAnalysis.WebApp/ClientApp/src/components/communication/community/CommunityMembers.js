import { faPlus, faRectangleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useRemoveCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useCreateInviteAsyncMutation, useLazyInviteIsExistQuery } from '../../../store/api/communication/community/InviteToCommunity.api';
import AddPeople from '../../AddPeople';
import Members from '../Members';
import CommunityMemberItem from './CommunityMemberItem';

const CommunityMembers = ({ community, customer, handleShowAddPeople, showAddPeople }) => {
    const { t } = useTranslation("communication/community/communityMembers");

    const [showRemovePeople, setShowRemovePeople] = useState(false);
    const [communityUsersId, setCommunityUsersId] = useState([]);
    const [peopleToJoin, setPeopleToJoin] = useState([]);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { data: communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id);
    const [isInviteExistAsync] = useLazyInviteIsExistQuery();
    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();

    useEffect(() => {
        const idList = [];
        for (let i = 0; i < communityUsers?.length; i++) {
            idList.push(communityUsers[i].customerId);
        }

        setCommunityUsersId(idList);
    }, [communityUsers])

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
                toCustomerId: peopleToJoin[i].id,
                when: new Date(),
                customerId: customer?.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        handleShowAddPeople();
    }

    const removeUsersAsync = async (peopleToRemove) => {
        for (let i = 0; i < peopleToRemove.length; i++) {
            await removeCommunityUserAsync(peopleToRemove[i].id);
        }

        setShowRemovePeople(false);
    }

    const clearListOfInvites = () => {
        setPeopleToJoin([]);
        handleShowAddPeople();
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <span className="members">
            <div className="members__title">
                <div className="actions">
                    <div>{t("Members")}</div>
                    <div className="tool">
                        {community.customerId === customer?.id &&
                            <FontAwesomeIcon
                                icon={faRectangleXmark}
                                title={t("RemovePeople")}
                                onClick={() => setShowRemovePeople((item) => !item)}
                            />
                        }
                        <FontAwesomeIcon
                            icon={faPlus}
                            title={t("AddNewPeople")}
                            onClick={clearListOfInvites}
                        />
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
            {showAddPeople &&
                <div className="add-people-to-community">
                    <AddPeople
                        customer={customer}
                        communityUsersId={communityUsersId}
                        peopleToJoin={peopleToJoin}
                        setPeopleToJoin={setPeopleToJoin}
                    />
                    <div className="item-result">
                        <input type="button" value={t("Invite")} className="btn btn-success" onClick={async () => await createInviteAsync()} />
                        <input type="button" value={t("Cancel")} className="btn btn-light" onClick={clearListOfInvites} />
                    </div>
                </div>
            }
            {showRemovePeople &&
                <Members
                    me={customer}
                    users={communityUsers}
                    communityItem={community}
                    removeUsersAsync={removeUsersAsync}
                    setShowMembers={setShowRemovePeople}
                    isPopup={true}
                />
            }
        </span>
    );
}

export default CommunityMembers;