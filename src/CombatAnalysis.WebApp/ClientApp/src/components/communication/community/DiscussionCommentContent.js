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
        <li className="post-comments__edit list-group-item">
            {editModeOn
                ? <div>
                    <textarea rows="1" cols="50" ref={commentContent} defaultValue={comment.content} />
                    <div>
                        <button type="button" className="btn btn-outline-info" onClick={async () => await updateDiscussionCommentAsync()}>{t("Save")}</button>
                        <button type="button" className="btn btn-outline-warning" onClick={() => setEditModeOne(false)}>{t("Cancel")}</button>
                    </div>
                </div>
                : <div className="card-text">{comment.content}</div>
            }
            {comment.customerId === customerId &&
                < FontAwesomeIcon
                    icon={faPen}
                    title={t("Edit")}
                    onClick={() => setEditModeOne((item) => !item)}
                />
            }
        </li>
    );
}

export default DiscussionCommentContent;