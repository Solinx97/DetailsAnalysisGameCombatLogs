import { useEffect, useState } from 'react';
import { usePostSearchByCommunityIdAsyncQuery } from '../../../store/api/ChatApi';
import { useLazyGetPostByIdQuery, useRemovePostMutation } from '../../../store/api/communication/Post.api';
import { useLazyGetCommunityPostByPostIdQuery, useRemoveCommunityPostMutation } from '../../../store/api/communication/community/CommunityPost.api';
import Post from '../Post';
import Loading from '../../Loading';

const SelectedCommunityItem = ({ user, communityId }) => {
    const { data: communityPosts, isLoading } = usePostSearchByCommunityIdAsyncQuery(communityId);

    const [getPostById] = useLazyGetPostByIdQuery();
    const [getCommunityPostByPostId] = useLazyGetCommunityPostByPostIdQuery();
    const [removeCommunityPost] = useRemoveCommunityPostMutation();
    const [removePost] = useRemovePostMutation();

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

    const removeCommunityPostAsync = async (postId) => {
        const communityPost = await getCommunityPostByPostId(postId);
        if (communityPost.data === undefined || communityPost.data.length === 0) {
            return;
        }

        const result = await removeCommunityPost(communityPost.data[0].id);
        if (result.error === undefined) {
            await removePost(postId);
        }
    }

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <ul className="posts">
            {posts?.map((post) => (
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