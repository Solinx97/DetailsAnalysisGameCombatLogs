import { usePostSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useRemoveCommunityPostAsyncMutation } from '../../../store/api/communication/community/CommunityPost.api';
import Post from '../Post';

const getCommunityPostsInterval = 10000;

const SelectedCommunityItem = ({ customer, communityId }) => {
    const { data: communityPosts, isLoading } = usePostSearchByCommunityIdAsyncQuery(communityId, {
        pollingInterval: getCommunityPostsInterval
    });
    const [removeCommunityPostAsync] = useRemoveCommunityPostAsyncMutation();

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <ul className="posts">
            {communityPosts?.map((communityPost) => (
                    <li key={communityPost?.id}>
                        <Post
                            customer={customer}
                            postId={communityPost.postId}
                            deletePostAsync={async () => await removeCommunityPostAsync(communityPost.id)}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default SelectedCommunityItem;