import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityDiscussionCommentAsyncMutation } from '../../../store/api/communication/community/CommunityDiscussionComment.api';

const DiscussionCommentContent = ({ customerId, comment }) => {
    const { t } = useTranslation("communication/postCommentContent");

    const [updateDiscussionCommentAsyncMut] = useUpdateCommunityDiscussionCommentAsyncMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef(null);

    const updateDiscussionCommentAsync = async () => {
        const postCommentForUpdate = Object.assign({}, comment);
        postCommentForUpdate.content = commentContent.current.value;

        const updatedItem = await updateDiscussionCommentAsyncMut(postCommentForUpdate);
        if (updatedItem.data !== undefined) {
            setEditModeOne(false);
        }
    }

    return (
        <div className="post-comments__content">
            {editModeOn
                ? <div>
                    <textarea className="form-control" rows="3" cols="50" ref={commentContent} defaultValue={comment.content} />
                    <div className="actions">
                        <button type="button" className="btn btn-outline-info" onClick={async () => await updateDiscussionCommentAsync()}>{t("Save")}</button>
                        <button type="button" className="btn btn-light" onClick={() => setEditModeOne(false)}>{t("Cancel")}</button>
                    </div>
                </div>
                : <div className="card-text">{comment.content}</div>
            }
            {comment.customerId === customerId &&
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