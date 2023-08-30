import { usePostSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useRemoveCommunityPostAsyncMutation } from '../../../store/api/communication/community/CommunityPost.api';
import Post from '../Post';

const getCommunityPostsInterval = 10000;

const SelectedCommunityItem = ({ customer, communityId }) => {
    const { data: communityPosts, isLoading } = usePostSearchByCommunityIdAsyncQuery(communityId, {
        pollingInterval: getCommunityPostsInterval
    });
    const [removeCommunityPostAsyncMutation] = useRemoveCommunityPostAsyncMutation();

    const deleteCommunityPostAsync = async (communityPostId) => {
        await removeCommunityPostAsyncMutation(communityPostId);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <ul>
            {
                communityPosts?.map((item) => (
                    <li key={item?.id}>
                        <Post
                            key={item?.id}
                            customer={customer}
                            targetPostType={item}
                            deletePostAsync={async () => await deleteCommunityPostAsync(item.id)}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default SelectedCommunityItem;