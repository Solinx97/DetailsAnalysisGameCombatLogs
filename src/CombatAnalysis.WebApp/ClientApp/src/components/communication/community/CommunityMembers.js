import { faGear, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useCreateInviteAsyncMutation } from '../../../store/api/InviteToCommunity.api';
import AddPeople from '../../AddPeople';
import CommunityMemberItem from './CommunityMemberItem';

import '../../../styles/communication/selectedCommunity.scss';

const CommunityMembers = ({ community, customer }) => {
    const { t } = useTranslation("communication/community/communityMembers");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [communityUsersId, setCommunityUsersId] = useState([]);
    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const { data: communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id);

    useEffect(() => {
        const idList = [];
        for (let i = 0; i < communityUsers?.length; i++) {
            idList.push(communityUsers[i].customerId);
        }

        setCommunityUsersId(idList);
    }, [communityUsers])

    const createInviteAsync = async (whomId) => {
        const newInviteToCommunity = {
            communityId: community.id,
            toCustomerId: whomId,
            when: new Date(),
            result: 0,
            ownerId: customer?.id
        }

        const createdInvite = await createInviteAsyncMut(newInviteToCommunity);
        return createdInvite.data ? createdInvite.data : null;
    }

    if (isLoading) {
        return <></>;
    }

    return (<>
        {showAddPeople &&
            <AddPeople
                customer={customer}
                createInviteAsync={createInviteAsync}
                communityUsersId={communityUsersId}
                setShowAddPeople={setShowAddPeople}
            />
        }
        <div className="title">
            <div>{t("Members")}</div>
            <div className="tool">
                <FontAwesomeIcon
                    icon={faGear}
                    title={t("Settings")}
                />
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