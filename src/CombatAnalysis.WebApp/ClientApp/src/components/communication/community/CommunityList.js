import { useSelector } from 'react-redux';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/communication/community/CommunityUser.api';
import CommunityItem from './CommunityItem';

const CommunityList = ({ filterContent, communities }) => {
    const user = useSelector((state) => state.user.value);

    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(user?.id);

    const anotherCommunity = (community) => {
        if (user == null) {
            return true;
        }

        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <ul>
            {communities?.filter(community => community.policyType === 0).map((item) => (
                    (anotherCommunity(item) && item.name.toLowerCase().startsWith(filterContent.toLowerCase())) &&
                    <li key={item.id} className="community">
                        <CommunityItem
                            id={item.id}
                            me={user}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default CommunityList;