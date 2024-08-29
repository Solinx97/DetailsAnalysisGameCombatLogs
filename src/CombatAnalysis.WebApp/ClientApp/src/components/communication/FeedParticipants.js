import { memo, useEffect, useRef, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';
import useFetchCommunityPosts from '../../hooks/useFetchUsersPosts';
import Loading from '../Loading';
import CommunityPost from './post/CommunityPost';
import UserPost from './post/UserPost';

const FeedParticipants = ({ userId, t }) => {
    const userPostsSizeRef = useRef(0);
    const communityPostsSizeRef = useRef(0);

    const [currentPosts, setCurrentPosts] = useState([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { posts, communityPosts, newPosts, newCommunityPosts, count, communityCount, isLoading, getMoreUserPostsAsync, getMoreCommunityPostsAsync, currentDateRef } = useFetchCommunityPosts(userId, false);

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

    useEffect(() => {
        if (!newPosts || newPosts.length === 0) {
            return;
        }

        const uniqNewPosts = getUniqueElements(currentPosts, newPosts);
        setHaveNewPosts(uniqNewPosts.length > 0);
    }, [newPosts]);

    useEffect(() => {
        if (!newCommunityPosts || newCommunityPosts.length === 0) {
            return;
        }

        const uniqNewPosts = getUniqueElements(currentPosts, newCommunityPosts);
        setHaveNewPosts(uniqNewPosts.length > 0);
    }, [newCommunityPosts]);

    const loadingMoreUserPostsAsync = async () => {
        const newUserPosts = await getMoreUserPostsAsync(userPostsSizeRef.current);
        const newCommunityPosts = await getMoreCommunityPostsAsync(communityPostsSizeRef.current);

        const newPosts = newUserPosts.concat(newCommunityPosts);

        const totalPosts = [...currentPosts, ...newPosts];
        totalPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

        setCurrentPosts(totalPosts);
    }

    const loadingNewUserPostsAsync = async () => {
        currentDateRef.current = (new Date()).toISOString();

        let uniqNewPosts = getUniqueElements(currentPosts, newPosts);
        setCurrentPosts(prevPosts => [...uniqNewPosts, ...prevPosts]);

        uniqNewPosts = getUniqueElements(currentPosts, newCommunityPosts);
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
                    <li key={uuidv4()}>
                        {post.communityId
                            ? <CommunityPost
                                userId={userId}
                                post={post}
                                communityId={post.communityId}
                            />
                            : <UserPost
                                user={userId}
                                post={post}
                            />
                        }
                    </li>
                ))}
                {(currentPosts.length < (count + communityCount) && currentPosts.length > 0) &&
                    <li onClick={loadingMoreUserPostsAsync}>Loading more...</li>
                }
            </ul>
        </>
    );
}

export default memo(FeedParticipants);