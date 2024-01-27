import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreatePostAsyncMutation } from '../../store/api/communication/Post.api';

const postType = {
    user: 0,
    community: 1
}

const CreatePost = ({ customer, owner, postTypeName, createTypeOfPostFunc }) => {
    const { t } = useTranslation("communication/feed");

    const [showCreatePost, setShowCreatePost] = useState(false);

    const [createNewPostAsync] = useCreatePostAsyncMutation();

    const postContentRef = useRef(null);

    const createPostAsync = async () => {
        const newPost = {
            owner: owner,
            content: postContentRef.current.value,
            postType: postType[postTypeName],
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

            const isCreated = await createTypeOfPostFunc(createdPost.data.id);
            return isCreated;
        }

        return false;
    }

    if (customer === null) {
        return <div>Loading...</div>;
    }

    return (
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
    );
}

export default CreatePost;