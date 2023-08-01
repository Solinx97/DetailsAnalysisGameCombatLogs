import { faHeart, faMessage, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useGetPostByIdQuery, useUpdatePostAsyncMutation } from '../../store/api/Post.api';
import { useCreatePostCommentAsyncMutation } from '../../store/api/PostComment.api';
import { useCreatePostDislikeAsyncMutation, useLazySearchPostDislikeByPostIdQuery, useRemovePostDislikeAsyncMutation } from '../../store/api/PostDislike.api';
import { useCreatePostLikeAsyncMutation, useLazySearchPostLikeByPostIdQuery, useRemovePostLikeAsyncMutation } from '../../store/api/PostLike.api';
import PostComments from './PostComments';
import PostTitle from './PostTitle';

import '../../styles/communication/post.scss';

const Post = ({ customer, targetPostType, deletePostAsync }) => {
    const { t, i18n } = useTranslation("communication/post");

    const { data: post, isLoading } = useGetPostByIdQuery(targetPostType?.postId);

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

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        const postForUpdate = {
            id: postId,
            content: post.content,
            when: post.when,
            likeCount: post.likeCount + likesCount,
            dislikeCount: post.dislikeCount + dislikesCount,
            commentCount: post.commentCount + commentsCount,
            ownerId: post.ownerId
        }

        await updatePostAsyncMut(postForUpdate);
    }

    const createPostLikeAsync = async (postId) => {
        const postLikeIsExist = await getPostLikesAsync(postId);
        if (postLikeIsExist) {
            return;
        }

        const postDislikeIsExist = await getPostDislikesAsync(postId)

        const newPostLike = {
            postId: postId,
            ownerId: customer.id
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
            if (postLikes[i].ownerId === customer.id) {
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
            ownerId: customer.id
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
            if (postDislikes[i].ownerId === customer.id) {
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
            ownerId: customer.id
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

    if (isLoading) {
        return <></>;
    }

    return (
        <>
            <div className="card">
                <ul className="list-group list-group-flush">
                    <PostTitle
                        post={post}
                        dateFormatting={dateFormatting}
                        deletePostAsync={deletePostAsync}
                    />
                    <li className="list-group-item">
                        <div className="card-text">{post.content}</div>
                    </li>
                    <li className="posts__reaction list-group-item">
                        <div className="posts__reaction item">
                            <FontAwesomeIcon
                                className="post__reaction_like"
                                icon={faHeart}
                                title={t("Like")}
                                onClick={async () => await createPostLikeAsync(post.id)}
                            />
                            <div className="count">{post.likeCount}</div>
                        </div>
                        <div className="posts__reaction item">
                            <FontAwesomeIcon
                                className="post__reaction_dislike"
                                icon={faThumbsDown}
                                title={t("Dislike")}
                                onClick={async () => await createPostDislikeAsync(post.id)}
                            />
                            <div className="count">{post.dislikeCount}</div>
                        </div>
                        <div className="posts__reaction item">
                            <FontAwesomeIcon
                                className={`post__reaction${showComments && '_active'}`}
                                icon={faMessage}
                                title={t("Comment")}
                                onClick={postCommentsHandler}
                            />
                            <div className="count">{post.commentCount}</div>
                        </div>
                    </li>
                </ul>
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
                            <div>{t("AddComment")}</div>
                        </div>
                        <textarea rows="1" cols="75" onChange={e => setPostCommentContent(e.target.value)} value={postCommentContent} />
                        <button type="button" className="btn btn-outline-info" onClick={async () => await createPostCommentAsync()}>{t("Add")}</button>
                    </div>
                </>
            }
        </>
    );
}

export default Post;