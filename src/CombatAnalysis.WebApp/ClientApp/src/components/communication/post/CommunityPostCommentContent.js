import React, { useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useUpdateCommunityPostCommentMutation } from '../../../store/api/communication/CommunityPostComment.api';

const CommunityPostCommentContent = ({ comment }) => {
    const { t } = useTranslation("communication/postCommentContent");

    const [updatePostComment] = useUpdateCommunityPostCommentMutation();

    const [editModeOn, setEditModeOne] = useState(false);

    const commentContent = useRef(null);

    const updatePostCommentAsync = async () => {
        const postCommentForUpdate = Object.assign({}, comment);
        postCommentForUpdate.content = commentContent.current.value;

        const response = await updatePostComment(postCommentForUpdate);
        if (response.data) {
            setEditModeOne(false);
        }
    }

    return (
        <div className="post-comments__content">
            {editModeOn
                ? <div>
                    <textarea className="form-control" rows="2" cols="65" ref={commentContent} defaultValue={comment.content} />
                    <div className="actions">
                        <div className="save" onClick={updatePostCommentAsync}>{t("Save")}</div>
                        <div className="cancel" onClick={() => setEditModeOne(false)}>{t("Cancel")}</div>
                    </div>
                </div>
                : <div className="card-text">{comment.content}</div>
            }
        </div>
    );
}

export default CommunityPostCommentContent;