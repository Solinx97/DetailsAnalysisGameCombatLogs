import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateUserPostCommentMutation } from '../../api/UserPostComment.api';
import type { useUpdateCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommentModel } from '../../types/CommentModel';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';
import type { UserPostCommentModel } from '../../types/UserPostCommentModel';

interface UserPostCommentContentProps {
    userId: string;
    comment: CommentModel;
    updateUserPostComment?: ReturnType<
        typeof useUpdateUserPostCommentMutation
    >[0];
    updateCommunityPostComment?: ReturnType<
        typeof useUpdateCommunityPostCommentMutation
    >[0];
}

const PostCommentContent: React.FC<UserPostCommentContentProps> = ({ userId, comment, updateUserPostComment, updateCommunityPostComment }) => {
    const { t } = useTranslation('communication/postCommentContent');

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef<HTMLTextAreaElement | null>(null);

    const isCommunityPostComment = (comment: CommentModel): comment is CommunityPostCommentModel => {
        return "communityId" in comment;
    }

    const isUserPostComment = (comment: CommentModel): comment is UserPostCommentModel => {
        return "userPostId" in comment;
    }

    const updatePostCommentAsync = async () => {
        try {
            if (!commentContent.current) {
                return;
            }

            if (isCommunityPostComment(comment) && updateCommunityPostComment) {
                const postCommentForUpdate = Object.assign({}, comment);
                postCommentForUpdate.content = commentContent.current?.value;

                await updateCommunityPostComment({ id: postCommentForUpdate.id, comment: postCommentForUpdate }).unwrap();
            }
            else if (isUserPostComment(comment) && updateUserPostComment) {
                const postCommentForUpdate = Object.assign({}, comment);
                postCommentForUpdate.content = commentContent.current?.value;

                await updateUserPostComment({ id: postCommentForUpdate.id, comment: postCommentForUpdate }).unwrap();
            }

            setEditModeOne(false);
        } catch (error) {
            console.error("Failed update post comment");
        }
    }

    return (
        <div className="post-comments__content">
            {editModeOn
                ? <div>
                    <textarea className="form-control" rows={2} cols={65} ref={commentContent} defaultValue={comment.content} />
                    <div className="actions">
                        <div className="save" onClick={updatePostCommentAsync}>{t("Save")}</div>
                        <div className="cancel" onClick={() => setEditModeOne(false)}>{t("Cancel")}</div>
                    </div>
                </div>
                : <div className="card-text">{comment.content}</div>
            }
            {comment.appUserId === userId &&
                <div className="post-comments__menu">
                    <FontAwesomeIcon
                        icon={faPen}
                        title={t("Edit")}
                        className={`comment-edit${editModeOn ? "_active" : ""}`}
                        onClick={() => setEditModeOne((item) => !item)}
                    />
                </div>
            }
        </div>
    );
}

export default PostCommentContent;