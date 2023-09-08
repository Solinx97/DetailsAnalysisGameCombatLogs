import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreatePostAsyncMutation } from '../../store/api/communication/Post.api';
import { useCreateUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import Communication from './Communication';
import FeedParticipants from './FeedParticipants';

const Feed = () => {
    const { t } = useTranslation("communication/feed");

    const customer = useSelector((state) => state.customer.value);

    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const postContentRef = useRef(null);

    const [showCreatePost, setShowCreatePost] = useState(false);

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

    if (customer === null) {
        return <></>;
    }

    return (
        <>
            <Communication
                currentMenuItem={0}
            />
            <div className="communication__content">
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
                <FeedParticipants
                    customer={customer}
                />
            </div>
        </>
    );
}

export default Feed;