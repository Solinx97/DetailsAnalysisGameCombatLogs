import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreatePostAsyncMutation } from '../../store/api/communication/Post.api';
import Loading from '../Loading';
import AddTagsToPost from './AddTagsToPost';

const postType = {
    user: 0,
    community: 1
}

const CreatePost = ({ customer, owner, postTypeName, createTypeOfPostFunc }) => {
    const { t } = useTranslation("communication/feed");

    const [showCreatePost, setShowCreatePost] = useState(false);
    const [postContent, setPostContent] = useState("");
    const [postTags, setPostTags] = useState([]);

    const [createNewPostAsync] = useCreatePostAsyncMutation();

    const createPostAsync = async () => {
        const newPost = {
            owner: owner,
            content: postContent,
            postType: postType[postTypeName],
            tags: postTags.join(';'),
            when: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            customerId: customer?.id
        }

        const createdPost = await createNewPostAsync(newPost);
        if (createdPost.data !== undefined) {
            setShowCreatePost(false);
            setPostContent("");

            const isCreated = await createTypeOfPostFunc(createdPost.data.id);
            return isCreated;
        }

        return false;
    }

    const postContentHandle = (e) => {
        setPostContent(e.target.value);
    }

    if (customer === null) {
        return (<Loading />);
    }

    return (
        <div className="create-post">
            <div className="create-post__tool">
                <div className="open-create-post container" style={{ display: !showCreatePost ? "flex" : "none" }}>
                    <div className="btn-shadow" title={t("NewPost")} onClick={() => setShowCreatePost((item) => !item)}>
                        <FontAwesomeIcon
                            icon={faPlus}
                        />
                        <div>{t("Create")}</div>
                    </div>
                </div>
                <div style={{ display: showCreatePost ? "flex" : "none" }} className="finish-create-post">
                    <div className={`btn-shadow${postContent === "" ? "_disabled" : ""}`} onClick={postContent === "" ? null : async () => await createPostAsync()}>
                        <FontAwesomeIcon
                            icon={faCheck}
                        />
                        <div>{t("Save")}</div>
                    </div>
                    <div className="btn-shadow" onClick={() => setShowCreatePost((item) => !item)}>
                        <FontAwesomeIcon
                            icon={faBan}
                        />
                        <div>{t("Cancel")}</div>
                    </div>
                </div>
            </div>
            {showCreatePost &&
                <div className="create-post__input-area">
                    <AddTagsToPost
                        postTags={postTags}
                        setPostTags={setPostTags}
                        t={t}
                    />
                    <textarea className="form-control" rows="5" onChange={postContentHandle} />
                </div>
            }
        </div>
    );
}

export default CreatePost;