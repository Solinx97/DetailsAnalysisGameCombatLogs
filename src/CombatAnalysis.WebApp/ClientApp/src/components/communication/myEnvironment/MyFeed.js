import { useEffect, useState } from "react";
import PostService from '../../../services/PostService';
import UserPostService from '../../../services/UserPostService';
import Alert from '../../Alert';
import useAuthentificationAsync from '../../../hooks/useAuthentificationAsync';
import Post from '../Post';

const MyFeed = () => {
    const userPostService = new UserPostService();
    const postService = new PostService();

    const [, customer] = useAuthentificationAsync();

    const [feed, setFeed] = useState(<></>);
    const [showUpdatingAlert, setShowUpdatingAlert] = useState(true);

    useEffect(() => {
        let getPosts = async () => {
            await getPostsAsync();
        }

        getPosts();
    }, [])

    const getUserPostsAsync = async (customersId) => {
        let userPosts = [];
        for (let i = 0; i < customersId.length; i++) {
            const result = await userPostService.searchByUserId(customersId[i]);
            if (result === null) {
                return [];
            }

            userPosts = [...userPosts, ...result];
        }

        return userPosts;
    }

    const getPostsAsync = async () => {
        const customersId = [customer?.id];

        const userPosts = await getUserPostsAsync(customersId);

        setFeed(<Post
            customer={customer}
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
            ownerId: customer?.id
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
            userId: customer?.id,
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