import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { v4 as uuidv4 } from 'uuid';
import useFetchCommunityPosts from '../../../hooks/useFetchUsersPosts';
import Loading from '../../Loading';
import CommunityPost from '../post/CommunityPost';
import CreateUserPost from '../post/CreateUserPost';
import UserPost from '../post/UserPost';

const MyFeed = () => {
    const { t } = useTranslation("communication/feed");

    const user = useSelector((state) => state.user.value);

    const userPostsSizeRef = useRef(0);
    const communityPostsSizeRef = useRef(0);

    const [currentPosts, setCurrentPosts] = useState([]);

    const { posts, communityPosts, count, communityCount, isLoading, getMoreUserPostsAsync, getMoreCommunityPostsAsync } = useFetchCommunityPosts(user?.id, true);

    useEffect(() => {
        if (!posts) {
            return;
        }

        userPostsSizeRef.current = posts.length;

        const totalPosts = [...currentPosts, ...posts];
        totalPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

        setCurrentPosts(totalPosts);
    }, [posts]);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        communityPostsSizeRef.current = communityPosts.length;

        const totalPosts = [...currentPosts, ...communityPosts];
        totalPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

        setCurrentPosts(totalPosts);
    }, [communityPosts]);

    const loadingMoreUserPostsAsync = async () => {
        const newUserPosts = await getMoreUserPostsAsync(userPostsSizeRef.current);
        const newCommunityPosts = await getMoreCommunityPostsAsync(communityPostsSizeRef.current);

        const newPosts = newUserPosts.concat(newCommunityPosts);

        const totalPosts = [...currentPosts, ...newPosts];
        totalPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

        setCurrentPosts(totalPosts);
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div>
            <CreateUserPost
                user={user}
                owner={user?.username}
                t={t}
            />

            <ul className="posts">
                {currentPosts?.map(post => (
                    <li className="posts__item" key={uuidv4()}>
                        {post.communityId
                            ? <CommunityPost
                                userId={user?.id}
                                postId={post.id}
                                communityId={post.communityId}
                            />
                            : <UserPost
                                userId={user?.id}
                                postId={post.id}
                            />
                        }
                    </li>
                ))}
                {(currentPosts.length < (count + communityCount) && currentPosts.length > 0) &&
                    <li className="load-more" onClick={loadingMoreUserPostsAsync}>
                        <div className="load-more__content">Load more</div>
                    </li>
                }
            </ul>
        </div>
    );
}

export default MyFeed;