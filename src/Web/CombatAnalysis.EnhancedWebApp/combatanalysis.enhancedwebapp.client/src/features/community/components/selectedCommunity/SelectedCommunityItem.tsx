import Loading from '@/shared/components/Loading';
import { memo, useEffect, useState } from 'react';
import CommunityPost from '../../../feed/components/post/CommunityPost';
import useFetchPosts from '../../../feed/hooks/useFetchPosts';
import type { CommunityPostModel } from '../../../feed/types/CommunityPostModel';

interface SelectedCommunityItemProps {
    myselfId: string;
    communityId: number;
    t: (key: string) => string;
}

const SelectedCommunityItem: React.FC<SelectedCommunityItemProps> = ({ myselfId, communityId, t }) => {
    const [currentPosts, setCurrentPosts] = useState<CommunityPostModel[]>([]);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { communityPosts, communityCount, currentDateRef } = useFetchPosts(myselfId, communityId);

    useEffect(() => {
        if (!communityPosts) {
            return;
        }

        setCurrentPosts(communityPosts);
    }, [communityPosts]);

    // useEffect(() => {
    //     if (!newCommunityPosts || newCommunityPosts.length === 0) {
    //         return;
    //     }

    //     const uniqNewPosts = getUniqueElements(currentPosts, newCommunityPosts);
    //     setHaveNewPosts(uniqNewPosts.length > 0);
    // }, [newCommunityPosts]);

    // const loadingMoreCommunityPostsAsync = async () => {
    //     const newPosts = await getMoreCommunityPostsAsync(currentPosts.length);

    //     setCurrentPosts(prevPosts => [...prevPosts, ...newPosts]);
    // }

    // const loadingNewCommunityPostsAsync = async () => {
    //     if (!newCommunityPosts) {
    //         return;
    //     }

    //     currentDateRef.current = (new Date()).toISOString();

    //     const uniqNewPosts = getUniqueElements(currentPosts, newCommunityPosts);
    //     setCurrentPosts(prevPosts => [...uniqNewPosts, ...prevPosts]);

    //     setHaveNewPosts(false);
    // }

    // const getUniqueElements = (oldArray: CommunityPostModel[], newArray: CommunityPostModel[]) => {
    //     const oldSet = new Set(oldArray.map(item => item.id));
    //     const uniqueNewElements = newArray.filter(item => !oldSet.has(item.id));

    //     return uniqueNewElements;
    // }

    if (!communityPosts) {
        return (<Loading />);
    }

    return (
        <>
            {/* {haveNewPosts &&
                <div className="new-posts" onClick={loadingNewCommunityPostsAsync}>
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
            } */}
            <ul className="posts">
                {communityPosts.map((post) => (
                    <li key={post?.id}>
                        <CommunityPost
                            userId={myselfId}
                            communityId={communityId}
                            post={post}
                        />
                    </li>
                    ))
                }
                {/* {currentPosts.length < communityCount &&
                    <li className="load-more" onClick={loadingMoreCommunityPostsAsync}>
                        <div className="load-more__content">Load more</div>
                    </li>
                } */}
            </ul>
        </>
    );
}

export default memo(SelectedCommunityItem);