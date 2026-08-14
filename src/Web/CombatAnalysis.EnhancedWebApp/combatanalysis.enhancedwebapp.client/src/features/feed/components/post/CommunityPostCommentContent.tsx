import logger from '@/utils/Logger';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';

interface CommunityPostCommentContentProps {
    userId: string;
    comment: CommunityPostCommentModel;
}

const CommunityPostCommentContent: React.FC<CommunityPostCommentContentProps> = ({ userId, comment }) => {
    const { t } = useTranslation('communication/postCommentContent');

    const [updatePostComment] = useUpdateCommunityPostCommentMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef<HTMLTextAreaElement | null>(null);

    const updatePostCommentAsync = async () => {
        try {
            const postCommentForUpdate = Object.assign({}, comment);
            postCommentForUpdate.content = commentContent.current?.value ?? "";

            await updatePostComment({ id: postCommentForUpdate.id, comment: postCommentForUpdate }).unwrap();
            setEditModeOne(false);
        } catch (e) {
            logger.error("Failed to update community post comment", e);
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

export default CommunityPostCommentContent;