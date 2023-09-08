import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemoveCommunityDiscussionCommentAsyncMutation } from '../../../store/api/communication/community/CommunityDiscussionComment.api';

const DiscussionCommentTitle = ({ meId, comment, dateFormatting }) => {
    const { t } = useTranslation("communication/postCommentTitle");

    const { data: customer, isLoading } = useGetCustomerByIdQuery(comment?.customerId);

    const [removeDiscussionCommentAsyncMut] = useRemoveCommunityDiscussionCommentAsyncMutation();

    const deletePostCommentAsync = async (discussionCommentId) => {
        await removeDiscussionCommentAsyncMut(discussionCommentId);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <li className="post-comments__title list-group-item">
            <div className="card-title">{customer.username}</div>
            <div className="card-title">{dateFormatting(comment.when)}</div>
            {comment?.customerId === meId &&
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

export default DiscussionCommentTitle;