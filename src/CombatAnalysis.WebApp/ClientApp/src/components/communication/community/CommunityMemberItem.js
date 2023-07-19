import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const CommunityMemberItem = ({ community, customerId }) => {
    const { data: member } = useGetCustomerByIdQuery(customerId);

    return (
        <div className="member">
            {member?.id !== community.ownerId && customerId !== member?.id
                ? <FontAwesomeIcon icon={faTrash} title="Remove" />
                : null
            }
            <div className="member__username">{member?.username}</div>
        </div>
    );
}

export default CommunityMemberItem;