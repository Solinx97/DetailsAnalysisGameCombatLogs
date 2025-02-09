import { memo, useEffect, useRef, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';
import useFetchUsersPosts from '../../hooks/useFetchUsersPosts';
import { FeedParticipantsProps } from '../../types/components/communication/FeedParticipantsProps';
import Loading from '../Loading';
import CommunityPost from './post/CommunityPost';
import UserPost from './post/UserPost';

const FeedParticipants: React.FC<FeedParticipantsProps> = ({ meId, t }) => {
    const userPostsSizeRef = useRef(0);
    const communityPostsSizeRef = useRef(0);

    const [currentPosts, setCurrentPosts] = useState<any[]>([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { posts, communityPosts, newPosts, newCommunityPosts, count, communityCount, isLoading, getMoreUserPostsAsync, getMoreCommunityPostsAsync, currentDateRef } = useFetchUsersPosts(meId);

    useEffect(() => {
        if (!posts) {
            return;
        }

        userPostsSizeRef.current = posts.length;
        if (posts.length === 0) {
            return;
        }

        const totalPosts = Array.from(posts);
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalPosts);
    }, [posts]);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        communityPostsSizeRef.current = communityPosts.length;
        if (communityPosts.length === 0) {
            return;
        }

        const totalPosts = Array.from(communityPosts);
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

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
        totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalPosts);
    }

    const loadingNewUserPostsAsync = async () => {
        currentDateRef.current = (new Date()).toISOString();

        newPosts?.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        setCurrentPosts(prevPosts => [...(newPosts || []), ...prevPosts]);

        newCommunityPosts?.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
        setCurrentPosts(prevPosts => [...(newCommunityPosts || []), ...prevPosts]);

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
                                userId={meId}
                                post={post}
                                communityId={post.communityId}
                            />
                            : <UserPost
                                meId={meId}
                                post={post}
                            />
                        }
                    </li>
                ))}
                {(currentPosts.length < (count + communityCount) && currentPosts.length > 0) &&
                    <li className="load-more" onClick={loadingMoreUserPostsAsync}>
                        <div className="load-more__content">{t("LoadMore")}</div>
                    </li>
                }
            </ul>
        </>
    );
}

export default memo(FeedParticipants);