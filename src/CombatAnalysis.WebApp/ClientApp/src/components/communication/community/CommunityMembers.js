import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useCreateInviteAsyncMutation } from '../../../store/api/InviteToCommunity.api';
import AddPeople from '../../AddPeople';
import CommunityMemberItem from './CommunityMemberItem';

const CommunityMembers = ({ community, customer }) => {
    const { t } = useTranslation("communication/community/communityMembers");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [communityUsersId, setCommunityUsersId] = useState([]);
    const [peopleIdToJoin, setPeopleToJoin] = useState([]);

    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { data: communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id);

    useEffect(() => {
        const idList = [];
        for (let i = 0; i < communityUsers?.length; i++) {
            idList.push(communityUsers[i].customerId);
        }

        setCommunityUsersId(idList);
    }, [communityUsers])

    const createInviteAsync = async () => {
        for (var i = 0; i < peopleIdToJoin.length; i++) {
            const newInviteToCommunity = {
                communityId: community.id,
                toCustomerId: peopleIdToJoin[i],
                when: new Date(),
                result: 0,
                ownerId: customer?.id
            }

            await createInviteAsyncMut(newInviteToCommunity);
        }

        setShowAddPeople(false);
    }

    if (isLoading) {
        return <></>;
    }

    return (<>
        {showAddPeople &&
            <div className="add-people-to-community">
                <AddPeople
                    customer={customer}
                    communityUsersId={communityUsersId}
                    peopleToJoin={peopleIdToJoin}
                    setPeopleToJoin={setPeopleToJoin}
                />
                <div className="item-result">
                    <input type="button" value={t("Invite")} className="btn btn-success" onClick={async () => await createInviteAsync()} />
                    <input type="button" value={t("Cancel")} className="btn btn-light" onClick={() => setShowAddPeople(false)} />
                </div>
            </div>
        }
        <div className="title">
            <div>{t("Members")}</div>
            <div className="tool">
                <FontAwesomeIcon
                    icon={faPlus}
                    title={t("AddNewPeople")}
                    onClick={() => setShowAddPeople(true)}
                />
            </div>
        </div>
        <ul>
            {
                communityUsers?.map((item) => (
                    <li key={item.id }>
                        <CommunityMemberItem
                            community={community}
                            customerId={item.customerId}
                        />
                    </li>
                ))
            }
        </ul>
    </>);
}

export default CommunityMembers;