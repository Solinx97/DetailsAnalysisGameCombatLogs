import { useCommunityPostSearchByCommunityIdAsyncQuery } from '../../../store/api/CommunityApi';
import { useRemoveCommunityPostMutation } from '../../../store/api/communication/CommunityPost.api';
import Loading from '../../Loading';
import CommunityPost from '../post/CommunityPost';

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
                        <CommunityPost
                            user={user}
                            post={post}
                            communityId={communityId}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default SelectedCommunityItem;