import { useTranslation } from 'react-i18next';
import { useInviteSearchByUserIdQuery } from '../../../store/api/community/InviteToCommunity.api';
import { InviteToCommunity } from '../../../types/InviteToCommunity';
import { InvitesToCommunityProps } from '../../../types/components/communication/myEnvironment/InvitesToCommunityProps';
import InvitesToCommunityItem from './InvitesToCommunityItem';

const InvitesToCommunity: React.FC<InvitesToCommunityProps> = ({ user }) => {
    const { t } = useTranslation("communication/myEnvironment/invitesToCommunity");

    const { data: invitesToCommunity, isLoading } = useInviteSearchByUserIdQuery(user?.id);

    if (isLoading || invitesToCommunity?.length === 0) {
        return (<></>);
    }

    return (
        <div className="invite-to-community">
            <div>{t("InvitesToCommunity")}</div>
            <ul>
                {
                    invitesToCommunity?.map((invite: InviteToCommunity) => (
                        <li key={invite.id}>
                            <InvitesToCommunityItem
                                user={user}
                                inviteToCommunity={invite}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default InvitesToCommunity;