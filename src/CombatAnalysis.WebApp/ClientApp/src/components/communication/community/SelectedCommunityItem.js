import { memo, useEffect, useState } from 'react';
import useFetchCommunityPosts from '../../../hooks/useFetchCommunityPosts';
import Loading from '../../Loading';
import CommunityPost from '../post/CommunityPost';

const SelectedCommunityItem = ({ user, communityId, t }) => {
    const [currentPosts, setCurrentPosts] = useState([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { communityPosts, newCommunityPosts, count, isLoading, getMoreCommunityPostsAsync, currentDateRef, skipCheckNewPostsRef } = useFetchCommunityPosts(communityId);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        setCurrentPosts(communityPosts);

        skipCheckNewPostsRef.current = false;
    }, [communityPosts]);

    useEffect(() => {
        if (!newCommunityPosts || newCommunityPosts.length === 0) {
            return;
        }

        const newPosts = getUniqueElements(currentPosts, newCommunityPosts);
        setHaveNewPosts(newPosts.length > 0);
    }, [newCommunityPosts]);

    const loadingMoreCommunityPostsAsync = async () => {
        const newPosts = await getMoreCommunityPostsAsync();

        setCurrentPosts(prevPosts => [...prevPosts, ...newPosts]);
    }

    const loadingNewCommunityPostsAsync = async () => {
        currentDateRef.current = (new Date()).toISOString();

        const newPosts = getUniqueElements(currentPosts, newCommunityPosts);
        setCurrentPosts(prevPosts => [...newPosts, ...prevPosts]);

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
                            user={user}
                            post={post}
                            communityId={communityId}
                        />
                    </li>
                ))
                }
                {currentPosts.length < count &&
                    <li onClick={loadingMoreCommunityPostsAsync}>Loading more...</li>
                }
            </ul>
        </>
    );
}

export default memo(SelectedCommunityItem);