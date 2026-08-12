import Loading from '@/shared/components/Loading';
import logger from '@/utils/Logger';
import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type ChangeEvent } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useCreateUserPostMutation } from '../../api/UserPost.api';
import type { UserPostModel } from '../../types/UserPostModel';
import AddTagsToPost from './AddTagsToPost';

interface CreateUserPostProps {
    user: AppUserModel | null;
    owner: string;
    t: (key: string) => string;
}

const CreateUserPost: React.FC<CreateUserPostProps> = ({ user, owner, t }) => {
    const [showCreatePost, setShowCreatePost] = useState(false);
    const [postContent, setPostContent] = useState("");
    const [postTags, setPostTags] = useState<string[]>([]);

    const [createNewUserPostAsync] = useCreateUserPostMutation();

    const createUserPostAsync = async () => {
        try {
            if (!user || postContent === "") {
                return;
            }

            const newPost: UserPostModel = {
                id: 0,
                owner: owner,
                content: postContent,
                publicType: 0,
                tags: postTags.join(';'),
                createdAt: new Date(),
                appUserId: user.id
            }

            await createNewUserPostAsync(newPost).unwrap();

            setShowCreatePost(false);
            setPostContent("");
        } catch (e) {
            logger.error("Failed to create User post", e);
        }
    }

    const postContentHandle = (e: ChangeEvent<HTMLTextAreaElement> | undefined) => {
        setPostContent(e?.target.value ?? "");
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
                        <div className={`btn-shadow${postContent === "" ? "_disabled" : ""}`} title={t("Save")} onClick={createUserPostAsync}>
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
                    <textarea className="form-control" rows={5} title={t("PostContent") || ""} onChange={postContentHandle} />
                </div>
            }
        </div>
    );
}

export default CreateUserPost;