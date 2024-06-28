import { useSearchByUserIdAsyncQuery } from '../../../store/api/communication/community/CommunityUser.api';
import CommunityItem from "../community/CommunityItem";

const SelectedUserCommunities = ({ customer }) => {
    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="communities__list">
            <ul>
                {userCommunities?.map((item) => (
                    <li key={item.id} className="community">
                        <CommunityItem
                            community={item}
                            id={item.communityId}
                        />
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default SelectedUserCommunities;