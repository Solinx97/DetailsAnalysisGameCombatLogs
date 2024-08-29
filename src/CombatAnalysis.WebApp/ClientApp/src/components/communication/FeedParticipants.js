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
        if (!posts || posts.length === 0) {
            return;
        }

        userPostsSizeRef.current = posts.length;

        const totalPosts = [...currentPosts, ...posts];
        totalPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

        setCurrentPosts(totalPosts);
    }, [posts]);

    useEffect(() => {
        if (!communityPosts || communityPosts.length === 0) {
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

        setHaveNewPosts(newPosts.length > 0);
    }, [newPosts]);

    useEffect(() => {
        if (!newCommunityPosts || newCommunityPosts.length === 0) {
            return;
        }

        setHaveNewPosts(newCommunityPosts.length > 0);
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

        newPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
        setCurrentPosts(prevPosts => [...newPosts, ...prevPosts]);

        newCommunityPosts.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
        setCurrentPosts(prevPosts => [...newCommunityPosts, ...prevPosts]);

        setHaveNewPosts(false);
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
                                userId={userId}
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