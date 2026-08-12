import logger from '@/utils/Logger';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import CommunityPostComments from './CommunityPostComments';
import CommunityPostReactions from './CommunityPostReactions';
import CommunityPostTitle from './CommunityPostTitle';
import type { UserFeedModel } from '../../types/UserFeedModel';

import './Post.scss';

interface CommunityPostProps {
    userId: string;
    communityId: number;
    post: CommunityPostModel | UserFeedModel | undefined;
}

const CommunityPost: React.FC<CommunityPostProps> = ({ userId, communityId, post }) => {
    const { t } = useTranslation("communication/post");

    const [createPostComment] = useCreateCommunityPostCommentMutation();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    useEffect(() => {
        setIsMyPost(post?.appUserId === userId);
    }, [post]);

    const createPostCommentAsync = async () => {
        if (!post) {
            return;
        }

        try {
            const newPostComment: CommunityPostCommentModel = {
                id: 0,
                content: postCommentContent,
                commentType: 0,
                createdAt: new Date(),
                communityPostId: post.id,
                communityId: communityId,
                appUserId: userId
            }

            await createPostComment(newPostComment).unwrap();
            setPostCommentContent("");
        } catch (e) {
            logger.error("Failed to create community post comment", e);
        }
    }

    if (!post) {
        return (<></>);
    }

    return (
        <>
            <div className="posts__card">
                <CommunityPostTitle
                    post={post}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post?.content}</div>
                <CommunityPostReactions
                    userId={userId}
                    communityId={communityId}
                    post={post}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    t={t}
                />
            </div>
            {showComments &&
                <>
                    <CommunityPostComments
                        userId={userId}
                        postId={post.id}
                    />
                    <div className="add-new-comment">
                        <div className="add-new-comment__title">
                            {showAddComment
                                ? <div>{t("AddComment")}</div>
                                : <div className="open-add-comment" onClick={() => setShowAddComment((item) => !item)}>{t("Add")}</div>
                            }
                        </div>
                        {showAddComment &&
                            <div className="add-new-comment__content">
                            <textarea className="form-control" rows={3} cols={60} onChange={e => setPostCommentContent(e.target.value)} value={postCommentContent} />
                                <div className="actions">
                                    <div className="add-comment" onClick={createPostCommentAsync}>{t("Add")}</div>
                                    <div className="hide" onClick={() => setShowAddComment((item) => !item)}>{t("Hide")}</div>
                                </div>
                            </div>
                        }
                    </div>
                </>
            }
        </>
    );
}

export default CommunityPost;