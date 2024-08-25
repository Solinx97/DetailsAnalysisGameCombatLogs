import { memo, useEffect, useState } from 'react';
import useFetchFriendsPosts from '../../hooks/useFetchFriendsPosts';
import { useRemoveUserPostMutation } from '../../store/api/communication/UserPost.api';
import { useFriendSearchMyFriendsQuery } from '../../store/api/communication/myEnvironment/Friend.api';
import Loading from '../Loading';
import Post from './Post';

const postType = {
    user: 0,
    community: 1
}

const FeedParticipants = ({ user, t }) => {
    const [currentPosts, setCurrentPosts] = useState([]);
    const [firstTimeLoaded, setFirstTimeLoaded] = useState(false);
    const [haveNewPosts, setHaveNewPosts] = useState(false);

    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(user?.id);

    const [removeUserPost] = useRemoveUserPostMutation();

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
        if (allPosts.length === 0 || firstTimeLoaded) {
            return;
        }

        const newPosts = getUniqueElements(currentPosts, allPosts);
        setCurrentPosts(prevPosts => [...newPosts, ...prevPosts]);

        setFirstTimeLoaded(true);
    }, [allPosts]);

    useEffect(() => {
        if (allPosts.length === 0 || !firstTimeLoaded) {
            return;
        }

        setHaveNewPosts(allPosts.length > currentPosts.length);
    }, [allPosts]);

    const removeUserPostAsync = async (userPostId) => {
        console.log(userPostId);
        const response = await removeUserPost(userPostId);
        if (response.error) {
            return;
        }

        const getRemovedPost = allPosts.filter(post => post.id === userPostId);
        if (getRemovedPost.length > 0) {
            const indexOf = allPosts.indexOf(getRemovedPost[0]);
            allPosts.splice(indexOf, 1);
        }
    }

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
                        <Post
                            user={user}
                            data={post}
                            deletePostAsync={async () => await removeUserPostAsync(post.id)}
                        />
                    </li>
                ))}
            </ul>
        </>
    );
}

export default memo(FeedParticipants);