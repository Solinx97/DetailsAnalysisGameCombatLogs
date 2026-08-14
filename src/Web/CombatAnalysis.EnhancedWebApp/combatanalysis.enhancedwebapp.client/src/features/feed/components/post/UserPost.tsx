import useFormatting from '@/shared/hooks/useFormatting';
import logger from '@/utils/Logger';
import { memo, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import type { AppUserModel } from '../../../user/types/AppUserModel';
import { useCreateUserPostCommentMutation } from '../../api/UserPostComment.api';
import type { UserPostCommentModel } from '../../types/UserPostCommentModel';
import type { UserPostModel } from '../../types/UserPostModel';
import UserPostComments from './UserPostComments';
import UserPostReactions from './UserPostReactions';
import UserPostTitle from './UserPostTitle';

import './Post.scss';

interface UserPostProps {
    myself: AppUserModel;
    post: UserPostModel;
}

const UserPost: React.FC<UserPostProps> = ({ myself, post }) => {
    const { t } = useTranslation('communication/post');

    const [createPostComment] = useCreateUserPostCommentMutation();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    const { dateFormatting } = useFormatting();

    useEffect(() => {
        setIsMyPost(post?.appUserId === myself.id);
    }, [post]);

    const createUserPostCommentAsync = async () => {
        try {
            const userPostComment: UserPostCommentModel = {
                id: 0,
                content: postCommentContent,
                createdAt: new Date(),
                userPostId: post.id,
                appUserId: myself.id
            }

            await createPostComment(userPostComment).unwrap();
            setPostCommentContent("");
        } catch (error) {
            logger.error("Failed to create user post comment");
        }
    }

    return (
        <>
            <div className="posts__card">
                <UserPostTitle
                    post={post}
                    dateFormatting={dateFormatting}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post.content}</div>
                <UserPostReactions
                    userId={myself.id}
                    post={post}
                    setShowComments={setShowComments}
                    showComments={showComments}
                />
            </div>
            {showComments &&
                <>
                    <UserPostComments
                        dateFormatting={dateFormatting}
                        userId={myself.id}
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
                                    <div className="add-comment" onClick={createUserPostCommentAsync}>{t("Add")}</div>
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

export default memo(UserPost);