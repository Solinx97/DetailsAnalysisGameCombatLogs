import { useInviteSearchByUserIdQuery } from '../../../store/api/InviteToCommunity.api';
import InvitesToCommunityItem from './InvitesToCommunityItem';

const InvitesToCommunity = ({ customer }) => {
    const { data: invitesToCommunity, isLoading } = useInviteSearchByUserIdQuery(customer?.id);

    if (isLoading) {
        return <></>
    }

    return (
        <div className="invite-to-community">
            <div>Invites to community</div>
            <ul>
                {
                    invitesToCommunity.map((item) => (
                        <li key={item.id}>
                            <InvitesToCommunityItem customer={customer} inviteToCommunity={item} />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default InvitesToCommunity;