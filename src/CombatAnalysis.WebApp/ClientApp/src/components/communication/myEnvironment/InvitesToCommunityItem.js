import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useRemoveInviteAsyncMutation } from '../../../store/api/communication/community/InviteToCommunity.api';

const InvitesToCommunityItem = ({ customer, inviteToCommunity }) => {
    const { t } = useTranslation("communication/myEnvironment/invitesToCommunityItem");

    const { data: community, isLoading: communityIsLoading } = useGetCommunityByIdQuery(inviteToCommunity?.communityId);
    const { data: inviteOwner, isLoading: targetUserIsLoading } = useGetCustomerByIdQuery(inviteToCommunity?.customerId);
    const [createCommunityUserAsyn] = useCreateCommunityUserAsyncMutation();
    const [removeInviteAsync] = useRemoveInviteAsyncMutation();

    const acceptRequestAsync = async () => {
        const newCommunityUser = {
            id: " ",
            username: customer?.username,
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
            <div><strong>{inviteOwner.username}</strong> {t("SentInvite")} <strong>'{community.name}'</strong></div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title={t("Accept")}
                    onClick={async () => await acceptRequestAsync()} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title={t("Reject")}
                    onClick={async () => await rejectRequestAsync()} /></div>
            </div>
        </div>
    );
}

export default InvitesToCommunityItem;