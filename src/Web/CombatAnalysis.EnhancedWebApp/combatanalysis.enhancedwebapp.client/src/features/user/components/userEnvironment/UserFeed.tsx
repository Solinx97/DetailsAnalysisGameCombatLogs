import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import Loading from '@/shared/components/Loading';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import CommunityPost from '../../../feed/components/post/CommunityPost';
import CreateUserPost from '../../../feed/components/post/CreateUserPost';
import UserPost from '../../../feed/components/post/UserPost';
import useFetchUserPosts from '../../../feed/hooks/useFetchUserPosts';
import type { CommunityPostModel } from '../../../feed/types/CommunityPostModel';
import type { UserPostModel } from '../../../feed/types/UserPostModel';

const UserFeed: React.FC = () => {
    const { t } = useTranslation("communication/feed");

    const myself = useSelector((state: RootState) => state.user.value);

    const [page, setPage] = useState(1);;
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.userPostSize ?? 5);

    const { userFeed, userFeedIsLoading } = useFetchUserPosts(page, pageSizeRef.current, myself?.id ?? "");

    useEffect(() => {
        if (!userFeed) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < userFeed.length);
    }, [page, userFeed]);

    if (!myself || !userFeed) {
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
                    {userFeed?.map(post => (
                        <li className="posts__item" key={post.id}>
                            {(post as CommunityPostModel).communityId !== undefined
                                ? <CommunityPost
                                    userId={myself.id}
                                    communityId={(post as CommunityPostModel).communityId}
                                    post={(post as CommunityPostModel)}
                                />
                                : <UserPost
                                    myself={myself}
                                    post={(post as UserPostModel)}
                                />
                            }
                        </li>
                    ))}
                    <li className="posts__item">
                        <InfiniteScrollTrigger
                            onLoadMore={() => setPage(p => p + 1)}
                            hasMore={hasMore}
                            isLoading={userFeedIsLoading}
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