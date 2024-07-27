import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useCreatePostAsyncMutation } from '../../store/api/communication/Post.api';
import Loading from '../Loading';
import AddTagsToPost from './AddTagsToPost';

const postType = {
    user: 0,
    community: 1
}

const CreatePost = ({ user, owner, postTypeName, createTypeOfPostFunc, t }) => {
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
            appUserId: user?.id
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

    if (user === null) {
        return (<Loading />);
    }

    return (
        <div className="create-post">
            <div className="create-post__tool">
                {!showCreatePost &&
                    <div className="open-create-post container">
                        <div className="btn-shadow" title={t("NewPost")} onClick={() => setShowCreatePost((item) => !item)}>
                            <FontAwesomeIcon
                                icon={faPlus}
                            />
                            <div>{t("Create")}</div>
                        </div>
                    </div>
                }
                {showCreatePost &&
                    <div className="finish-create-post">
                        <div className={`btn-shadow${postContent === "" ? "_disabled" : ""}`} title={t("Save")} onClick={postContent === "" ? null : async () => await createPostAsync()}>
                            <FontAwesomeIcon
                                icon={faCheck}
                            />
                            <div>{t("Save")}</div>
                        </div>
                        <div className="btn-shadow" title={t("Cancel")} onClick={() => setShowCreatePost((item) => !item)}>
                            <FontAwesomeIcon
                                icon={faBan}
                            />
                            <div>{t("Cancel")}</div>
                        </div>
                    </div>
                }
            </div>
            {showCreatePost &&
                <div className="create-post__input-area">
                    <AddTagsToPost
                        postTags={postTags}
                        setPostTags={setPostTags}
                        t={t}
                    />
                    <textarea className="form-control" rows="5" title={t("PostContent")} onChange={postContentHandle} />
                </div>
            }
        </div>
    );
}

export default CreatePost;