import { APP_CONFIG } from '@/config/appConfig';
import Loading from '@/shared/components/Loading';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
import CommunityPost from './post/CommunityPost';
import UserPost from './post/UserPost';
import { useCountFeedNewPostsQuery, useGetFeedQuery } from '../api/UserFeed.api';

interface FeedParticipantsProps {
    myself: AppUserModel;
    lastCheck: string;
    setLastCheck: (value: SetStateAction<string>) => void;
    feedVersion: number;
    setFeedVersion: (value: SetStateAction<number>) => void;
    t: (key: string) => string;
}

const FeedParticipants: React.FC<FeedParticipantsProps> = ({ myself, lastCheck, setLastCheck, feedVersion, setFeedVersion, t }) => {
    const [page, setPage] = useState(1);
    const [countNew, setCountNew] = useState(0);
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.userPostSize ?? 5);
    const intervalCheckNewPostsRef = useRef<number>(APP_CONFIG.communication.intervalCheckNewPosts ?? 100000);

    const { data: countFeedNewPost } = useCountFeedNewPostsQuery(
        { appUserId: myself.id, lastCheck },
        {
            pollingInterval: intervalCheckNewPostsRef.current
        }
    );
    const { data: userFeed, isLoading, isFetching } = useGetFeedQuery({ appUserId: myself.id, page, pageSize: pageSizeRef.current, feedVersion });

    useEffect(() => {
        if (!userFeed) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < userFeed?.count);
    }, [page, userFeed]);

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

    if (!userFeed || isLoading) {
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
                {!userFeed.posts
                    ? <Loading />
                    : userFeed.posts.map(post => (
                        post.communityId
                            ? <li key={`${post.id} c`}>
                                <CommunityPost
                                    userId={myself.id}
                                    communityId={post.communityId ?? 0}
                                    post={post}
                                    feedVersion={feedVersion}
                                />
                            </li>
                            : <li key={`${post.id} u`}>
                                <UserPost
                                    myself={myself}
                                    post={post}
                                    feedVersion={feedVersion}
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
