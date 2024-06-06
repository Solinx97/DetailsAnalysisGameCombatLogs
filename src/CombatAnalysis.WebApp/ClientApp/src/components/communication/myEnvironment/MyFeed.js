import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { useLazyUserPostSearchByUserIdQuery } from '../../../store/api/ChatApi';
import { useLazyGetPostByIdQuery } from '../../../store/api/communication/Post.api';
import { useCreateUserPostAsyncMutation, useLazyUserPostSearchByPostIdQuery, useRemoveUserPostAsyncMutation } from '../../../store/api/communication/UserPost.api';
import { useRemovePostMutation } from '../../../store/api/communication/Post.api';
import CreatePost from '../CreatePost';
import Post from '../Post';

const MyFeed = () => {
    const customer = useSelector((state) => state.customer.value);

    const [getUserPosts] = useLazyUserPostSearchByUserIdQuery();
    const [getPostById] = useLazyGetPostByIdQuery();
    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();
    const [getUserPostByPostId] = useLazyUserPostSearchByPostIdQuery();
    const [removeUserPost] = useRemoveUserPostAsyncMutation();
    const [removePost] = useRemovePostMutation();

    const [allPosts, setAllPosts] = useState([]);

    useEffect(() => {
        if (customer === null) {
            return;
        }

        const getAllPosts = async () => {
            await getAllPostsAsync();
        }

        getAllPosts();
    }, [customer])

    const getAllPostsAsync = async () => {
        const userPosts = await getUserPosts(customer.id);

        if (userPosts.data !== undefined) {
            const userPersonalPosts = await getUserPostsAsync(userPosts.data);
            setAllPosts(userPersonalPosts);
        }
    }

    const getUserPostsAsync = async (userPosts) => {
        const userPersonalPosts = [];

        for (let i = 0; i < userPosts.length; i++) {
            const post = await getPostById(userPosts[i].postId);

            if (post.data !== undefined) {
                userPersonalPosts.push(post.data);
            }
        }

        return userPersonalPosts;
    }

    const createUserPostAsync = async (postId) => {
        const newUserPost = {
            userId: customer?.id,
            postId: postId
        }

        const createdUserPost = await createNewUserPostAsync(newUserPost);
        return createdUserPost.data === undefined ? false : true;
    }

    const removeUserPostAsync = async (postId) => {
        const userPost = await getUserPostByPostId(postId);
        if (userPost.data === undefined || userPost.data.length === 0) {
            return;
        }

        const result = await removeUserPost(userPost.data[0].id);
        if (result.error === undefined) {
            await removePost(postId);
        }
    }

    if (customer === null) {
        return <div>Loading...</div>;
    }

    return (
        <div>
            <CreatePost
                customer={customer}
                owner={customer?.username}
                postTypeName="user"
                createTypeOfPostFunc={createUserPostAsync}
            />
            <ul className="posts">
                {allPosts?.map(post => (
                    <li className="posts__item" key={post.id}>
                        <Post
                            customer={customer}
                            post={post}
                            deletePostAsync={async () => await removeUserPostAsync(post.id)}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MyFeed;