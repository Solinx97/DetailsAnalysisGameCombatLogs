import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { type SetStateAction } from 'react';
import { useCreateUserPostDislikeMutation, useCountUserPostDislikeByPostIdQuery } from '../../api/UserPostDislike.api';
import { useCreateUserPostLikeMutation, useCountUserPostLikeByPostIdQuery } from '../../api/UserPostLike.api';
import { useCountUserPostCommentByUserPostIdQuery } from '../../api/UserPostComment.api';
import type { UserPostModel } from '../../types/UserPostModel';
import type { UserPostReactionModel } from '../../types/UserPostReactionModel';

interface UserPostReactionsProps {
    userId: string;
    post: UserPostModel;
    setShowComments: (value: SetStateAction<boolean>) => void;
    showComments: boolean;
    t: (key: string) => string;
}

const UserPostReactions: React.FC<UserPostReactionsProps> = ({ userId, post, setShowComments, showComments, t }) => {
    const [createPostLike] = useCreateUserPostLikeMutation();
    const { data: likes, isLoading: likesIsLoading } = useCountUserPostLikeByPostIdQuery(post.id);
    const [createPostDislike] = useCreateUserPostDislikeMutation();
    const { data: dislikes, isLoading: dislikesIsLoading } = useCountUserPostDislikeByPostIdQuery(post.id);
    const { data: comments, isLoading: commentsIsLoading } = useCountUserPostCommentByUserPostIdQuery(post.id);

    const createPostLikeAsync = async () => {
        try {
            const newPostLike: UserPostReactionModel = {
                id: 0,
                createdAt: new Date(),
                userPostId: post?.id,
                appUserId: userId
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
                appUserId: userId
            }

            await createPostDislike(newPostDislike).unwrap();
        } catch (error) {
            console.error("Failed to create post dislike");
        }
    }

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
    }

    if (likesIsLoading || dislikesIsLoading || commentsIsLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <div className="posts__reactions">
            <div className="container">
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
            </div>
        </div>
    );
}

export default UserPostReactions;