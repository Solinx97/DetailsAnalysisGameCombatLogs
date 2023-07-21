import { faGear, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import AddPeople from '../../AddPeople';
import CommunityMemberItem from './CommunityMemberItem';

import '../../../styles/communication/selectedCommunity.scss';

const CommunityMembers = ({ community, customer }) => {
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [communityUsersId, setCommunityUsersId] = useState([]);

    const { data: communityUsers, isLoading } = useSearchByCommunityIdAsyncQuery(community?.id);

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
                community={community}
                communityUsersId={communityUsersId}
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