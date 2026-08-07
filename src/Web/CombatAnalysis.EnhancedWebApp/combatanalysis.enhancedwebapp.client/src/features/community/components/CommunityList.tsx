import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { useGetCommunitiesCountQuery, useLazyGetCommunitiesQuery, useLazyGetMoreCommunitiesWithPaginationQuery } from '../api/Community.api';
import { useCommunityUserFindByUserIdQuery } from '../api/CommunityUser.api';
import type { CommunityModel } from '../types/CommunityModel';
import CommunityItem from './CommunityItem';

const CommunityList: React.FC<{ filterContent: string }> = ({ filterContent }) => {
    const user = useSelector((state: RootState) => state.user.value);

    const pageSizeRef = useRef<number>(5);

    const [communities, setCommunities] = useState<CommunityModel[]>([]);
    const [actualCount, setActualCount] = useState(0);

    const { data: count, isLoading: countIsLoading } = useGetCommunitiesCountQuery();
    const { data: userCommunities, isLoading } = useCommunityUserFindByUserIdQuery(user?.id ?? "");

    const [getCommunities] = useLazyGetCommunitiesQuery();
    const [getMoreCommunities] = useLazyGetMoreCommunitiesWithPaginationQuery();

    useEffect(() => {
        if (pageSizeRef.current !== null) {
            pageSizeRef.current = APP_CONFIG.communication.communityPageSize || 5;
        }

        const getCommunitiesAsync = async () => {
            try {
                const communities = await getCommunities({ page: 1, pageSize: pageSizeRef.current }).unwrap();

                setCommunities(communities);
            } catch (e) {
                logger.error("Failed to receive communities", e);
            }
        }

        getCommunitiesAsync();
    }, []);

    useEffect(() => {
        setActualCount(count === undefined ? 0 : count);
    }, [count]);

    const anotherCommunity = (community: CommunityModel) => {
        if (user == null) {
            return true;
        }

        return userCommunities?.filter(userCommunity => userCommunity.communityId === community.id).length === 0
            || userCommunities?.length === 0;
    }

    const getMoreCommunitiesAsync = async () => {
        const arg = {
            offset: +communities.length,
            pageSize: +pageSizeRef.current 
        };

        const response = await getMoreCommunities(arg).unwrap();
        setCommunities(prevCom => [...prevCom, ...response]);
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
                                myself={user}
                            />
                        </li>
                    ))
                }
            </ul>
            {communities?.length < actualCount &&
                <div className="load-more" onClick={getMoreCommunitiesAsync}>
                    <div className="load-more__content">Load more</div>
                </div>
            }
        </>
    );
}

export default CommunityList;