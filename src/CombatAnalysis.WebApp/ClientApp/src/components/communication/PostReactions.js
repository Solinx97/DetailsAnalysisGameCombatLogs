import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useCallback } from 'react';
import { useCreatePostDislikeAsyncMutation, useLazySearchPostDislikeByPostIdQuery, useRemovePostDislikeAsyncMutation } from '../../store/api/communication/PostDislike.api';
import { useCreatePostLikeAsyncMutation, useLazySearchPostLikeByPostIdQuery, useRemovePostLikeAsyncMutation } from '../../store/api/communication/PostLike.api';

const PostReactions = ({ customer, post, updatePostAsync, setShowComments, showComments, t }) => {
    const [createPostLikeAsyncMut] = useCreatePostLikeAsyncMutation();
    const [removePostLikeAsyncMut] = useRemovePostLikeAsyncMutation();
    const [searchPostLikeByPostIdAsync] = useLazySearchPostLikeByPostIdQuery();
    const [createPostDislikeAsyncMut] = useCreatePostDislikeAsyncMutation();
    const [removePostDislikeAsyncMut] = useRemovePostDislikeAsyncMutation();
    const [searchPostDislikeByPostIdAsync] = useLazySearchPostDislikeByPostIdQuery();

    const getPostLikesAsync = async (postId) => {
        const postLikes = await searchPostLikeByPostIdAsync(postId);
        if (postLikes.data !== undefined) {
            return await removePostLikeIfExistAsync(postLikes.data);
        }

        return false;
    }

    const getPostDislikesAsync = async (postId) => {
        const postDislikes = await searchPostDislikeByPostIdAsync(postId);
        if (postDislikes.data !== undefined) {
            return await removePostDislikeIfExistAsync(postDislikes.data);
        }

        return false;
    }

    const removePostLikeIfExistAsync = async (postLikes) => {
        for (let i = 0; i < postLikes.length; i++) {
            if (postLikes[i].customerId === customer.id) {
                await removePostLikeAsyncMut(postLikes[i].id);
                return true;
            }
        }

        return false;
    }

    const removePostDislikeIfExistAsync = async (postDislikes) => {
        for (let i = 0; i < postDislikes.length; i++) {
            if (postDislikes[i].customerId === customer.id) {
                await removePostDislikeAsyncMut(postDislikes[i].id);
                return true;
            }
        }

        return false;
    }

    const createPostLikeAsync = useCallback(async () => {
        const postLikeIsExist = await getPostLikesAsync(post?.id);
        if (postLikeIsExist) {
            await updatePostAsync(post?.id, -1, 0, 0);
            return;
        }

        const postDislikeIsExist = await getPostDislikesAsync(post?.id)

        const newPostLike = {
            postId: post?.id,
            customerId: customer.id
        }

        const createdPostLike = await createPostLikeAsyncMut(newPostLike);
        if (createdPostLike.data !== undefined) {
            if (postDislikeIsExist) {
                await updatePostAsync(post?.id, 1, -1, 0);
            }
            else {
                await updatePostAsync(post?.id, 1, 0, 0);
            }
        }
    }, [post]);

    const createPostDislikeAsync = useCallback(async () => {
        const postDislikeIsExist = await getPostDislikesAsync(post?.id);
        if (postDislikeIsExist) {
            await updatePostAsync(post?.id, 0, -1, 0);
            return;
        }

        const postLikeIsExist = await getPostLikesAsync(post?.id);

        const newPostDislike = {
            postId: post?.id,
            customerId: customer.id
        }

        const createdPostDislike = await createPostDislikeAsyncMut(newPostDislike);
        if (createdPostDislike.data !== undefined) {
            if (postLikeIsExist) {
                await updatePostAsync(post?.id, -1, 1, 0);
            }
            else {
                await updatePostAsync(post?.id, 0, 1, 0);
            }
        }
    }, [post]);

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
                        title={t("Like")}
                        onClick={async () => await createPostLikeAsync()}
                    />
                    <div className="count">{post?.likeCount}</div>
                </div>
                <div className="item">
                    <FontAwesomeIcon
                        className="item__dislike"
                        icon={faThumbsDown}
                        title={t("Dislike")}
                        onClick={async () => await createPostDislikeAsync()}
                    />
                    <div className="count">{post?.dislikeCount}</div>
                </div>
                <div className="item">
                    <FontAwesomeIcon
                        className={`item__comment${showComments ? '_active' : ''}`}
                        icon={faMessage}
                        title={t("Comment")}
                        onClick={postCommentsHandler}
                    />
                    <div className="count">{post?.commentCount}</div>
                </div>
            </div>
        </div>
    );
}

export default PostReactions;