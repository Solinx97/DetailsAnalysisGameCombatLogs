import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { useCommunityUserSearchByUserIdQuery } from '../../../store/api/community/CommunityUser.api';
import { useGetCommunitiesCountQuery, useLazyGetCommunitiesWithPaginationQuery, useLazyGetMoreCommunitiesWithPaginationQuery } from '../../../store/api/core/Community.api';
import CommunityItem from './CommunityItem';

const CommunityList = ({ filterContent }) => {
    const user = useSelector((state) => state.user.value);

    const pageSizeRef = useRef(1);

    const [communities, setCommunities] = useState([]);

    const { data: count, isLoading: countIsLoading } = useGetCommunitiesCountQuery();
    const { data: userCommunities, isLoading } = useCommunityUserSearchByUserIdQuery(user?.id);

    const [getCommunities] = useLazyGetCommunitiesWithPaginationQuery();
    const [getMoreCommunities] = useLazyGetMoreCommunitiesWithPaginationQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_COMMUNITY_PAGE_SIZE;

        const getCommunitiesAsync = async () => {
            const response = await getCommunities(pageSizeRef.current);

            if (!response.error) {
                setCommunities(response.data);
            }
        }

        getCommunitiesAsync();
    }, []);

    const anotherCommunity = (community) => {
        if (user == null) {
            return true;
        }

        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    const getMoreCommunitiesAsync = async () => {
        const arg = {
            offset: communities.length,
            pageSize: pageSizeRef.current 
        };

        const response = await getMoreCommunities(arg);
        if (!response.error) {
            setCommunities(prevCom => [...prevCom, ...response.data]);
        }
    }

    if (countIsLoading || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
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
            {communities.length < count &&
                <div className="load-more" onClick={getMoreCommunitiesAsync}>
                    <div className="load-more__content">Load more</div>
                </div>
            }
        </>
    );
}

export default CommunityList;