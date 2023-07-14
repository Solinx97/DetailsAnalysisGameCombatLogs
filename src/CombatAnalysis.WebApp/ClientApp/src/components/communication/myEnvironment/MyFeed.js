import { useEffect, useState } from "react";
import { useSelector } from 'react-redux';
import Post from '../Post';
import Alert from '../../Alert';

const MyFeed = () => {
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
            const response = await fetch(`/api/v1/UserPost/searchByUserId/${customersId[i]}`);
            if (response.status !== 200) {
                return [];
            }

            userPosts = await response.json();
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

        const response = await fetch("/api/v1/Post", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newPost)
        });

        if (response.status === 200) {
            const createdPost = await response.json();
            const isCreated = await createUserPostAsync(createdPost.id);
            return isCreated;
        }

        return false;
    }

    const createUserPostAsync = async (postId) => {
        const newUserPost = {
            id: 0,
            userId: customer.id,
            postId: postId
        }

        const response = await fetch("/api/v1/UserPost", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newUserPost)
        });

        if (response.status === 200) {
            return true;
        }

        return false;
    }

    const render = () => {
        return (<div>
            <Alert
                isVisible={showUpdatingAlert}
                content="Updating..."
            />
            {feed}
        </div>)
    }

    return render();
}

export default MyFeed;