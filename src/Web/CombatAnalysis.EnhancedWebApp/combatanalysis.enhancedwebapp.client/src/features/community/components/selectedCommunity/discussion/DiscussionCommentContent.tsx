import logger from '@/utils/Logger';
import { APP_CONFIG } from '@/config/appConfig';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityDiscussionCommentAsyncMutation } from '../../../api/CommunityDiscussionComment.api';
import type { CommunityDiscussionCommentModel } from '../../../types/CommunityDiscussionCommentModel';

interface DiscussionCommentContentProps {
    userId: string;
    comment: CommunityDiscussionCommentModel;
}

const DiscussionCommentContent: React.FC<DiscussionCommentContentProps> = ({ userId, comment }) => {
    const maxContentLength = 256;

    const { t } = useTranslation("communication/postCommentContent");

    const maxContentLengthRef = useRef<number>(APP_CONFIG.communication.length.communityDiscussionCommentContentMaxLength ?? maxContentLength);

    const [updateDiscussionCommentAsyncMut] = useUpdateCommunityDiscussionCommentAsyncMutation();

    const [editModeOn, setEditModeOne] = useState(false);
    const [currentContentLength, setCurrentContentLength] = useState(0);

    const commentContentRef = useRef<HTMLTextAreaElement | null>(null);

    useEffect(() => {
        setCurrentContentLength(comment.content.length);
    }, [comment, editModeOn]);

    const updateDiscussionCommentAsync = async () => {
        try {
            if (!commentContentRef.current) {
                return;
            }

            const postCommentForUpdate = Object.assign({}, comment);
            postCommentForUpdate.content = commentContentRef.current.value;

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
                    <div className={`content-length ${commentContentRef.current?.value.length === maxContentLengthRef.current ? 'limit' : ''}`}>{currentContentLength}/{maxContentLengthRef.current}</div>
                    <textarea className="form-control" rows={3} cols={50} ref={commentContentRef} maxLength={maxContentLengthRef.current}
                        onChange={e => setCurrentContentLength(e.target.value.length)} defaultValue={comment.content} />
                    <div className="actions">
                        <div className="btn-shadow create" onClick={async () => await updateDiscussionCommentAsync()}>{t("Save")}</div>
                        <div className="btn-shadow" onClick={() => setEditModeOne(false)}>{t("Cancel")}</div>
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