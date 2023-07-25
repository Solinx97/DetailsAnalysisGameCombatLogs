import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useRemoveCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';

const CommunityMemberItem = ({ community, customerId }) => {
    const [showRemovePeopleAlert, setShowRemovePeopleAlert] = useState(false);

    const [removeCommunityUserAsync] = useRemoveCommunityUserAsyncMutation();
    const { data: member, isLoading } = useGetCustomerByIdQuery(customerId);

    const removePeopleAsync = async () => {
        const deletedItemCount = await removeCommunityUserAsync(member.communityUserId);
        if (deletedItemCount.data !== undefined) {
            showRemovePeopleAlert(false);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            {showRemovePeopleAlert
                ? <div className="remove-people">
                    <div>Remove people</div>
                    <div>
                        <div>You sure, that you want to remove <strong>'{member.username}'</strong> from community <strong>'{community.name}'</strong>?</div>
                    </div>
                    <div>
                        <button className="btn btn-outline-warning" onClick={async () => await removePeopleAsync()}>Remove</button>
                        <button className="btn btn-outline-success" onClick={() => setShowRemovePeopleAlert((item) => !item)}>Cancel</button>
                    </div>
                </div>
                : null
            }
            <div className="member">
                {member?.id !== community.ownerId && customerId !== member?.id &&
                    <FontAwesomeIcon
                        icon={faTrash}
                        title="Remove"
                        onClick={() => setShowRemovePeopleAlert((item) => !item)}
                    />
                }
                <div className="member__username">{member?.username}</div>
            </div>
        </>
    );
}

export default CommunityMemberItem;