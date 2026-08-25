import { APP_CONFIG } from '@/config/appConfig';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import CommunityItem from '@/features/community/components/CommunityItem';
import { useEffect, useRef, useState } from 'react';
import { useGetCommunityByUserIdQuery } from '../../../community/api/Community.api';
import type { AppUserModel } from '../../types/AppUserModel';

interface SelectedUserCommunitiesProps {
    user: AppUserModel;
    myself: AppUserModel;
    t: (key: string) => string;
}

const SelectedUserCommunities: React.FC<SelectedUserCommunitiesProps> = ({ user, myself, t }) => {
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communitySize ?? 10);

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const { data: userCommunities, isLoading, isFetching } = useGetCommunityByUserIdQuery({ appUserId: user.id, page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!userCommunities) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < userCommunities.count);
    }, [page, userCommunities]);

    if (!userCommunities || isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="communities__list">
            {userCommunities.communities.length === 0
                ? <div>{t("Empty")}</div>
                : <ul>
                    {userCommunities.communities.map((community) => (
                        <li key={community.id} className="community">
                            <CommunityItem
                                community={community}
                                targetUser={user}
                                myself={myself}
                            />
                        </li>
                    ))}
                    <li className="posts__item">
                        <InfiniteScrollTrigger
                            onLoadMore={() => setPage(p => p + 1)}
                            hasMore={hasMore}
                            isLoading={isFetching}
                        />
                    </li>
                </ul>
            }
        </div>
    );
}

export default SelectedUserCommunities;