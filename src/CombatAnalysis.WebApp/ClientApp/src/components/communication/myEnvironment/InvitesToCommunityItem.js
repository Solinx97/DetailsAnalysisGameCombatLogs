import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/Account.api';
import { useGetCommunityByIdQuery } from '../../../store/api/communication/community/Community.api';
import { useCreateCommunityUserAsyncMutation } from '../../../store/api/communication/community/CommunityUser.api';
import { useRemoveInviteAsyncMutation } from '../../../store/api/communication/community/InviteToCommunity.api';
import User from '../User';

const InvitesToCommunityItem = ({ user, inviteToCommunity }) => {
    const { t } = useTranslation("communication/myEnvironment/invitesToCommunityItem");

    const { data: community, isLoading: communityIsLoading } = useGetCommunityByIdQuery(inviteToCommunity?.communityId);
    const { data: inviteOwner, isLoading: targetUserIsLoading } = useGetUserByIdQuery(inviteToCommunity?.appUserId);
    const [createCommunityUserAsyn] = useCreateCommunityUserAsyncMutation();
    const [removeInviteAsync] = useRemoveInviteAsyncMutation();

    const [userInformation, setUserInformation] = useState(null);

    const acceptRequestAsync = async () => {
        const newCommunityUser = {
            id: " ",
            username: user?.username,
            communityId: community?.id,
            appUserId: user?.id
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
            <div className="request-to-connect__content">
                <User
                    me={user}
                    itIsMe={false}
                    targetUserId={inviteOwner.id}
                    setUserInformation={setUserInformation}
                    allowRemoveFriend={false}
                />
                <div>{t("SentInvite")}</div>
                <div className="community-name">{community.name}</div>
            </div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title={t("Accept")}
                    onClick={async () => await acceptRequestAsync()} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title={t("Reject")}
                    onClick={async () => await rejectRequestAsync()} /></div>
            </div>
            {userInformation}
        </div>
    );
}

export default InvitesToCommunityItem;