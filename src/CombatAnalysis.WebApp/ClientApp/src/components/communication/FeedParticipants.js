import { memo, useEffect, useState } from 'react';
import Loading from '../Loading';
import UserPost from './post/UserPost';
import useFetchCommunityPosts from '../../hooks/useFetchUsersPosts';

const FeedParticipants = ({ userId, t }) => {
    const [currentPosts, setCurrentPosts] = useState([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { posts, newPosts, count, isLoading, getMoreUserPostsAsync, currentDateRef, skipCheckNewPostsRef } = useFetchCommunityPosts(userId);

    useEffect(() => {
        if (!posts) {
            return;
        }

        setCurrentPosts(posts);

        skipCheckNewPostsRef.current = false;
    }, [posts]);

    useEffect(() => {
        if (!newPosts || newPosts.length === 0) {
            return;
        }

        const uniqNewPosts = getUniqueElements(currentPosts, newPosts);
        setHaveNewPosts(uniqNewPosts.length > 0);
    }, [newPosts]);

    const loadingMoreUserPostsAsync = async () => {
        const newPosts = await getMoreUserPostsAsync(currentPosts.length);

        setCurrentPosts(prevPosts => [...prevPosts, ...newPosts]);
    }

    const loadingNewUserPostsAsync = async () => {
        currentDateRef.current = (new Date()).toISOString();

        const uniqNewPosts = getUniqueElements(currentPosts, newPosts);
        setCurrentPosts(prevPosts => [...uniqNewPosts, ...prevPosts]);

        setHaveNewPosts(false);
    }

    const getUniqueElements = (oldArray, newArray) => {
        const oldSet = new Set(oldArray.map(item => item.id));
        const uniqueNewElements = newArray.filter(item => !oldSet.has(item.id));

        return uniqueNewElements;
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
            {haveNewPosts &&
                <div onClick={loadingNewUserPostsAsync} className="new-posts">
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
            }
            <ul className="posts">
                {currentPosts?.map(post => (
                    <li key={post.id}>
                        <UserPost
                            user={userId}
                            post={post}
                        />
                    </li>
                ))}
                {currentPosts.length < count &&
                    <li onClick={loadingMoreUserPostsAsync}>Loading more...</li>
                }
            </ul>
        </>
    );
}

export default memo(FeedParticipants);