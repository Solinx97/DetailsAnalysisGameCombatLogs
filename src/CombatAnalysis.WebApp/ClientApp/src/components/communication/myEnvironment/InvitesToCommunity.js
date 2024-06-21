import { useTranslation } from 'react-i18next';
import { useInviteSearchByUserIdQuery } from '../../../store/api/communication/community/InviteToCommunity.api';
import InvitesToCommunityItem from './InvitesToCommunityItem';

const InvitesToCommunity = ({ customer }) => {
    const { t } = useTranslation("communication/myEnvironment/invitesToCommunity");

    const { data: invitesToCommunity, isLoading } = useInviteSearchByUserIdQuery(customer?.id);

    if (isLoading || invitesToCommunity?.length === 0) {
        return <></>;
    }

    return (
        <div className="invite-to-community">
            <div>{t("InvitesToCommunity")}</div>
            <ul>
                {
                    invitesToCommunity?.map((invite) => (
                        <li key={invite.id}>
                            <InvitesToCommunityItem
                                customer={customer}
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