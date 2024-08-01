import { memo } from 'react';
import useFetchFriendsPosts from '../../hooks/useFetchFriendsPosts';
import { useLazyUserPostSearchByPostIdQuery, useRemoveUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import { useFriendSearchMyFriendsQuery } from '../../store/api/communication/myEnvironment/Friend.api';
import Loading from '../Loading';
import Post from './Post';

const postType = {
    user: 0,
    community: 1
}

const FeedParticipants = ({ user, t }) => {
    const { data: myFriends, isLoading } = useFriendSearchMyFriendsQuery(user?.id);

    const [getUserPostByPostId] = useLazyUserPostSearchByPostIdQuery();
    const [removeUserPost] = useRemoveUserPostAsyncMutation();

    const { allPosts, newPosts, insertNewPostsAsync } = useFetchFriendsPosts(user?.id, myFriends);

    const removeUserPostAsync = async (postId) => {
        const userPost = await getUserPostByPostId(postId);
        if (userPost.data === undefined || userPost.data.length === 0) {
            return;
        }

        const userPostData = userPost.data[0];
        const result = await removeUserPost(userPostData.id);
        if (result.error !== undefined) {
            return;
        }

        const getRemovedPost = allPosts.filter(post => post.id === userPostData.postId);
        if (getRemovedPost.length > 0) {
            const indexOf = allPosts.indexOf(getRemovedPost[0]);
            allPosts.splice(indexOf, 1);
        }
    }

    const insertNewPostsHandleAsync = async () => {
        window.scroll(0, 0);
        await insertNewPostsAsync();
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <>
            {newPosts.length > 0 &&
                <div onClick={async () => await insertNewPostsHandleAsync()} className="new-posts">
                    <div className="new-posts__content">{t("NewPosts")}</div>
                </div>
            }
            <ul className="posts">
                {allPosts?.map(post => (
                    <li key={post.id}>
                        <Post
                            user={user}
                            data={post}
                            deletePostAsync={async () => await removeUserPostAsync(post.id)}
                            canBeRemoveFromUserFeed={post.postType === postType["user"]}
                        />
                    </li>
                ))}
            </ul>
        </>
    );
}

export default memo(FeedParticipants);