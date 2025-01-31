import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from 'react';
import { useSelector } from 'react-redux';
import { useCreateCommunityPostMutation } from '../../../store/api/post/CommunityPost.api';
import Loading from '../../Loading';
import VerificationRestriction from '../../common/VerificationRestriction';
import AddTagsToPost from './AddTagsToPost';

const CreateCommunityPost = ({ user, communityName, communityId, t }) => {
    const userPrivacy = useSelector((state) => state.userPrivacy.value);

    const [showCreatePost, setShowCreatePost] = useState(false);
    const [postContent, setPostContent] = useState("");
    const [postTags, setPostTags] = useState([]);

    const [createNewCommunityPostAsync] = useCreateCommunityPostMutation();

    const createCommunityPostAsync = async () => {
        const newPost = {
            communityName: communityName,
            owner: communityName,
            content: postContent,
            postType: 0,
            publicType: 0,
            restrictions: 0,
            tags: postTags.join(';'),
            createdAt: new Date(),
            likeCount: 0,
            dislikeCount: 0,
            postComment: 0,
            communityId: communityId,
            appUserId: user?.id
        }

        const response = await createNewCommunityPostAsync(newPost);
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
                        {userPrivacy?.emailVerified
                            ? <div className="btn-shadow" title={t("NewPost")} onClick={() => setShowCreatePost((item) => !item)}>
                                <FontAwesomeIcon
                                    icon={faPlus}
                                />
                                <div>{t("Create")}</div>
                            </div>
                            : <VerificationRestriction
                                contentText={t("Create")}
                                infoText={t("VerificationCreateCommunityPost")}
                            />
                        }
                    </div>
                }
                {showCreatePost &&
                    <div className="finish-create-post">
                        <div className={`btn-shadow${postContent === "" ? "_disabled" : ""}`} title={t("Save")} onClick={postContent === "" ? null : async () => await createCommunityPostAsync()}>
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

export default CreateCommunityPost;