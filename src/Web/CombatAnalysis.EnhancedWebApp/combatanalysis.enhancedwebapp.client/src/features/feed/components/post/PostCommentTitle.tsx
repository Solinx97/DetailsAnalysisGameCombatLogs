import useFormatting from '@/shared/hooks/useFormatting';
import logger from '@/utils/Logger';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import { useRemoveCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import type { CommunityPostCommentModel } from '../../types/CommunityPostCommentModel';
import type { useRemoveUserPostCommentMutation } from '../../api/UserPostComment.api';
import type { CommentModel } from '../../types/CommentModel';
import type { UserPostCommentModel } from '../../types/UserPostCommentModel';

interface PostCommentTitleProps {
    userId: string;
    comment: CommentModel;
    feedVersion: number;
    removeUserPostComment?: ReturnType<
        typeof useRemoveUserPostCommentMutation
    >[0];
    removeCommunityPostComment?: ReturnType<
        typeof useRemoveCommunityPostCommentMutation
    >[0];
}

const PostCommentTitle: React.FC<PostCommentTitleProps> = ({ userId, comment, feedVersion, removeUserPostComment, removeCommunityPostComment }) => {
    const { t } = useTranslation('communication/postCommentTitle');

    const { dateFormatting } = useFormatting();

    const { data: targetUser, isLoading } = useGetUserByIdQuery(comment?.appUserId);

    const isCommunityPostComment = (comment: CommentModel): comment is CommunityPostCommentModel => {
        return "communityId" in comment;
    }

    const isUserPostComment = (comment: CommentModel): comment is UserPostCommentModel => {
        return "userPostId" in comment;
    }

    const removePostCommentAsync = async (postCommentId: number) => {
        try {
            if (isCommunityPostComment(comment) && removeCommunityPostComment) {
                await removeCommunityPostComment({ id: postCommentId, communityPostId: comment.communityPostId, communityId: comment.communityId, appUserId: userId, feedVersion }).unwrap();
            }
            else if (isUserPostComment(comment) && removeUserPostComment) {
                await removeUserPostComment({ id: postCommentId, userPostId: comment.userPostId, appUserId: userId, feedVersion }).unwrap();
            }
        } catch (error) {
            logger.error("Failed to remove post comment", error);
        }
    }

    if (isLoading || !targetUser || !comment) {
        return (<></>);
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
                        onClick={async () => await removePostCommentAsync(comment.id)}
                    />
                </div>
            }
        </div>
    );
}

export default PostCommentTitle;