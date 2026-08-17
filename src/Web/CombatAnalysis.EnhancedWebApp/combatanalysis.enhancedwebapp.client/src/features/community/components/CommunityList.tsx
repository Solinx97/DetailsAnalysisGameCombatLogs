import { APP_CONFIG } from '@/config/appConfig';
import type { RootState } from '@/app/Store';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { useGetCommunitiesQuery } from '../api/Community.api';
import CommunityItem from './CommunityItem';

const CommunityList: React.FC = () => {
    const myself = useSelector((state: RootState) => state.user.value);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communitySize ?? 10);

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const { data: communities, isLoading, isFetching } = useGetCommunitiesQuery({ page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!communities) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < communities.count);
    }, [page, communities]);

    if (!communities || !myself || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <ul>
                {communities.communities.filter(community => community.policyType === 0).map((item) => (
                    <li key={item.id} className="community">
                        <CommunityItem
                            community={item}
                            targetUser={myself}
                            myself={myself}
                        />
                    </li>
                ))
                }
                <li className="community">
                    <InfiniteScrollTrigger
                        onLoadMore={() => setPage(p => p + 1)}
                        hasMore={hasMore}
                        isLoading={isFetching}
                    />
                </li>
            </ul>
        </>
    );
}

export default CommunityList;