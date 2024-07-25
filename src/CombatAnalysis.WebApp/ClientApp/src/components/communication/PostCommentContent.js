import { faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdatePostCommentAsyncMutation } from '../../store/api/communication/PostComment.api';

const PostCommentContent = ({ userId, comment }) => {
    const { t } = useTranslation("communication/postCommentContent");

    const [updatePostCommentAsyncMut] = useUpdatePostCommentAsyncMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef(null);

    const updatePostCommentAsync = async () => {
        const postCommentForUpdate = Object.assign({}, comment);
        postCommentForUpdate.content = commentContent.current.value;

        const updatedItem = await updatePostCommentAsyncMut(postCommentForUpdate);
        if (updatedItem.data !== undefined) {
            setEditModeOne(false);
        }
    }

    return (
        <div className="post-comments__content">
            {editModeOn
                ? <div>
                    <textarea className="form-control" rows="4" cols="65" ref={commentContent} defaultValue={comment.content} />
                    <div className="actions">
                        <div className="save" onClick={async () => await updatePostCommentAsync()}>{t("Save")}</div>
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