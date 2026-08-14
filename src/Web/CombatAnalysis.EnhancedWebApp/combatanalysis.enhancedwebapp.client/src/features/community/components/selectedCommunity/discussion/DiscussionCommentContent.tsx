import logger from '@/utils/Logger';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityDiscussionCommentAsyncMutation } from '../../../api/CommunityDiscussionComment.api';
import type { CommunityDiscussionCommentModel } from '../../../types/CommunityDiscussionCommentModel';

interface DiscussionCommentContentProps {
    userId: string;
    comment: CommunityDiscussionCommentModel;
}

const DiscussionCommentContent: React.FC<DiscussionCommentContentProps> = ({ userId, comment }) => {
    const { t } = useTranslation("communication/postCommentContent");

    const [updateDiscussionCommentAsyncMut] = useUpdateCommunityDiscussionCommentAsyncMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef<HTMLTextAreaElement | null>(null);

    const updateDiscussionCommentAsync = async () => {
        try {
            if (!commentContent.current) {
                return;
            }

            const postCommentForUpdate = Object.assign({}, comment);
            postCommentForUpdate.content = commentContent.current.value;

            await updateDiscussionCommentAsyncMut({ id: postCommentForUpdate.id, comment: postCommentForUpdate }).unwrap();
            setEditModeOne(false);
        } catch (error) {
            logger.error("Failed to update community discussion comment", error);
        }
    }

    return (
        <div className="post-comments__content">
            {editModeOn
                ? <div>
                    <textarea className="form-control" rows={3} cols={50} ref={commentContent} defaultValue={comment.content} />
                    <div className="actions">
                        <button type="button" className="btn btn-outline-info" onClick={async () => await updateDiscussionCommentAsync()}>{t("Save")}</button>
                        <button type="button" className="btn btn-secondary" onClick={() => setEditModeOne(false)}>{t("Cancel")}</button>
                    </div>
                </div>
                : <div className="card-text">{comment.content}</div>
            }
            {comment.appUserId === userId &&
                <div className="post-comments__menu">
                    <FontAwesomeIcon
                        icon={faPen}
                        title={t("Edit")}
                        onClick={() => setEditModeOne((item) => !item)}
                    />
                </div>
            }
        </div>
    );
}

export default DiscussionCommentContent;