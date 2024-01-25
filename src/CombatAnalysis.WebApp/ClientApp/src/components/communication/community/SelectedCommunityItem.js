import { useEffect, useState } from 'react';
import { usePostSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useLazyGetPostByIdQuery } from '../../../store/api/communication/Post.api';
import { useRemoveCommunityPostAsyncMutation } from '../../../store/api/communication/community/CommunityPost.api';
import Post from '../Post';

const SelectedCommunityItem = ({ customer, communityId }) => {
    const { data: communityPosts, isLoading } = usePostSearchByCommunityIdAsyncQuery(communityId);

    const [getPostById] = useLazyGetPostByIdQuery();

    const [removeCommunityPostAsync] = useRemoveCommunityPostAsyncMutation();
    const [posts, setPosts] = useState([]);

    useEffect(() => {
        if (communityPosts === undefined) {
            return;
        }

        const getPosts = async () => {
            await getPostsAsync();
        }

        getPosts();
    }, [communityPosts])

    const getPostsAsync = async () => {
        const allPosts = [];

        for (let i = 0; i < communityPosts.length; i++) {
            const post = await getPostById(communityPosts[i].postId);

            if (post.data !== undefined) {
                allPosts.push(post.data);
            }
        }

        setPosts(allPosts);
    }

    if (isLoading) {
        return <div>Loading...</div>;
    }

    return (
        <ul className="posts">
            {posts?.map((post) => (
                    <li key={post?.id}>
                        <Post
                            customer={customer}
                            post={post}
                            deletePostAsync={async () => await removeCommunityPostAsync(post.id)}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default SelectedCommunityItem;