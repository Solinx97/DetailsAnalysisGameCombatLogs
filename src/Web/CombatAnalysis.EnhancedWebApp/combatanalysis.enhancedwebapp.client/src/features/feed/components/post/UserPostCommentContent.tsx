import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateUserPostCommentMutation } from '../../api/UserPostComment.api';
import type { UserPostCommentModel } from '../../types/UserPostCommentModel';

interface UserPostCommentContentProps {
    userId: string;
    comment: UserPostCommentModel;
}

const UserPostCommentContent: React.FC<UserPostCommentContentProps> = ({ userId, comment }) => {
    const { t } = useTranslation('communication/postCommentContent');

    const [updatePostComment] = useUpdateUserPostCommentMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef<HTMLTextAreaElement | null>(null);

    const updatePostCommentAsync = async () => {
        try {
            if (!commentContent.current) {
                return;
            }

            const postCommentForUpdate = Object.assign({}, comment);
            postCommentForUpdate.content = commentContent.current?.value;

            await updatePostComment({ id: postCommentForUpdate.id, comment: postCommentForUpdate }).unwrap();
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

export default UserPostCommentContent;