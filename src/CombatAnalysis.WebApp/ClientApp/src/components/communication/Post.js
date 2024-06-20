import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useUpdatePostAsyncMutation } from '../../store/api/communication/Post.api';
import { useCreatePostCommentAsyncMutation } from '../../store/api/communication/PostComment.api';
import { useCreatePostDislikeAsyncMutation, useLazySearchPostDislikeByPostIdQuery, useRemovePostDislikeAsyncMutation } from '../../store/api/communication/PostDislike.api';
import { useCreatePostLikeAsyncMutation, useLazySearchPostLikeByPostIdQuery, useRemovePostLikeAsyncMutation } from '../../store/api/communication/PostLike.api';
import PostComments from './PostComments';
import PostTitle from './PostTitle';

import '../../styles/communication/post.scss';

const Post = ({ customer, data, deletePostAsync, canBeRemoveFromUserFeed = true }) => {
    const { t } = useTranslation("communication/post");

    const [updatePostAsyncMut] = useUpdatePostAsyncMutation();
    const [createPostLikeAsyncMut] = useCreatePostLikeAsyncMutation();
    const [removePostLikeAsyncMut] = useRemovePostLikeAsyncMutation();
    const [searchPostLikeByPostIdAsync] = useLazySearchPostLikeByPostIdQuery();
    const [createPostDislikeAsyncMut] = useCreatePostDislikeAsyncMutation();
    const [removePostDislikeAsyncMut] = useRemovePostDislikeAsyncMutation();
    const [searchPostDislikeByPostIdAsync] = useLazySearchPostDislikeByPostIdQuery();
    const [createPostCommentAsyncMut] = useCreatePostCommentAsyncMutation();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);
    const [post, setPost] = useState(data);

    useEffect(() => {
        setIsMyPost(post?.customerId === customer?.id && canBeRemoveFromUserFeed);
    }, [])

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        try {
            const postForUpdate = {
                id: postId,
                owner: post.owner,
                content: post.content,
                when: post.when,
                likeCount: post.likeCount + likesCount,
                dislikeCount: post.dislikeCount + dislikesCount,
                commentCount: post.commentCount + commentsCount,
                customerId: post.customerId
            }

            const response = await updatePostAsyncMut(postForUpdate);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            let updateReactions = Object.assign({}, post);
            updateReactions.likeCount = post.likeCount + likesCount;
            updateReactions.dislikeCount = post.dislikeCount + dislikesCount;
            updateReactions.commentCount = post.commentCount + commentsCount;

            setPost(updateReactions);
        } catch (e) {
            console.error("Failed to update post:", e);
        }
    }

    const createPostLikeAsync = async (postId) => {
        const postLikeIsExist = await getPostLikesAsync(postId);
        if (postLikeIsExist) {
            return;
        }

        const postDislikeIsExist = await getPostDislikesAsync(postId)

        const newPostLike = {
            postId: postId,
            customerId: customer.id
        }

        const createdPostLike = await createPostLikeAsyncMut(newPostLike);
        if (createdPostLike.data !== undefined) {
            if (postDislikeIsExist) {
                await updatePostAsync(postId, 1, -1, 0);
            }
            else {
                await updatePostAsync(postId, 1, 0, 0);
            }
        }
    }

    const getPostLikesAsync = async (postId) => {
        const postLikes = await searchPostLikeByPostIdAsync(postId);
        if (postLikes.data !== undefined) {
            return await removePostLikeIfExistAsync(postLikes.data);
        }

        return false;
    }

    const removePostLikeIfExistAsync = async (postLikes) => {
        for (let i = 0; i < postLikes.length; i++) {
            if (postLikes[i].customerId === customer.id) {
                await deletePostLikeAsync(postLikes[i].postId, postLikes[i].id);
                return true;
            }
        }

        return false;
    }

    const deletePostLikeAsync = async (postId, postLikeId) => {
        const deletedItem = await removePostLikeAsyncMut(postLikeId);
        if (deletedItem !== null) {
            await updatePostAsync(postId, -1, 0, 0);
        }
    }

    const createPostDislikeAsync = async (postId) => {
        const postDislikeIsExist = await getPostDislikesAsync(postId);
        if (postDislikeIsExist) {
            return;
        }

        const postLikeIsExist = await getPostLikesAsync(postId);

        const newPostDislike = {
            postId: postId,
            customerId: customer.id
        }

        const createdPostDislike = await createPostDislikeAsyncMut(newPostDislike);
        if (createdPostDislike.data !== undefined) {
            if (postLikeIsExist) {
                await updatePostAsync(postId, -1, 1, 0);
            }
            else {
                await updatePostAsync(postId, 0, 1, 0);
            }
        }
    }

    const getPostDislikesAsync = async (postId) => {
        const postDislikes = await searchPostDislikeByPostIdAsync(postId);
        if (postDislikes.data !== undefined) {
            return await removePostDislikeIfExistAsync(postDislikes.data);
        }

        return false;
    }

    const removePostDislikeIfExistAsync = async (postDislikes) => {
        for (let i = 0; i < postDislikes.length; i++) {
            if (postDislikes[i].customerId === customer.id) {
                await deletePostDislikeAsync(postDislikes[i].postId, postDislikes[i].id);
                return true;
            }
        }

        return false;
    }

    const deletePostDislikeAsync = async (postId, postDislikeId) => {
        const deletedItem = await removePostDislikeAsyncMut(postDislikeId);
        if (deletedItem.data !== undefined) {
            await updatePostAsync(postId, 0, -1, 0);
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            when: new Date(),
            postId: post.id,
            customerId: customer.id
        }

        const createdPostComment = await createPostCommentAsyncMut(newPostComment);
        if (createdPostComment.data !== undefined) {
            setPostCommentContent("");

            await updatePostAsync(post.id, 0, 0, 1);
        }
    }

    const postCommentsHandler = () => {
        setShowComments((item) => !item);
    }

    const dateFormatting = (stringOfDate) => {
        const date = new Date(stringOfDate);
        const month = date.getMonth();
        let nameOfMonth = "";

        switch (month) {
            case 0:
                nameOfMonth = "January";
                break;
            case 1:
                nameOfMonth = "February";
                break;
            case 2:
                nameOfMonth = "March";
                break;
            case 3:
                nameOfMonth = "April";
                break;
            case 4:
                nameOfMonth = "May";
                break;
            case 5:
                nameOfMonth = "June";
                break;
            case 6:
                nameOfMonth = "July";
                break;
            case 7:
                nameOfMonth = "August";
                break;
            case 8:
                nameOfMonth = "September";
                break;
            case 9:
                nameOfMonth = "October";
                break;
            case 10:
                nameOfMonth = "November";
                break;
            case 11:
                nameOfMonth = "December";
                break;
            default:
                break;
        }

        const formatted = `${date.getDate()} ${nameOfMonth}, ${date.getHours()}:${date.getMinutes()}`;

        return formatted;
    }

    return (
        <>
            <div className="posts__card">
                <PostTitle
                    post={post}
                    dateFormatting={dateFormatting}
                    deletePostAsync={deletePostAsync}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post?.content}</div>
                <div className="posts__reactions">
                    <div className="container">
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__like"
                                icon={faHeart}
                                title={t("Like")}
                                onClick={async () => await createPostLikeAsync(post?.id)}
                            />
                            <div className="count">{post?.likeCount}</div>
                        </div>
                        <div className="item">
                            <FontAwesomeIcon
                                className="item__dislike"
                                icon={faThumbsDown}
                                title={t("Dislike")}
                                onClick={async () => await createPostDislikeAsync(post?.id)}
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
            </div>
            {showComments &&
                <>
                    <PostComments
                        dateFormatting={dateFormatting}
                        customerId={customer.id}
                        postId={post.id}
                        updatePostAsync={updatePostAsync}
                    />
                    <div className="add-new-comment">
                        <div className="add-new-comment__title">
                            {showAddComment
                                ? <div>{t("AddComment")}</div>
                                : <div className="open-add-comment" onClick={() => setShowAddComment((item) => !item)}>{t("Add")}</div>
                            }
                        </div>
                        {showAddComment &&
                            <div className="add-new-comment__content">
                            <textarea className="form-control" rows="3" cols="60" onChange={e => setPostCommentContent(e.target.value)} value={postCommentContent} />
                            <div className="actions">
                                    <div className="add-comment" onClick={async () => await createPostCommentAsync()}>{t("Add")}</div>
                                    <div className="hide" onClick={() => setShowAddComment((item) => !item)}>{t("Hide")}</div>
                                </div>
                            </div>
                        }
                    </div>
                </>
            }
        </>
    );
}

export default Post;