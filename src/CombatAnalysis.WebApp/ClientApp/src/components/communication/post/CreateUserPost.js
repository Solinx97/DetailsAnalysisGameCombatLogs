import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useCreateUserPostMutation } from '../../../store/api/communication/UserPost.api';
import Loading from '../../Loading';
import AddTagsToPost from './AddTagsToPost';

const CreateUserPost = ({ user, owner, t }) => {
    const [showCreatePost, setShowCreatePost] = useState(false);
    const [postContent, setPostContent] = useState("");
    const [postTags, setPostTags] = useState([]);

    const [createNewUserPostAsync] = useCreateUserPostMutation();

    const createUserPostAsync = async () => {
        const newPost = {
            owner: owner,
            content: postContent,
            publicType: 0,
            tags: postTags.join(';'),
            createdAt: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            appUserId: user?.id
        }

        const response = await createNewUserPostAsync(newPost);
        if (response.data) {
            setShowCreatePost(false);
            setPostContent("");

            return true;
        }

        return false;
    }

    const postContentHandle = (e) => {
        setPostContent(e.target.value);
    }

    if (!user) {
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
                        <div className={`btn-shadow${postContent === "" ? "_disabled" : ""}`} title={t("Save")} onClick={postContent === "" ? null : async () => await createUserPostAsync()}>
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

export default CreateUserPost;