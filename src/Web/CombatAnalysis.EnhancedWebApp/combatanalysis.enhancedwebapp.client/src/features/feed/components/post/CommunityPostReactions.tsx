import type { RootState } from '@/app/Store';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import logger from '@/utils/Logger';
import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useCreateCommunityPostDislikeMutation, useCountCommunityPostDislikeByPostIdQuery } from '../../api/CommunityPostDislike.api';
import { useCreateCommunityPostLikeMutation, useCountCommunityPostLikeByPostIdQuery } from '../../api/CommunityPostLike.api';
import { useCountCommunityPostCommentByPostIdQuery } from '../../api/CommunityPostComment.api';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import type { CommunityPostReactionModel } from '../../types/CommunityPostReactionModel';

interface CommunityPostReactionsProps {
    userId: string;
    communityId: number;
    post: CommunityPostModel;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
    t: (key: string) => string;
}

const CommunityPostReactions: React.FC<CommunityPostReactionsProps> = ({ userId, communityId, post, setShowComments, showComments, t }) => {
    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const [createPostLike] = useCreateCommunityPostLikeMutation();
    const { data: likes, isLoading: likesIsLoading } = useCountCommunityPostLikeByPostIdQuery(post.id);
    const [createPostDislike] = useCreateCommunityPostDislikeMutation();
    const { data: dislikes, isLoading: dislikesIsLoading } = useCountCommunityPostDislikeByPostIdQuery(post.id);
    const { data: comments, isLoading: commentsIsLoading } = useCountCommunityPostCommentByPostIdQuery(post.id);

    const createPostLikeAsync = useCallback(async () => {
        try {
            const newPostLike: CommunityPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                communityPostId: post?.id,
                communityId: communityId,
                appUserId: userId
            }

            await createPostLike(newPostLike);
        } catch (error) {
            logger.error("Failed to create community post like");
        }
    }, [post]);

    const createPostDislikeAsync = useCallback(async () => {
        try {
            const newPostDislike: CommunityPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                communityPostId: post?.id,
                communityId: communityId,
                appUserId: userId
            }

            await createPostDislike(newPostDislike);
        } catch (error) {
            logger.error("Failed to create community post dislike");
        }
    }, [post]);

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
    }

    if (likesIsLoading || dislikesIsLoading || commentsIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="posts__reactions">
            <div className="container">
                {userPrivacy?.emailVerified
                    ? <>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__like"
                                icon={faHeart}
                                title={t("Like")}
                                onClick={createPostLikeAsync}
                            />
                            <div className="count">{likes}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__dislike"
                                icon={faThumbsDown}
                                title={t("Dislike")}
                                onClick={createPostDislikeAsync}
                            />
                            <div className="count">{dislikes}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className={`item__comment${showComments ? '_active' : ''}`}
                                icon={faMessage}
                                title={t("Comment")}
                                onClick={postCommentsHandler}
                            />
                            <div className="count">{comments}</div>
                        </div>
                    </>
                    : <VerificationRestriction
                        contentText={t("ReactionsForbidden")}
                        infoText={t("VerificationReactions")}
                    />
                }
            </div>
        </div>
    );
}

export default CommunityPostReactions;