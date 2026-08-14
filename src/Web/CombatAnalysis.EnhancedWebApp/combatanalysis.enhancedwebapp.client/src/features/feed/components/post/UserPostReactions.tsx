import { Reaction } from '@/shared/helpers/EnumHelper';
import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { type SetStateAction } from 'react';
import { useCreateUserPostDislikeMutation } from '../../api/UserPostDislike.api';
import { useCreateUserPostLikeMutation } from '../../api/UserPostLike.api';
import type { UserPostModel } from '../../types/UserPostModel';
import type { UserPostReactionModel } from '../../types/UserPostReactionModel';

interface UserPostReactionsProps {
    userId: string;
    post: UserPostModel;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
}

const UserPostReactions: React.FC<UserPostReactionsProps> = ({ userId, post, setShowComments, showComments }) => {
    const [createPostLike] = useCreateUserPostLikeMutation();
    const [createPostDislike] = useCreateUserPostDislikeMutation();

    const createPostLikeAsync = async () => {
        try {
            const newPostLike: UserPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                userPostId: post?.id,
                appUserId: userId,
                status: 0
            }

            await createPostLike(newPostLike).unwrap();
        } catch (error) {
            console.error("Failed to create post like");
        }
    }

    const createPostDislikeAsync = async () => {
        try {
            const newPostDislike: UserPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                userPostId: post?.id,
                appUserId: userId,
                status: 0
            }

            await createPostDislike(newPostDislike).unwrap();
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
            </div>
        </div>
    );
}

export default UserPostReactions;