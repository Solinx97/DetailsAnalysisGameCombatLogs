import Loading from '@/shared/components/Loading';
import { memo, useEffect, useRef, useState } from 'react';
import type { AppUserModel } from '../../user/types/AppUserModel';
import useFetchPosts from '../hooks/useFetchPosts';
import type { CommunityPostModel } from '../types/CommunityPostModel';
import type { UserPostModel } from '../types/UserPostModel';
import CommunityPost from './post/CommunityPost';
import UserPost from './post/UserPost';

interface FeedParticipantsProps {
    myself: AppUserModel;
    t: (key: string) => string;
}

const FeedParticipants: React.FC<FeedParticipantsProps> = ({ myself, t }) => {
    const userPostsSizeRef = useRef(0);
    const communityPostsSizeRef = useRef(0);

    const [currentPosts, setCurrentPosts] = useState<UserPostModel[] | CommunityPostModel[]>([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { userPosts, communityPosts, count, communityCount, currentDateRef } = useFetchPosts(myself.id);

    useEffect(() => {
        if (!userPosts) {
            return;
        }

        userPostsSizeRef.current = userPosts.length;
        if (userPosts.length === 0) {
            return;
        }

        const totalUserPosts = Array.from(userPosts);
        totalUserPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts(totalUserPosts);
    }, [userPosts]);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        communityPostsSizeRef.current = communityPosts.length;
        if (communityPosts.length === 0) {
            return;
        }

        const totalCommunityPosts = Array.from(communityPosts);
        totalCommunityPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

        setCurrentPosts([...currentPosts, ...totalCommunityPosts]);
    }, [communityPosts]);

    // useEffect(() => {
    //     if (!newPosts || newPosts.length === 0) {
    //         return;
    //     }

    //     setHaveNewPosts(newPosts.length > 0);
    // }, [newPosts]);

    // useEffect(() => {
    //     if (!newCommunityPosts || newCommunityPosts.length === 0) {
    //         return;
    //     }

    //     setHaveNewPosts(newCommunityPosts.length > 0);
    // }, [newCommunityPosts]);

    // const loadingMoreUserPostsAsync = async () => {
    //     const newUserPosts = await getMoreUserPostsAsync(userPostsSizeRef.current);
    //     const newCommunityPosts = await getMoreCommunityPostsAsync(communityPostsSizeRef.current);

    //     const newPosts = newUserPosts.concat(newCommunityPosts);

    //     const totalPosts = [...currentPosts, ...newPosts];
    //     totalPosts.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());

    //     setCurrentPosts(totalPosts);
    // }

    // const loadingNewUserPostsAsync = async () => {
    //     currentDateRef.current = (new Date()).toISOString();

    //     newPosts?.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
    //     setCurrentPosts(prevPosts => [...(newPosts || []), ...prevPosts]);

    //     newCommunityPosts?.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
    //     setCurrentPosts(prevPosts => [...(newCommunityPosts || []), ...prevPosts]);

    //     setHaveNewPosts(false);
    // }

    return (
        <>
            {/* {haveNewPosts &&
                <div onClick={loadingNewUserPostsAsync} className="new-posts">
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
            } */}
            <ul className="posts">
                {!currentPosts
                    ? <Loading />
                    : currentPosts.map(post => (
                    <li key={post.id}>
                        {"communityId" in post
                            ? <CommunityPost
                                userId={myself.id}
                                post={post}
                                communityId={post.communityId}
                            />
                            : <UserPost
                                myself={myself}
                                post={post}
                            />
                        }
                    </li>
                ))}
                {/* {(currentPosts.length < (count + communityCount) && currentPosts.length > 0) &&
                    <li className="load-more" onClick={loadingMoreUserPostsAsync}>
                        <div className="load-more__content">{t("LoadMore")}</div>
                    </li>
                } */}
            </ul>
        </>
    );
}

export default memo(FeedParticipants);