import { faGear, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import CommunityMemberItem from './CommunityMemberItem';

import '../../../styles/communication/selectedCommunity.scss';

const CommunityMembers = ({ community }) => {
    const { data: communityUsers } = useSearchByCommunityIdAsyncQuery(community?.id);

    return (<>
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