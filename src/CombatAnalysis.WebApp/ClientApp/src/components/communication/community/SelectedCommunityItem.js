import { useCommunityPostSearchByCommunityIdAsyncQuery } from '../../../store/api/CommunityApi';
import { useRemoveCommunityPostMutation } from '../../../store/api/communication/CommunityPost.api';
import Loading from '../../Loading';
import Post from '../Post';

const SelectedCommunityItem = ({ user, communityId }) => {
    const { data: communityPosts, isLoading } = useCommunityPostSearchByCommunityIdAsyncQuery(communityId);

    const [removeCommunityPost] = useRemoveCommunityPostMutation();

    const removeCommunityPostAsync = async (communityPostId) => {
        await removeCommunityPost(communityPostId);
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <ul className="posts">
            {communityPosts?.map((post) => (
                    <li key={post?.id}>
                        <Post
                            user={user}
                            data={post}
                            deletePostAsync={async () => await removeCommunityPostAsync(post.id)}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default SelectedCommunityItem;