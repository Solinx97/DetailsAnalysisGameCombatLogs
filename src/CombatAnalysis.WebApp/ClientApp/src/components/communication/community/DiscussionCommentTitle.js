import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { useRemoveCommunityDiscussionCommentAsyncMutation } from '../../../store/api/community/CommunityDiscussionComment.api';

const DiscussionCommentTitle = ({ meId, comment, dateFormatting }) => {
    const { t } = useTranslation("communication/community/discussion");

    const { data: user, isLoading } = useGetUserByIdQuery(comment?.appUserId);

    const [removeDiscussionCommentAsyncMut] = useRemoveCommunityDiscussionCommentAsyncMutation();

    const deletePostCommentAsync = async (discussionCommentId) => {
        await removeDiscussionCommentAsyncMut(discussionCommentId);
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="post-comments__title">
            <div className="user">
                <div className="username">{user.username}</div>
                <div className="when">{dateFormatting(comment.when)}</div>
            </div>
            {comment?.customerId === meId &&
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

export default DiscussionCommentTitle;