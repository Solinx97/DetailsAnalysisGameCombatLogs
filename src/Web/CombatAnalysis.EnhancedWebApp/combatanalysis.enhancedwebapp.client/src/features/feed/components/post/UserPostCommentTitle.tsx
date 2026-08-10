import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import { useRemoveUserPostCommentMutation } from '../../api/UserPostComment.api';
import type { UserPostCommentModel } from '../../types/UserPostCommentModel';

interface UserPostCommentTitleProps {
    userId: string;
    comment: UserPostCommentModel;
    dateFormatting: (stringOfDate: string) => string;
}

const UserPostCommentTitle: React.FC<UserPostCommentTitleProps> = ({ userId, comment, dateFormatting }) => {
    const { t } = useTranslation('communication/postCommentTitle');

    const { data: targetUser, isLoading } = useGetUserByIdQuery(comment?.appUserId);

    const [removePostCommentAsyncMut] = useRemoveUserPostCommentMutation();

    const deletePostCommentAsync = async (postCommentId: number) => {
        try {
            await removePostCommentAsyncMut(postCommentId).unwrap();
        } catch (error) {
            console.error("Failed to delete post comment");
        }
    }

    if (isLoading || !targetUser) {
        return <></>;
    }

    return (
        <div className="post-comments__title">
            <div className="user">
                <div className="username">{targetUser.username}</div>
                <div className="when">{dateFormatting(comment.createdAt.toString())}</div>
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

export default UserPostCommentTitle;