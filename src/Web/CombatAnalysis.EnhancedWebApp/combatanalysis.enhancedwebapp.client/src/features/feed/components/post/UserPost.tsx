import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import { memo, useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { useCreateUserPostCommentMutation } from '../../api/UserPostComment.api';
import type { UserPostCommentModel } from '../../types/UserPostCommentModel';
import type { UserPostModel } from '../../types/UserPostModel';
import UserPostComments from './UserPostComments';
import PostReactions from './PostReactions';
import PostTitle from './PostTitle';
import { useRemoveUserPostMutation } from '../../api/UserPost.api';
import { useCreateUserPostLikeMutation } from '../../api/UserPostLike.api';
import { useCreateUserPostDislikeMutation } from '../../api/UserPostDislike.api';

import './Post.scss';

interface UserPostProps {
    user: AppUserModel;
    post: UserPostModel;
    feedVersion: number;
}

const UserPost: React.FC<UserPostProps> = ({ user, post, feedVersion }) => {
    const maxLength = 256;

    const { t } = useTranslation('communication/post');

    const maxLengthRef = useRef<number>(APP_CONFIG.communication.length.userPostCommentContentMaxLength ?? maxLength);

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [currentContentLength, setCurrentContentLength] = useState(0);
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    const [createPostComment] = useCreateUserPostCommentMutation();
    const [removeUserPost] = useRemoveUserPostMutation();
    const [createPostLike] = useCreateUserPostLikeMutation();
    const [createPostDislike] = useCreateUserPostDislikeMutation();

    useEffect(() => {
        setIsMyPost(post.appUserId === user.id);
    }, [post]);

    const createUserPostCommentAsync = async () => {
        try {
            const userPostComment: UserPostCommentModel = {
                id: 0,
                content: postCommentContent,
                createdAt: new Date(),
                userPostId: post.id,
                appUserId: user.id
            }

            await createPostComment({ feedVersion, comment: userPostComment }).unwrap();
            setPostCommentContent("");
            setShowAddComment(false);
            setCurrentContentLength(0);
        } catch (e) {
            logger.error("Failed to create post comment", e);
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
                    removeUserPost={removeUserPost}
                    feedVersion={feedVersion}
                    t={t}
                />
                <div className="posts__content">{post.content}</div>
                <PostReactions
                    userId={user.id}
                    post={post}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    feedVersion={feedVersion}
                    t={t}
                    useEmailVerification={false}
                    createUserPostLike={createPostLike}
                    createUserPostDislike={createPostDislike}
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
                                    <div className="add-comment" onClick={createUserPostCommentAsync}>{t("Add")}</div>
                                    <div className="hide" onClick={() => setShowAddComment((item) => !item)}>{t("Cancel")}</div>
                                </div>
                            </div>
                        }
                    </div>
                    <UserPostComments
                        userId={user.id}
                        postId={post.id}
                        feedVersion={feedVersion}
                    />
                </>
            }
        </>
    );
}

export default memo(UserPost);