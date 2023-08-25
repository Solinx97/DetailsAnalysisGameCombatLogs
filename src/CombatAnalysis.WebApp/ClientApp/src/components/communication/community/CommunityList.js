import { useSelector } from 'react-redux';
import { useGetCommunitiesQuery } from '../../../store/api/ChatApi';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/CommunityUser.api';
import CommunityItem from './CommunityItem';

const CommunityList = ({ filterContent }) => {
    const customer = useSelector((state) => state.customer.value);

    const { data: communities, isLoading } = useGetCommunitiesQuery();
    const { data: userCommunities, isLoading: userCommunitiesIsLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    if (isLoading || userCommunitiesIsLoading) {
        return <></>;
    }

    const anotherCommunity = (community) => {
        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    const render = () => {
        return (
            <ul>
                {
                    communities?.map((item) => (
                        (anotherCommunity(item) && item.name.toLowerCase().startsWith(filterContent.toLowerCase())) &&
                        <li key={item.id} className="community">
                             <CommunityItem
                                id={item.id}
                            />
                        </li>
                    ))
                }
            </ul>
        );
    }

    return render();
}

export default CommunityList;