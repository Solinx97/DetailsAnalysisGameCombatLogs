import { memo, useEffect, useState } from 'react';
import useFetchFriendsPosts from '../../hooks/useFetchFriendsPosts';
import { useFriendSearchMyFriendsQuery } from '../../store/api/communication/myEnvironment/Friend.api';
import Loading from '../Loading';
import UserPost from './post/UserPost';

const FeedParticipants = ({ user, t }) => {
    const [currentPosts, setCurrentPosts] = useState([]);
    const [firstTimeLoaded, setFirstTimeLoaded] = useState(false);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(user?.id);

    const { allMyPersonalPosts, allPosts, postsAreLoading } = useFetchFriendsPosts(user?.id, myFriends);

    useEffect(() => {
        if (allMyPersonalPosts.length === 0 || firstTimeLoaded) {
            return;
        }

        const newPosts = getUniqueElements(currentPosts, allMyPersonalPosts);
        setCurrentPosts(prevPosts => [...newPosts, ...prevPosts]);

        setFirstTimeLoaded(true);
    }, [allMyPersonalPosts]);

    useEffect(() => {
        if (!allPosts || allPosts.length === 0 || firstTimeLoaded) {
            return;
        }

        const newPosts = getUniqueElements(currentPosts, allPosts);
        setCurrentPosts(prevPosts => [...newPosts, ...prevPosts]);

        setFirstTimeLoaded(true);
    }, [allPosts]);

    useEffect(() => {
        if (!allPosts || allPosts.length === 0 || !firstTimeLoaded) {
            return;
        }

        setHaveNewPosts(allPosts.length > currentPosts.length);
    }, [allPosts]);

    const getUniqueElements = (oldArray, newArray) => {
        const oldSet = new Set(oldArray.map(item => item.id));
        const uniqueNewElements = newArray.filter(item => !oldSet.has(item.id));

        return uniqueNewElements;
    }

    const handleShowNewPosts = () => {
        const newPosts = getUniqueElements(currentPosts, allPosts);
        setCurrentPosts(prevPosts => [...newPosts, ...prevPosts]);

        setHaveNewPosts(false);
    }

    if (isLoading || (!firstTimeLoaded && postsAreLoading)) {
        return (<Loading />);
    }

    return (
        <>
            {haveNewPosts &&
                <div onClick={handleShowNewPosts} className="new-posts">
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
            }
            <ul className="posts">
                {currentPosts?.map(post => (
                    <li key={post.id}>
                        <UserPost
                            user={user}
                            post={post}
                        />
                    </li>
                ))}
            </ul>
        </>
    );
}

export default memo(FeedParticipants);