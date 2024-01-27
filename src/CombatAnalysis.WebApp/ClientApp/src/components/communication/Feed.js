import { useState } from 'react';
import { useSelector } from 'react-redux';
import { useCreateUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import CommunicationMenu from './CommunicationMenu';
import CreatePost from './CreatePost';
import FeedParticipants from './FeedParticipants';

const Feed = () => {
    const customer = useSelector((state) => state.customer.value);

    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const [showNewPostsInform, setShowNewPostsInform] = useState(false);
    const [showNewPosts, setShowNewPosts] = useState(false);

    const createUserPostAsync = async (postId) => {
        const newUserPost = {
            userId: customer?.id,
            postId: postId
        }

        const createdUserPost = await createNewUserPostAsync(newUserPost);
        return createdUserPost.data === undefined ? false : true;
    }

    const showNewPostsHandle = () => {
        setShowNewPosts(true);
        setShowNewPostsInform(false);
    }

    if (customer === null) {
        return (
            <CommunicationMenu
                currentMenuItem={0}
            />
        );
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={0}
            />
            <div className="communication__content">
                <div className="new-posts"
                    style={{ display: showNewPostsInform ? "flex" : "none" }}
                    onClick={showNewPostsHandle}>
                    <div className="new-posts__content">New posts</div>
                </div>
                <CreatePost
                    customer={customer}
                    owner={customer?.username}
                    postTypeName="user"
                    createTypeOfPostFunc={createUserPostAsync}
                />
                <FeedParticipants
                    customer={customer}
                    setShowNewPostsInform={setShowNewPostsInform}
                    showNewPosts={showNewPosts}
                />
            </div>
        </>
    );
}

export default Feed;