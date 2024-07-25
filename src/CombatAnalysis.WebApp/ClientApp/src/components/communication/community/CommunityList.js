import { useSelector } from 'react-redux';
import { useSearchByUserIdAsyncQuery } from '../../../store/api/communication/community/CommunityUser.api';
import CommunityItem from './CommunityItem';

const CommunityList = ({ filterContent, communities }) => {
    const customer = useSelector((state) => state.customer.value);

    const { data: userCommunities, isLoading } = useSearchByUserIdAsyncQuery(customer?.id);

    const anotherCommunity = (community) => {
        if (customer == null) {
            return true;
        }

        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <ul>
            {communities?.map((item) => (
                    (anotherCommunity(item) && item.name.toLowerCase().startsWith(filterContent.toLowerCase())) &&
                    <li key={item.id} className="community">
                        <CommunityItem
                            id={item.id}
                            me={customer}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default CommunityList;