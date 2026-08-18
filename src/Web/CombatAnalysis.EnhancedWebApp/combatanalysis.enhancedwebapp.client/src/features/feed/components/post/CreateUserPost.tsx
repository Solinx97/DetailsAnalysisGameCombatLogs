import { APP_CONFIG } from '@/config/appConfig';
import Loading from '@/shared/components/Loading';
import logger from '@/utils/Logger';
import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState, type ChangeEvent } from 'react';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useCreateUserPostMutation } from '../../api/UserPost.api';
import type { UserPostModel } from '../../types/UserPostModel';
import AddTagsToPost from './AddTagsToPost';

interface CreateUserPostProps {
    user: AppUserModel;
    feedVersion: number;
    t: (key: string) => string;
}

const CreateUserPost: React.FC<CreateUserPostProps> = ({ user, feedVersion, t }) => {
    const maxLength = 512;

    const maxLengthRef = useRef<number>(APP_CONFIG.communication.length.userPostContentMaxLength ?? maxLength);

    const [showCreatePost, setShowCreatePost] = useState(false);
    const [currentContentLength, setCurrentContentLength] = useState(0);
    const [postContent, setPostContent] = useState("");
    const [postTags, setPostTags] = useState<string[]>([]);

    const [createNewUserPostAsync] = useCreateUserPostMutation();

    const createUserPostAsync = async () => {
        try {
            if (postContent === "") {
                return;
            }

            const newPost: UserPostModel = {
                id: 0,
                content: postContent,
                publicType: 0,
                tags: postTags.join(';'),
                createdAt: new Date(),
                appUserId: user.id,
                likeCount: 0,
                dislikeCount: 0,
                commentCount: 0,
                reaction: 0
            }

            await createNewUserPostAsync({ feedVersion, post: newPost }).unwrap();

            setShowCreatePost(false);
            setPostContent("");
        } catch (e) {
            logger.error("Failed to create User post", e);
        }
    }

    const contentHandle = (e: ChangeEvent<HTMLTextAreaElement>) => {
        if (e.target.value.length > maxLengthRef.current) {
            return;
        }

        setPostContent(e.target.value);
        setCurrentContentLength(e.target.value.length);
    }

    const createPostCancel = () => {
        setPostTags([]);
        setShowCreatePost((item) => !item);
    }

    if (!user) {
        return (<Loading />);
    }

    return (
        <div className="create-post">
            <div className="create-post__tool">
                {!showCreatePost &&
                    <div className="open-create-post container">
                        <div className="btn-shadow" onClick={() => setShowCreatePost((item) => !item)}>
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
                        <div className="btn-shadow" title={t("Cancel")} onClick={createPostCancel}>
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
                    <div className={`content-length ${postContent.length === maxLengthRef.current ? 'limit' : ''}`}>{currentContentLength}/{maxLengthRef.current}</div>
                    <textarea className="form-control" rows={5} title={t("PostContent") || ""} value={postContent}
                        onChange={contentHandle} />
                </div>
            }
        </div>
    );
}

export default CreateUserPost;