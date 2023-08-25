import { useSearchByUserIdAsyncQuery } from '../../../store/api/CommunityUser.api';
import CommunityItem from "../community/CommunityItem";

const SelectedUserCommunities = ({ customer }) => {
    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    if (isLoading) {
        return <></>;
    }

    return (
        <ul>
            {
                userCommunities?.map((item) => (
                    <li key={item.id} className="community">
                        <CommunityItem
                            community={item}
                            id={item.communityId}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default SelectedUserCommunities;