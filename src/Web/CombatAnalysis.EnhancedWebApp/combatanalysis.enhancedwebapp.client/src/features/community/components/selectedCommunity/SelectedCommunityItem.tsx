import { APP_CONFIG } from '@/config/appConfig';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import Loading from '@/shared/components/Loading';
import { memo, useEffect, useRef, useState } from 'react';
import CommunityPost from '../../../feed/components/post/CommunityPost';
import { useGetCommunityPostsByCommunityIdQuery } from '@/features/feed/api/Post.api';

interface SelectedCommunityItemProps {
    myselfId: string;
    communityId: number;
}

const SelectedCommunityItem: React.FC<SelectedCommunityItemProps> = ({ myselfId, communityId }) => {
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityPostPageSize ?? 10);

    const { data: posts, isLoading, isFetching } = useGetCommunityPostsByCommunityIdQuery({ communityId, page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!posts) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < posts.count);
    }, [page, posts]);

    if (!posts || isLoading) {
        return (<Loading />);
    }

    return (
        <>
            <ul className="posts">
                {!posts.posts
                    ? <Loading />
                    : posts.posts.map((post) => (
                        <li key={post?.id} className="posts__item">
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
                        isLoading={isFetching}
                    />
                </li>
            </ul>
        </>
    );
}

export default memo(SelectedCommunityItem);