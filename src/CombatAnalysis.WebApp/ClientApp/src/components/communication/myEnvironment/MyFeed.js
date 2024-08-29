import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import useFetchCommunityPosts from '../../../hooks/useFetchUsersPosts';
import Loading from '../../Loading';
import CreateUserPost from '../post/CreateUserPost';
import UserPost from '../post/UserPost';

const MyFeed = () => {
    const { t } = useTranslation("communication/feed");

    const user = useSelector((state) => state.user.value);

    const [currentPosts, setCurrentPosts] = useState([]);

    const { posts, count, isLoading, getMoreUserPostsAsync } = useFetchCommunityPosts(user?.id, true);

    useEffect(() => {
        if (!posts) {
            return;
        }

        setCurrentPosts(posts);
    }, [posts]);

    const loadingMoreUserPostsAsync = async () => {
        const newPosts = await getMoreUserPostsAsync(currentPosts.length);

        setCurrentPosts(prevPosts => [...prevPosts, ...newPosts]);
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
                    <li className="posts__item" key={post.id}>
                        <UserPost
                            user={user}
                            post={post}
                        />
                    </li>
                ))}
                {currentPosts.length < count &&
                    <li onClick={loadingMoreUserPostsAsync}>Loading more...</li>
                }
            </ul>
        </div>
    );
}

export default MyFeed;