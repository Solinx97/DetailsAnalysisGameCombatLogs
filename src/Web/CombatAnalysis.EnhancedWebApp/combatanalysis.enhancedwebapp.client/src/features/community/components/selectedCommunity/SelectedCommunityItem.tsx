import { APP_CONFIG } from '@/config/appConfig';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import Loading from '@/shared/components/Loading';
import useFetchCommunityPosts from '@/features/feed/hooks/useFetchCommunityPosts';
import { memo, useEffect, useRef, useState } from 'react';
import CommunityPost from '../../../feed/components/post/CommunityPost';

interface SelectedCommunityItemProps {
    myselfId: string;
    communityId: number;
}

const SelectedCommunityItem: React.FC<SelectedCommunityItemProps> = ({ myselfId, communityId }) => {
    const [page, setPage] = useState(1);;
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityPostPageSize ?? 5);

    const { communityPosts, communityPostIsLoading } = useFetchCommunityPosts(page, pageSizeRef.current, communityId);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < communityPosts.length);
    }, [page, communityPosts]);

    if (!communityPosts) {
        return (<Loading />);
    }

    return (
        <>
            <ul className="posts">
                {communityPosts.map((post) => (
                    <li key={post?.id}>
                        <CommunityPost
                            userId={myselfId}
                            communityId={communityId}
                            post={post}
                        />
                    </li>
                ))
                }
                <li className="posts__item">
                    <InfiniteScrollTrigger
                        onLoadMore={() => setPage(p => p + 1)}
                        hasMore={hasMore}
                        isLoading={communityPostIsLoading}
                    />
                </li>
            </ul>
        </>
    );
}

export default memo(SelectedCommunityItem);