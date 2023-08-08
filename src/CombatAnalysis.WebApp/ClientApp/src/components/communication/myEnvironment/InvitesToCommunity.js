import { useTranslation } from 'react-i18next';
import { useInviteSearchByUserIdQuery } from '../../../store/api/InviteToCommunity.api';
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
                    invitesToCommunity?.map((item) => (
                        <li key={item.id}>
                            <InvitesToCommunityItem
                                customer={customer}
                                inviteToCommunity={item}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default InvitesToCommunity;