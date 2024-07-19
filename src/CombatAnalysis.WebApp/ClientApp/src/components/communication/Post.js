import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetPostByIdQuery, useUpdatePostAsyncMutation } from '../../store/api/communication/Post.api';
import { useCreatePostCommentAsyncMutation } from '../../store/api/communication/PostComment.api';
import PostComments from './PostComments';
import PostReactions from './PostReactions';
import PostTitle from './PostTitle';

import '../../styles/communication/post.scss';

const Post = ({ user, data, deletePostAsync, canBeRemoveFromUserFeed = true }) => {
    const { t } = useTranslation("communication/post");

    const [updatePostAsyncMut] = useUpdatePostAsyncMutation();

    const [createPostCommentAsyncMut] = useCreatePostCommentAsyncMutation();
    const [getPostByIdAsync] = useLazyGetPostByIdQuery();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);
    const [post, setPost] = useState(data);

    useEffect(() => {
        setIsMyPost(post?.appUserId === user?.id && canBeRemoveFromUserFeed);
    }, [])

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        try {
            const getRefreshedPostResponse = await getPostByIdAsync(postId);
            if (getRefreshedPostResponse.error) {
                console.error("Error updating post:", getRefreshedPostResponse.error);

                return;
            }

            const postForUpdate = {
                id: postId,
                owner: getRefreshedPostResponse.data.owner,
                content: getRefreshedPostResponse.data.content,
                postType: getRefreshedPostResponse.data.postType,
                tags: getRefreshedPostResponse.data.tags,
                when: getRefreshedPostResponse.data.when,
                likeCount: getRefreshedPostResponse.data.likeCount + likesCount,
                dislikeCount: getRefreshedPostResponse.data.dislikeCount + dislikesCount,
                commentCount: getRefreshedPostResponse.data.commentCount + commentsCount,
                appUserId: getRefreshedPostResponse.data.appUserId
            }

            const response = await updatePostAsyncMut(postForUpdate);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            let updateReactions = Object.assign({}, getRefreshedPostResponse.data);
            updateReactions.likeCount = getRefreshedPostResponse.data.likeCount + likesCount;
            updateReactions.dislikeCount = getRefreshedPostResponse.data.dislikeCount + dislikesCount;
            updateReactions.commentCount = getRefreshedPostResponse.data.commentCount + commentsCount;

            setPost(updateReactions);
        } catch (e) {
            console.error("Failed to update post:", e);
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            when: new Date(),
            postId: post.id,
            appUserId: user.id
        }

        const createdPostComment = await createPostCommentAsyncMut(newPostComment);
        if (createdPostComment.data !== undefined) {
            setPostCommentContent("");

            await updatePostAsync(post.id, 0, 0, 1);
        }
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
                <PostReactions
                    user={user}
                    post={post}
                    updatePostAsync={updatePostAsync}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    t={t}
                />
            </div>
            {showComments &&
                <>
                    <PostComments
                        dateFormatting={dateFormatting}
                        userId={user.id}
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