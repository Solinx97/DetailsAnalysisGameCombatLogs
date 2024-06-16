import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../store/api/Customer.api';
import { useRemovePostCommentAsyncMutation } from '../../store/api/communication/PostComment.api';

const PostCommentTitle = ({ customerId, comment, dateFormatting, postId, updatePostAsync }) => {
    const { t } = useTranslation("communication/postCommentTitle");

    const { data: targetCustomer, isLoading } = useGetCustomerByIdQuery(comment?.customerId);

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
                <div className="username">{targetCustomer.username}</div>
                <div className="when">{dateFormatting(comment.when)}</div>
            </div>
            {comment.customerId === customerId &&
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