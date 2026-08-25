import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useInviteGetByUserIdQuery } from '../../api/InviteToCommunity.api';
import InvitesToCommunityItem from './InvitesToCommunityItem';

const InvitesToCommunity: React.FC<{ user: AppUserModel | null }> = ({ user }) => {
    const { t } = useTranslation('communication/myEnvironment/invitesToCommunity');

    const { data: invitesToCommunity, isLoading } = useInviteGetByUserIdQuery(user?.id ?? "");

    if (isLoading || !invitesToCommunity || !user || invitesToCommunity.length === 0) {
        return (<></>);
    }

    return (
        <div className="invite-to-community">
            <div>{t("InvitesToCommunity")}</div>
            <ul>
                {invitesToCommunity.map((invite) => (
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