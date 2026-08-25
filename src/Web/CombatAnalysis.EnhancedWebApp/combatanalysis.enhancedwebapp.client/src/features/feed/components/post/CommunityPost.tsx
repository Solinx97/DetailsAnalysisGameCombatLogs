import logger from '@/utils/Logger';
import { APP_CONFIG } from '@/config/appConfig';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import { useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import CommunityPostComments from './CommunityPostComments';
import PostTitle from './PostTitle';
import type { UserFeedModel } from '../../types/UserFeedModel';
import { useRemoveCommunityPostMutation } from '../../api/CommunityPost.api';
import { useCreateCommunityPostLikeMutation } from '../../api/CommunityPostLike.api';
import { useCreateCommunityPostDislikeMutation } from '../../api/CommunityPostDislike.api';
import PostReactions from './PostReactions';

import './Post.scss';

interface CommunityPostProps {
    user: AppUserModel;
    communityId: number;
    post: CommunityPostModel | UserFeedModel;
    feedVersion: number;
}

const CommunityPost: React.FC<CommunityPostProps> = ({ user, communityId, post, feedVersion }) => {
    const maxLength = 256;

    const { t } = useTranslation('communication/post');

    const maxLengthRef = useRef<number>(APP_CONFIG.communication.length.communityPostCommentContentMaxLength ?? maxLength);

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [currentContentLength, setCurrentContentLength] = useState(0);
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    const [createPostComment] = useCreateCommunityPostCommentMutation();
    const [removeCommunityPost] = useRemoveCommunityPostMutation();
    const [createPostLike] = useCreateCommunityPostLikeMutation();
    const [createPostDislike] = useCreateCommunityPostDislikeMutation();

    useEffect(() => {
        setIsMyPost(post.appUserId === user.id);
    }, [post]);

    const createPostCommentAsync = async () => {
        try {
            const newPostComment: CommunityPostCommentModel = {
                id: 0,
                content: postCommentContent,
                commentType: 0,
                createdAt: new Date(),
                communityPostId: post.id,
                communityId: communityId,
                appUserId: user.id
            }

            await createPostComment({ feedVersion, comment: newPostComment }).unwrap();
            setPostCommentContent("");
            setShowAddComment(false);
            setCurrentContentLength(0);
        } catch (e) {
            logger.error("Failed to create community post comment", e);
        }
    }

    const contentHandle = (e: ChangeEvent<HTMLTextAreaElement>) => {
        setPostCommentContent(e.target.value);
        setCurrentContentLength(e.target.value.length);
    }

    return (
        <>
            <div className="posts__card">
                <PostTitle
                    user={user}
                    post={post}
                    isMyPost={isMyPost}
                    removeCommunityPost={removeCommunityPost}
                    feedVersion={feedVersion}
                    t={t}
                />
                <div className="posts__content">{post?.content}</div>
                <PostReactions
                    userId={user.id}
                    post={post}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    feedVersion={feedVersion}
                    t={t}
                    useEmailVerification={true}
                    createCommunityPostLike={createPostLike}
                    createCommunityPostDislike={createPostDislike}
                />
            </div>
            {showComments &&
                <>
                    <div className="add-new-comment">
                        <div className="add-new-comment__title">
                            {showAddComment
                                ? <div className="info">
                                    <div>{t("AddComment")}</div>
                                    <div className={`content-length ${postCommentContent.length === maxLengthRef.current ? 'limit' : ''}`}>{currentContentLength}/{maxLengthRef.current}</div>
                                </div>
                                : <div className="open-add-comment" onClick={() => setShowAddComment((item) => !item)}>{t("Add")}</div>
                            }
                        </div>
                        {showAddComment &&
                            <div className="add-new-comment__content">
                                <textarea className="form-control" rows={3} cols={60} value={postCommentContent} maxLength={maxLengthRef.current}
                                    onChange={contentHandle} />
                                <div className="actions">
                                    <div className="add-comment" onClick={createPostCommentAsync}>{t("Add")}</div>
                                    <div className="hide" onClick={() => setShowAddComment((item) => !item)}>{t("Cancel")}</div>
                                </div>
                            </div>
                        }
                    </div>
                    <CommunityPostComments
                        userId={user.id}
                        postId={post.id}
                        feedVersion={feedVersion}
                    />
                </>
            }
        </>
    );
}

export default CommunityPost;