import { useSelector } from 'react-redux';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/communication/community/CommunityUser.api';
import CommunityItem from './CommunityItem';

const CommunityList = ({ filterContent, communities }) => {
    const customer = useSelector((state) => state.customer.value);

    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    if (isLoading) {
        return <></>;
    }

    const anotherCommunity = (community) => {
        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    return (
        <ul>
            {communities?.map((item) => (
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

export default CommunityList;