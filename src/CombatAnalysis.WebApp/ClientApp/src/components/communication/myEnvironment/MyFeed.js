import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import PostService from '../../../services/PostService';
import UserPostService from '../../../services/UserPostService';
import Alert from '../../Alert';
import Post from '../Post';

const MyFeed = () => {
    const userPostService = new UserPostService();
    const postService = new PostService();

    const customer = useSelector((state) => state.customer.value);

    const [feed, setFeed] = useState(<></>);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(true);

    useEffect(() => {
        if (customer === null) {
            return;
        }

        let getPosts = async () => {
            await getPostsAsync();
        }

        getPosts();
    }, [])

    const getUserPostsAsync = async (customersId) => {
        let userPosts = [];
        for (let i = 0; i < customersId.length; i++) {
            const response = await userPostService.searchByUserId(customersId[i]);
            if (response.status !== 200) {
                return [];
            }

            const result = await response.json();
            userPosts = [...userPosts, ...result];
        }

        return userPosts;
    }

    const getPostsAsync = async () => {
        const customersId = [customer.id];

        const userPosts = await getUserPostsAsync(customersId);
        setFeed(<Post
            selectedPostsType={userPosts}
            createPostAsync={createPostAsync}
            updatePostsAsync={getPostsAsync}
            setShowUpdatingAlert={setShowUpdatingAlert}
        />);
    }

    const createPostAsync = async (postContent) => {
        const newPost = {
            id: 0,
            content: postContent,
            when: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            ownerId: customer.id
        }

        const createdPost = await postService.createAsync(newPost);
        if (createdPost !== null) {
            const isCreated = await createUserPostAsync(createdPost.id);
            return isCreated;
        }

        return false;
    }

    const createUserPostAsync = async (postId) => {
        const newUserPost = {
            userId: customer.id,
            postId: postId
        }

        const createdUserPost = await userPostService.createAsync(newUserPost);
        return createdUserPost === null ? false : true;
    }

    const render = () => {
        return (
            <div>
                <Alert
                    isVisible={showUpdatingAlert}
                    content="Updating..."
                />
                {feed}
            </div>
        )
    }

    return render();
}

export default MyFeed;