import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../store/api/Account.api';
import { useRemovePostCommentAsyncMutation } from '../../store/api/communication/PostComment.api';

const PostCommentTitle = ({ userId, comment, dateFormatting, postId, updatePostAsync }) => {
    const { t } = useTranslation("communication/postCommentTitle");

    const { data: targetUser, isLoading } = useGetUserByIdQuery(comment?.appUserId);

    const [removePostCommentAsyncMut] = useRemovePostCommentAsyncMutation();

    const deletePostCommentAsync = async (postCommentId) => {
        const deletedItem = await removePostCommentAsyncMut(postCommentId);
        if (deletedItem.data !== undefined) {
            await updatePostAsync(postId, 0, 0, -1);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="post-comments__title">
            <div className="user">
                <div className="username">{targetUser.username}</div>
                <div className="when">{dateFormatting(comment.when)}</div>
            </div>
            {comment.appUserId === userId &&
                <div className="post-comments__menu">
                    <FontAwesomeIcon
                        icon={faTrash}
                        title={t("Remove")}
                        onClick={async () => await deletePostCommentAsync(comment.id)}
                    />
                </div>
            }
        </div>
    );
}

export default PostCommentTitle;