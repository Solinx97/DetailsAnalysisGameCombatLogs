import logger from '@/utils/Logger';
import { faCircleCheck, faCircleXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type JSX, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import User from '../../../user/components/User';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useGetCommunityByIdQuery } from '../../api/Community.api';
import { useAcceptCommunityInviteMutation, useRemoveCommunityInviteMutation } from '../../api/InviteToCommunity.api';
import type { InviteToCommunityModel } from '../../types/InviteToCommunityModel';

interface InvitesToCommunityItemProps {
    user: AppUserModel;
    inviteToCommunity: InviteToCommunityModel;
}

const InvitesToCommunityItem: React.FC<InvitesToCommunityItemProps> = ({ user, inviteToCommunity }) => {
    const { t } = useTranslation('communication/myEnvironment/invitesToCommunityItem');

    const { data: community, isLoading: communityIsLoading } = useGetCommunityByIdQuery(inviteToCommunity.communityId);
    const { data: inviteOwner, isLoading: targetUserIsLoading } = useGetUserByIdQuery(inviteToCommunity.appUserId);
    const [acceptInviteAsync] = useAcceptCommunityInviteMutation();
    const [removeInviteAsync] = useRemoveCommunityInviteMutation();

    const [userInformation, setUserInformation] = useState<JSX.Element | null>(null);

    const acceptRequestAsync = async () => {
        try {
            if (!community) {
                return;
            }

            await acceptInviteAsync({ id: inviteToCommunity.id, communityId: community.id, appUserId: user.id }).unwrap();
        } catch (e) {
            logger.error(`Failed to send accept request to community: ${community?.id}`, e);
        }
    }

    const rejectRequestAsync = async () => {
        try {
            if (!community) {
                return;
            }

            await removeInviteAsync({ id: inviteToCommunity.id, communityId: community.id }).unwrap();
        } catch (e) {
            logger.error(`Failed to send reject request to community: ${community?.id}`, e);
        }
    }

    if (communityIsLoading || targetUserIsLoading || !inviteOwner || !community) {
        return (<></>);
    }

    return (
        <div className="request-to-connect">
            <div className="request-to-connect__content">
                <User
                    targetUserId={inviteOwner.id}
                    setUserInformation={setUserInformation}
                />
                <div>{t("SentInvite")}</div>
                <div className="community-name">{community?.name}</div>
            </div>
            <div className="request-to-connect__answer">
                <div className="accept"><FontAwesomeIcon icon={faCircleCheck} title={t("Accept") || ""}
                    onClick={acceptRequestAsync} /></div>
                <div className="reject"><FontAwesomeIcon icon={faCircleXmark} title={t("Reject") || ""}
                    onClick={rejectRequestAsync} /></div>
            </div>
            {userInformation}
        </div>
    );
}

export default InvitesToCommunityItem;