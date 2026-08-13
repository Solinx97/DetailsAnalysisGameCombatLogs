import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import CreateUserPost from '../../../feed/components/post/CreateUserPost';
import UserPost from '../../../feed/components/post/UserPost';
import { useGetUserPostsByUserIdQuery } from '@/features/feed/api/Post.api';

const UserFeed: React.FC = () => {
    const { t } = useTranslation("communication/feed");

    const myself = useSelector((state: RootState) => state.user.value);

    const [page, setPage] = useState(1);;
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.userPostSize ?? 5);

    const { data: posts, isLoading, isFetching } = useGetUserPostsByUserIdQuery({ appUserId: myself?.id ?? "", page, pageSize: pageSizeRef.current });

    useEffect(() => {
        if (!posts) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < posts.count);
    }, [page, posts]);

    if (!posts || isLoading) {
        return (<Loading />);
    }

    if (!myself) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={5}
                    hasSubMenu={true}
                />
                <Loading />
            </>
        );
    }

    return (
        <>
            <div>
                <CreateUserPost
                    user={myself}
                    owner={myself.username}
                    t={t}
                />
                <ul className="posts">
                    {!posts.posts
                        ? <Loading />
                        : posts.posts.map(post => (
                            <li key={`${post.id}`}>
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
            </div>
            <CommunicationMenu
                currentMenuItem={5}
                hasSubMenu={true}
            />
        </>
    );
}

export default UserFeed;