import { APP_CONFIG } from '@/config/appConfig';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import Loading from '@/shared/components/Loading';
import { useGetCommunityPostsByCommunityIdQuery } from '@/features/feed/api/Post.api';
import { useCountCommunityNewPostsQuery } from '@/features/feed/api/CommunityPost.api';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import CommunityPost from '../../../feed/components/post/CommunityPost';

interface SelectedCommunityItemProps {
    myselfId: string;
    communityId: number;
    lastCheck: string;
    setLastCheck: (value: SetStateAction<string>) => void;
    feedVersion: number;
    setFeedVersion: (value: SetStateAction<number>) => void;
    t: (key: string) => string;
}

const SelectedCommunityItem: React.FC<SelectedCommunityItemProps> = ({ myselfId, communityId, lastCheck, setLastCheck, feedVersion, setFeedVersion, t }) => {
    const [page, setPage] = useState(1);
    const [countNew, setCountNew] = useState(0);

    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.communityPostPageSize ?? 10);
    const intervalCheckNewPostsRef = useRef<number>(APP_CONFIG.communication.intervalCheckNewPosts ?? 100000);

    const { data: countFeedNewPost } = useCountCommunityNewPostsQuery(
        { communityId, lastCheck },
        {
            pollingInterval: intervalCheckNewPostsRef.current
        }
    );
    const { data: posts, isLoading, isFetching } = useGetCommunityPostsByCommunityIdQuery({ communityId, appUserId: myselfId, page, pageSize: pageSizeRef.current, feedVersion });

    useEffect(() => {
        if (!posts) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < posts.count);
    }, [page, posts]);

    useEffect(() => {
        if (!countFeedNewPost) {
            return;
        }

        setCountNew(countFeedNewPost);
    }, [countFeedNewPost]);

    const refreshPosts = () => {
        window.scrollTo(0, 0);

        setCountNew(0);
        setLastCheck((new Date()).toISOString());
        setPage(1);
        setFeedVersion(prev => prev + 1);
    }

    if (!posts || isLoading) {
        return (<Loading />);
    }

    return (
        <>
            {countNew > 0 &&
                <div className="refresh-posts" onClick={refreshPosts}>
                    <div className="btn-shadow" title={t("ShowNewPosts")} onClick={refreshPosts}>
                        <FontAwesomeIcon
                            icon={faPlus}
                        />
                        <div>{t("NewPost")}</div>
                    </div>
                </div>
            }
            <ul className="posts">
                {!posts.posts
                    ? <Loading />
                    : posts.posts.map((post) => (
                        <li key={post?.id} className="posts__item">
                            <CommunityPost
                                userId={myselfId}
                                communityId={communityId}
                                post={post}
                                feedVersion={feedVersion}
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