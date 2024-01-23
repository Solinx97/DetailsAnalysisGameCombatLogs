import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLazyUserPostSearchByUserIdQuery } from '../../../store/api/ChatApi';
import { useCreatePostAsyncMutation, useLazyGetPostByIdQuery } from '../../../store/api/communication/Post.api';
import { useCreateUserPostAsyncMutation } from '../../../store/api/communication/UserPost.api';
import Post from '../Post';

const MyFeed = () => {
    const { t } = useTranslation("communication/myEnvironment/myFeed");

    const customer = useSelector((state) => state.customer.value);

    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const [getUserPosts] = useLazyUserPostSearchByUserIdQuery();
    const [getPostById] = useLazyGetPostByIdQuery();

    const postContentRef = useRef(null);

    const [showCreatePost, setShowCreatePost] = useState(false);
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

    const createPostAsync = async () => {
        const newPost = {
            content: postContentRef.current.value,
            when: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            customerId: customer?.id
        }

        const createdPost = await createNewPostAsync(newPost);
        if (createdPost.data !== undefined) {
            setShowCreatePost(false);
            postContentRef.current.value = "";

            const isCreated = await createUserPostAsync(createdPost.data.id);
            return isCreated;
        }

        return false;
    }

    const createUserPostAsync = async (postId) => {
        const newUserPost = {
            userId: customer?.id,
            postId: postId
        }

        const createdUserPost = await createNewUserPostAsync(newUserPost);
        return createdUserPost.data === undefined ? false : true;
    }

    if (customer === undefined || customer === null) {
        return <></>;
    }

    return (
        <div>
            <div className="create-post">
                <div>
                    <div className="create-post__tool" style={{ display: !showCreatePost ? "flex" : "none" }}>
                        <button type="button" className="btn btn-outline-info" onClick={() => setShowCreatePost((item) => !item)}>{t("NewPost")}</button>
                    </div>
                    <div style={{ display: showCreatePost ? "flex" : "none" }} className="create-post__create-tool">
                        <button type="button" className="btn btn-outline-warning" onClick={() => setShowCreatePost((item) => !item)}>{t("Cancel")}</button>
                        <button type="button" className="btn btn-outline-success" onClick={async () => await createPostAsync()}>{t("Create")}</button>
                    </div>
                </div>
                <textarea rows="5" cols="100" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
            </div>
            <ul className="posts">
                {allPosts?.map(post => (
                    <li key={post.id}>
                        <Post
                            customer={customer}
                            postId={post.id}
                            deletePostAsync={null}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default MyFeed;