import { memo, useEffect, useState } from 'react';
import useFetchCommunityPosts from '../../../hooks/useFetchCommunityPosts';
import Loading from '../../Loading';
import CommunityPost from '../post/CommunityPost';

const SelectedCommunityItem = ({ userId, communityId, t }) => {
    const [currentPosts, setCurrentPosts] = useState([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { posts, newPosts, count, isLoading, getMoreCommunityPostsAsync, currentDateRef, skipCheckNewPostsRef } = useFetchCommunityPosts(communityId);

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

    const loadingMoreCommunityPostsAsync = async () => {
        const newPosts = await getMoreCommunityPostsAsync(currentPosts.length);

        setCurrentPosts(prevPosts => [...prevPosts, ...newPosts]);
    }

    const loadingNewCommunityPostsAsync = async () => {
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
                <div className="new-posts" onClick={loadingNewCommunityPostsAsync}>
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
            }
            <ul className="posts">
                {currentPosts.map((post) => (
                    <li key={post?.id}>
                        <CommunityPost
                            userId={userId}
                            post={post}
                            communityId={communityId}
                        />
                    </li>
                    ))
                }
                {currentPosts.length < count &&
                    <li className="load-more" onClick={loadingMoreCommunityPostsAsync}>
                        <div className="load-more__content">Load more</div>
                    </li>
                }
            </ul>
        </>
    );
}

export default memo(SelectedCommunityItem);