import type { RootState } from '@/app/Store';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import logger from '@/utils/Logger';
import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useCreateCommunityPostDislikeMutation } from '../../api/CommunityPostDislike.api';
import { useCreateCommunityPostLikeMutation } from '../../api/CommunityPostLike.api';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import type { CommunityPostReactionModel } from '../../types/CommunityPostReactionModel';
import type { UserFeedModel } from '../../types/UserFeedModel';

interface CommunityPostReactionsProps {
    userId: string;
    communityId: number;
    post: CommunityPostModel | UserFeedModel;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
    t: (key: string) => string;
}

const CommunityPostReactions: React.FC<CommunityPostReactionsProps> = ({ userId, communityId, post, setShowComments, showComments, t }) => {
    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const [createPostLike] = useCreateCommunityPostLikeMutation();
    const [createPostDislike] = useCreateCommunityPostDislikeMutation();

    const createPostLikeAsync = async () => {
        try {
            const newPostLike: CommunityPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                communityId: communityId,
                communityPostId: post.id,
                appUserId: userId,
                status: 0
            }

            await createPostLike(newPostLike).unwrap();
        } catch (error) {
            logger.error("Failed to create community post like");
        }
    }

    const createPostDislikeAsync = async () => {
        try {
            const newPostDislike: CommunityPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                communityId: communityId,
                communityPostId: post.id,
                appUserId: userId,
                status: 0
            }

            await createPostDislike(newPostDislike).unwrap();
        } catch (error) {
            logger.error("Failed to create community post dislike");
        }
    };

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
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
                            <div className="count">{post.likeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__dislike"
                                icon={faThumbsDown}
                                title={t("Dislike")}
                                onClick={createPostDislikeAsync}
                            />
                            <div className="count">{post.dislikeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className={`item__comment${showComments ? '_active' : ''}`}
                                icon={faMessage}
                                title={t("Comment")}
                                onClick={postCommentsHandler}
                            />
                            <div className="count">{post.commentCount}</div>
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