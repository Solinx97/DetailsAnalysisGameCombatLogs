import useFormatting from '@/shared/hooks/useFormatting';
import logger from '@/utils/Logger';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import { useRemoveCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';

interface CommunityPostCommentTitleProps {
    userId: string;
    comment: CommunityPostCommentModel;
}

const CommunityPostCommentTitle: React.FC<CommunityPostCommentTitleProps> = ({ userId, comment }) => {
    const { t } = useTranslation('communication/postCommentTitle');

    const { dateFormatting } = useFormatting();

    const { data: targetUser, isLoading } = useGetUserByIdQuery(comment?.appUserId);

    const [removePostCommentAsyncMut] = useRemoveCommunityPostCommentMutation();

    const deletePostCommentAsync = async (postCommentId: number) => {
        try {
            await removePostCommentAsyncMut(postCommentId).unwrap();
        } catch (e) {
            logger.error("Failed to remove community post comment", e);
        }
    }

    if (isLoading || !targetUser || !comment) {
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

export default CommunityPostCommentTitle;