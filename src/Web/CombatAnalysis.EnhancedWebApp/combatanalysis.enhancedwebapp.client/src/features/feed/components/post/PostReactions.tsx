import type { RootState } from '@/app/Store';
import { Reaction } from '@/shared/helpers/EnumHelper';
import VerificationRestriction from '@/shared/components/VerificationRestriction';
import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useCreateUserPostDislikeMutation } from '../../api/UserPostDislike.api';
import { useCreateUserPostLikeMutation } from '../../api/UserPostLike.api';
import type { UserPostReactionModel } from '../../types/UserPostReactionModel';
import type { useCreateCommunityPostLikeMutation } from '../../api/CommunityPostLike.api';
import type { useCreateCommunityPostDislikeMutation } from '../../api/CommunityPostDislike.api';
import type { PostModel } from '../../types/PostModel';
import type { CommunityPostModel } from '../../types/CommunityPostModel';
import type { PostReactionModel } from '../../types/PostReactionModel';
import type { CommunityPostReactionModel } from '../../types/CommunityPostReactionModel';

interface UserPostReactionsProps {
    userId: string;
    post: PostModel;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
    feedVersion: number;
    useEmailVerification: boolean;
    t: (key: string) => string;
    createUserPostLike?: ReturnType<
        typeof useCreateUserPostLikeMutation
    >[0];
    createUserPostDislike?: ReturnType<
        typeof useCreateUserPostDislikeMutation
    >[0];
    createCommunityPostLike?: ReturnType<
        typeof useCreateCommunityPostLikeMutation
    >[0];
    createCommunityPostDislike?: ReturnType<
        typeof useCreateCommunityPostDislikeMutation
    >[0];
}

const PostReactions: React.FC<UserPostReactionsProps> = ({ userId, post, setShowComments, showComments, feedVersion, t, useEmailVerification, createUserPostLike, createUserPostDislike, createCommunityPostLike, createCommunityPostDislike }) => {
    const userPrivacy = useSelector((state: RootState) => state.userPrivacy.value);

    const isCommunityPost = (post: PostModel): post is CommunityPostModel => {
        return "communityId" in post;
    }

    const createPostLikeAsync = async () => {
        try {
            const newPostLike: PostReactionModel = {
                id: 0,
                createdAt: new Date(),
                appUserId: userId,
                status: 0
            }

            if (isCommunityPost(post) && createCommunityPostLike) {
                const communityLike: CommunityPostReactionModel = {
                    ...newPostLike,
                    communityId: post.communityId,
                    communityPostId: post.id
                }

                await createCommunityPostLike({ feedVersion, reaction: communityLike }).unwrap();
            }
            else if (createUserPostLike) {
                const userLike: UserPostReactionModel = {
                    ...newPostLike,
                    userPostId: post.id
                }

                await createUserPostLike({ feedVersion, reaction: userLike }).unwrap();
            }
        } catch (error) {
            console.error("Failed to create post like");
        }
    }

    const createPostDislikeAsync = async () => {
        try {
            const newPostDislike: PostReactionModel = {
                id: 0,
                createdAt: new Date(),
                appUserId: userId,
                status: 0
            }

            if (isCommunityPost(post) && createCommunityPostDislike) {
                const communityLike: CommunityPostReactionModel = {
                    ...newPostDislike,
                    communityId: post.communityId,
                    communityPostId: post.id
                }

                await createCommunityPostDislike({ feedVersion, reaction: communityLike }).unwrap();
            }
            else if (createUserPostDislike) {
                const userLike: UserPostReactionModel = {
                    ...newPostDislike,
                    userPostId: post.id
                }

                await createUserPostDislike({ feedVersion, reaction: userLike }).unwrap();
            }
        } catch (error) {
            console.error("Failed to create post dislike");
        }
    }

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
    }

    return (
        <div className="posts__reactions">
            <div className="container">
                {(userPrivacy?.emailVerified || !useEmailVerification)
                    ? <>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__like"
                                icon={faHeart}
                                color={`${post.reaction === Reaction.Like ? 'green' : ''}`}
                                onClick={createPostLikeAsync}
                            />
                            <div className="count">{post.likeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__dislike"
                                icon={faThumbsDown}
                                color={`${post.reaction === Reaction.Dislike ? 'green' : ''}`}
                                onClick={createPostDislikeAsync}
                            />
                            <div className="count">{post.dislikeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className={`item__comment${showComments ? '_active' : ''}`}
                                icon={faMessage}
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

export default PostReactions;