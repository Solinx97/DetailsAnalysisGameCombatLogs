import { useUserPostSearchByUserIdQuery as useSearchByUserIdQuery } from '../../store/api/ChatApi';
import { useRemoveUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import Post from './Post';

const getUserPostsInterval = 10000;

const UserPosts = ({ customer, userId }) => {
    const { data: userPosts, isLoading } = useSearchByUserIdQuery(userId, {
        pollingInterval: getUserPostsInterval
    });
    const [removeCommunityPostAsyncMutation] = useRemoveUserPostAsyncMutation();

    const deleteCommunityPostAsync = async (communityPostId) => {
        await removeCommunityPostAsyncMutation(communityPostId);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            {userPosts?.map((userPost) => (
                    <li key={userPost?.id}>
                        <Post
                            customer={customer}
                            targetPostType={userPost}
                            deletePostAsync={async () => await deleteCommunityPostAsync(userPost.id)}
                        />
                    </li>
                ))
            }
        </>
    );
}

export default UserPosts;