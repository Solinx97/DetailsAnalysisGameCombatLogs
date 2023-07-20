import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect } from 'react';
import { useGetCommunityByIdQuery } from '../../../store/api/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemoveInviteAsyncMutation } from '../../../store/api/InviteToCommunity.api';

const InvitesToCommunityItem = ({ customer, inviteToCommunity }) => {
    const { data: community } = useGetCommunityByIdQuery(inviteToCommunity?.communityId);
    const { data: inviteOwner, isLoading } = useGetCustomerByIdQuery(inviteToCommunity?.ownerId);
    const [createCommunityUserAsyn] = useCreateCommunityUserAsyncMutation();
    const [removeInviteAsync] = useRemoveInviteAsyncMutation();

    useEffect(() => {
        if (!isLoading) {
            inviteToCommunity.username = inviteOwner.username;
            inviteToCommunity.community = community.name;
        }
    }, [isLoading])

    const acceptRequestAsync = async (inviteToCommunity) => {
        const newCommunityUser = {
            communityId: inviteToCommunity.communityId,
            customerId: customer.id
        };

        let createdItem = await createCommunityUserAsyn(newCommunityUser);
        if (createdItem.data !== undefined) {
            return;
        }

        await removeInviteAsync(inviteToCommunity.id);
    }

    const rejectRequestAsync = async (inviteToCommunityId) => {
        await removeInviteAsync(inviteToCommunityId);
    }

    if (isLoading) {
        return <>Loading....</>
    }

    return (
        <div className="request-to-connect">
            <div>'{inviteToCommunity.username}' sent you invite to '{inviteToCommunity.community}'</div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title="Accept"
                    onClick={async () => await acceptRequestAsync(inviteToCommunity)} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Reject"
                    onClick={async () => await rejectRequestAsync(inviteToCommunity.id)} /></div>
            </div>
        </div>
    );
}

export default InvitesToCommunityItem;