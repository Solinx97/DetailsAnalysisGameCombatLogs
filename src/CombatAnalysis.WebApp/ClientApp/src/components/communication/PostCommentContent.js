import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdatePostCommentAsyncMutation } from '../../store/api/PostComment.api';

const PostCommentContent = ({ customerId, comment }) => {
    const { t } = useTranslation("communication/postCommentContent");

    const [updatePostCommentAsyncMut] = useUpdatePostCommentAsyncMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef(null);

    const updatePostCommentAsync = async (postComment) => {
        const postCommentForUpdate = Object.assign({}, postComment);
        postCommentForUpdate.content = commentContent.current.value;

        const updatedItem = await updatePostCommentAsyncMut(postCommentForUpdate);
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
                        <button type="button" className="btn btn-outline-info" onClick={async () => await updatePostCommentAsync(comment)}>{t("Save")}</button>
                        <button type="button" className="btn btn-outline-warning" onClick={() => setEditModeOne(false)}>{t("Cancel")}</button>
                    </div>
                </div>
                : <div className="card-text">{comment.content}</div>
            }
            {comment.ownerId === customerId &&
                < FontAwesomeIcon
                    icon={faPen}
                    title={t("Edit")}
                    onClick={() => setEditModeOne((item) => !item)}
                />
            }
        </li>
    );
}

export default PostCommentContent;