import { useUserPostSearchByUserIdQuery as useSearchByUserIdQuery } from '../../store/api/ChatApi';
import { useRemoveUserPostAsyncMutation } from '../../store/api/UserPost.api';
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
            {
                userPosts?.map((item) => (
                    <li key={item?.id}>
                        <Post
                            customer={customer}
                            targetPostType={item}
                            deletePostAsync={async () => await deleteCommunityPostAsync(item.id)}
                        />
                    </li>
                ))
            }
        </>
    );
}

export default UserPosts;