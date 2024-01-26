import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useCreatePostAsyncMutation } from '../../store/api/communication/Post.api';
import { useCreateUserPostAsyncMutation } from '../../store/api/communication/UserPost.api';
import CommunicationMenu from './CommunicationMenu';
import FeedParticipants from './FeedParticipants';

const postType = {
    user: 0,
    community: 1
}

const Feed = () => {
    const { t } = useTranslation("communication/feed");

    const customer = useSelector((state) => state.customer.value);

    const [createNewPostAsync] = useCreatePostAsyncMutation();
    const [createNewUserPostAsync] = useCreateUserPostAsyncMutation();

    const postContentRef = useRef(null);

    const [showCreatePost, setShowCreatePost] = useState(false);

    const createPostAsync = async () => {
        const newPost = {
            owner: customer.username,
            content: postContentRef.current.value,
            postType: postType["user"],
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
                <div className="create-post">
                    <div className="create-post__tool">
                        <div className="open-create-post container" style={{ display: !showCreatePost ? "flex" : "none" }}>
                            <div className="open-create-post">
                                <div className="btn-shadow" title={t("NewPost")} onClick={() => setShowCreatePost((item) => !item)}>
                                    <FontAwesomeIcon
                                        icon={faPlus}
                                    />
                                    <div>{t("Create")}</div>
                                </div>
                            </div>
                        </div>
                        <div style={{ display: showCreatePost ? "flex" : "none" }} className="finish-create-post">
                            <div className="btn-shadow" onClick={async () => await createPostAsync()}>
                                <FontAwesomeIcon
                                    icon={faCheck}
                                />
                                <div>{t("Create")}</div>
                            </div>
                            <div className="btn-shadow" onClick={() => setShowCreatePost((item) => !item)}>
                                <FontAwesomeIcon
                                    icon={faBan}
                                />
                                <div>{t("Cancel")}</div>
                            </div>
                        </div>
                    </div>
                    <textarea className="form-control" rows="5" ref={postContentRef} style={{ display: showCreatePost ? "flex" : "none" }} />
                </div>
                <FeedParticipants
                    customer={customer}
                />
            </div>
        </>
    );
}

export default Feed;