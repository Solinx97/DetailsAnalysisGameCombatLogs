import { faGear, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useCreateInviteAsyncMutation } from '../../../store/api/InviteToCommunity.api';
import AddPeople from '../../AddPeople';
import CommunityMemberItem from './CommunityMemberItem';

import '../../../styles/communication/selectedCommunity.scss';

const CommunityMembers = ({ community, customer }) => {
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [communityUsersId, setCommunityUsersId] = useState([]);

    const { data: communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id);
    const [createInviteAsyncMut] = useCreateInviteAsyncMutation();

    const createInviteAsync = async (userId) => {
        const newInviteToCommunity = {
            communityId: community.id,
            toCustomerId: userId,
            when: new Date(),
            result: 0,
            ownerId: customer?.id
        }

        const res = await createInviteAsyncMut(newInviteToCommunity);
        if (res.data !== undefined) {
            setShowAddPeople(false);
        }
    }

    useEffect(() => {
        const idList = [];
        for (var i = 0; i < communityUsers?.length; i++) {
            idList.push(communityUsers[i].id);
        }

        setCommunityUsersId(idList);
    }, [])

    if (isLoading) {
        return <>Loading...</>;
    }

    return (<>
        {showAddPeople
            ? <AddPeople
                customer={customer}
                communityUsersId={communityUsersId}
                createInviteAsync={createInviteAsync}
                setShowAddPeople={setShowAddPeople}
            />
            : null
        }
        <div className="title">
            <div>Members</div>
            <div className="tool">
                <FontAwesomeIcon
                    icon={faGear}
                    title="Settings"
                />
                <FontAwesomeIcon
                    icon={faPlus}
                    title="Add a new people"
                    onClick={() => setShowAddPeople(true)}
                />
            </div>
        </div>
        <ul>
            {
                communityUsers?.map((item) => (
                    <li key={item.id }>
                        <CommunityMemberItem community={community} customerId={item.customerId} />
                    </li>
                ))
            }
        </ul>
    </>);
}

export default CommunityMembers;