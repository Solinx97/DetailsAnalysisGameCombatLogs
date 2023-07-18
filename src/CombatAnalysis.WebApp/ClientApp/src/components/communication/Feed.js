import { useEffect, useState } from "react";
import useAuthentificationAsync from '../../hooks/useAuthentificationAsync';
import FriendService from '../../services/FriendService';
import PostService from "../../services/PostService";
import UserPostService from '../../services/UserPostService';
import Alert from '../Alert';
import Post from './Post';

const Feed = () => {
    const friendService = new FriendService();
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
    }, [customer])

    const getFriendsIdByUserIdAsync = async () => {
        const customersId = [];

        const friends = await friendService.searchByUserId(customer?.id);
        if (friends === null) {
            return [];
        }

        for (let i = 0; i < friends.length; i++) {
            const customerId = friends[i].whoFriendId === customer?.id ? friends[i].forWhomId : friends[i].whoFriendId;
            customersId.push(customerId);
        }

        return customersId;
    }

    const getUserPostsAsync = async (customersId) => {
        let allUserPosts = [];
        for (let i = 0; i < customersId.length; i++) {
            const userPosts = await userPostService.searchByUserId(customersId[i]);
            if (userPosts !== null) {
                allUserPosts = [...allUserPosts, ...userPosts];
            }
        }

        return allUserPosts;
    }

    const getPostsAsync = async () => {
        const customersId = await getFriendsIdByUserIdAsync();
        customersId.push(customer?.id);

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
            id: 0,
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

export default Feed;