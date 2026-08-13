import type { RootState } from '@/app/Store';
import Loading from '@/shared/components/Loading';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import { faBan, faCheck, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, type ChangeEvent } from 'react';
import { useSelector } from 'react-redux';
import { useCreateCommunityPostMutation } from '../../../feed/api/CommunityPost.api';
import type { CommunityPostModel } from '../../../feed/types/CommunityPostModel';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import AddTagsToPost from './AddTagsToPost';

interface CreateCommunityPostProps {
    user: AppUserModel | null;
    communityName: string;
    communityId: number;
    t: (key: string) => string;
}

const CreateCommunityPost: React.FC<CreateCommunityPostProps> = ({ user, communityName, communityId, t }) => {
    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const [showCreatePost, setShowCreatePost] = useState(false);
    const [postContent, setPostContent] = useState("");
    const [postTags, setPostTags] = useState<string[]>([]);

    const [createNewCommunityPostAsync] = useCreateCommunityPostMutation();

    const createCommunityPostAsync = async () => {
        try {
            const newPost: CommunityPostModel = {
                id: 0,
                communityName: communityName,
                owner: communityName,
                content: postContent,
                postType: 0,
                publicType: 0,
                restrictions: 0,
                tags: postTags.join(';'),
                createdAt: new Date(),
                appUserId: user?.id ?? "",
                communityId: communityId,
                likeCount: 0,
                dislikeCount: 0,
                commentCount: 0
            }

            await createNewCommunityPostAsync(newPost).unwrap();
            setShowCreatePost(false);
            setPostContent("");
        } catch (error) {
            console.error("Failed to create community post");
        }
    }

    const postContentHandle = (e: ChangeEvent<HTMLTextAreaElement>) => {
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
                        <div className={`btn-shadow${postContent === "" ? "_disabled" : ""}`} title={t("Save")}
                            onClick={postContent === "" ? () => { } : createCommunityPostAsync}>
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
                    <textarea className="form-control" rows={5} title={t("PostContent")}
                        onChange={postContentHandle} />
                </div>
            }
        </div>
    );
}

export default CreateCommunityPost;