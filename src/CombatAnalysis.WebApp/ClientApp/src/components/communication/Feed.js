import { faArrowsRotate } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import useAuthentificationAsync from '../../hooks/useAuthentificationAsync';
import FeedParticipants from './FeedParticipants';
import { useCreatePostAsyncMutation } from '../../store/api/Post.api';
import { useCreateUserPostAsyncMutation  } from '../../store/api/UserPost.api';

const Feed = () => {
    const [, customer] = useAuthentificationAsync();

    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const postContentRef = useRef(null);

    const [showCreatePost, setShowCreatePost] = useState(false);

    const createPostAsync = async () => {
        const newPost = {
            content: postContentRef.current.value,
            when: new Date(),
            likeCount: '',
            dislikeCount: 0,
            postComment: 0,
            ownerId: customer?.id
        }

        const createdPost = await createNewPostAsync(newPost);
        if (createdPost.data !== undefined) {
            const isCreated = await createUserPostAsync(createdPost.data?.id);
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

    return (<div>
        <div className="create-post">
            <div>
                <div className="create-post__tool" style={{ display: !showCreatePost ? "flex" : "none" }}>
                    <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" />
                    <button type="button" className="btn btn-outline-info" onClick={() => setShowCreatePost((item) => !item)}>New post</button>
                </div>
                <div style={{ display: showCreatePost ? "flex" : "none" }} className="create-post__create-tool">
                    <FontAwesomeIcon icon={faArrowsRotate} title="Refresh" />
                    <button type="button" className="btn btn-outline-warning" onClick={() => setShowCreatePost((item) => !item)}>Cancel</button>
                    <button type="button" className="btn btn-outline-success" onClick={async () => await createPostAsync()}>Create</button>
                </div>
            </div>
            <textarea rows="5" cols="100" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
        </div>
        <FeedParticipants customer={customer} />
    </div>);
}

export default Feed;