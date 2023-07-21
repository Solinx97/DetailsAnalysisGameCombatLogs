import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useGetCommunityByIdQuery } from '../../../store/api/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/CommunityUser.api';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemoveInviteAsyncMutation } from '../../../store/api/InviteToCommunity.api';

const InvitesToCommunityItem = ({ customer, inviteToCommunity }) => {
    const { data: community, isLoading: communityIsLoading } = useGetCommunityByIdQuery(inviteToCommunity?.communityId);
    const { data: inviteOwner, isLoading: targetUserIsLoading } = useGetCustomerByIdQuery(inviteToCommunity?.ownerId);
    const [createCommunityUserAsyn] = useCreateCommunityUserAsyncMutation();
    const [removeInviteAsync] = useRemoveInviteAsyncMutation();

    const acceptRequestAsync = async () => {
        const newCommunityUser = {
            communityId: community?.id,
            customerId: customer?.id
        };

        const createdItem = await createCommunityUserAsyn(newCommunityUser);
        if (createdItem.error !== undefined) {
            return;
        }

        await removeInviteAsync(inviteToCommunity.id);
    }

    const rejectRequestAsync = async () => {
        await removeInviteAsync(inviteToCommunity.id);
    }

    if (communityIsLoading || targetUserIsLoading) {
        return <></>;
    }

    return (
        <div className="request-to-connect">
            <div><strong>{inviteOwner.username}</strong> sent you invite to <strong>'{community.name}'</strong></div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title="Accept"
                    onClick={async () => await acceptRequestAsync()} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title="Reject"
                    onClick={async () => await rejectRequestAsync()} /></div>
            </div>
        </div>
    );
}

export default InvitesToCommunityItem;