import Loading from '@/shared/components/Loading';
import { memo, useEffect, useRef, useState } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
import useFetchUserPosts from '../hooks/useFetchUserPosts';
import CommunityPost from './post/CommunityPost';
import UserPost from './post/UserPost';
import { APP_CONFIG } from '@/config/appConfig';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';

interface FeedParticipantsProps {
    myself: AppUserModel;
}

const FeedParticipants: React.FC<FeedParticipantsProps> = ({ myself }) => {
    const [page, setPage] = useState(1);;
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.userPostSize ?? 5);

    const { posts, isLoading, isFetching, count } = useFetchUserPosts(page, pageSizeRef.current, myself.id);

    useEffect(() => {
        if (!posts) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < count);
    }, [page, posts]);

    if (!posts || isLoading) {
        return (<Loading />);
    }

    return (
        <>
            <ul className="posts">
                {!posts
                    ? <Loading />
                    : posts.map(post => (
                        post.communityId
                            ? <li key={`${post.id} c`}>
                                <CommunityPost
                                    userId={myself.id}
                                    communityId={post.communityId ?? 0}
                                    post={post}
                                />
                            </li>
                            : <li key={`${post.id} u`}>
                                <UserPost
                                    myself={myself}
                                    post={post}
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
        </>
    );
}

export default memo(FeedParticipants);