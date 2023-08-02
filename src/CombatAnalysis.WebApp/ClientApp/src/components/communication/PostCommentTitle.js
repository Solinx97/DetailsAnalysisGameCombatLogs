import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';
import { useRemovePostCommentAsyncMutation } from '../../store/api/PostComment.api';

const PostCommentTitle = ({ customerId, comment, dateFormatting, postId, updatePostAsync }) => {
    const { t } = useTranslation("communication/postCommentTitle");

    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(comment?.ownerId);

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
        <li className="post-comments__title list-group-item">
            <div className="card-title">{targetCustomer.username}</div>
            <div className="card-title">{dateFormatting(comment.when)}</div>
            {comment.ownerId === customerId &&
                <div className="post-comments__menu">
                    <FontAwesomeIcon
                        icon={faTrash}
                        title={t("Remove")}
                        onClick={async () => await deletePostCommentAsync(comment.id)}
                    />
                </div>
            }
        </li>
    );
}

export default PostCommentTitle;