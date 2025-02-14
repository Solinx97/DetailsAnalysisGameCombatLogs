import { memo, useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useGetUserPostByIdQuery, useLazyGetUserPostByIdQuery, useUpdateUserPostMutation } from '../../../store/api/post/UserPost.api';
import { useCreateUserPostCommentMutation } from '../../../store/api/post/UserPostComment.api';
import UserPostComments from './UserPostComments';
import UserPostReactions from './UserPostReactions';
import UserPostTitle from './UserPostTitle';

import '../../../styles/communication/post.scss';

const UserPost = ({ userId, postId }) => {
    const { t } = useTranslation("communication/post");

    const [updatePost] = useUpdateUserPostMutation();

    const [createPostComment] = useCreateUserPostCommentMutation();
    const [getPostByIdAsync] = useLazyGetUserPostByIdQuery();

    const { data: post, isLoading } = useGetUserPostByIdQuery(postId);

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    useEffect(() => {
        setIsMyPost(post?.appUserId === userId);
    }, []);

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        try {
            let response = await getPostByIdAsync(postId);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            let userPost = response.data;
            const postForUpdate = {
                ...userPost,
                likeCount: userPost.likeCount + likesCount,
                dislikeCount: userPost.dislikeCount + dislikesCount,
                commentCount: userPost.commentCount + commentsCount
            };

            response = await updatePost(postForUpdate);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }
        } catch (e) {
            console.error("Failed to update post:", e);

            return post;
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            createdAt: new Date(),
            userPostId: postId,
            appUserId: userId
        }

        const response = await createPostComment(newPostComment);
        if (response.data) {
            setPostCommentContent("");

            await updatePostAsync(postId, 0, 0, 1);
        }
    }

    const dateFormatting = (stringOfDate) => {
        const date = new Date(stringOfDate);
        const month = date.getMonth();
        const monthes = {
            0: "January",
            1: "February",
            2: "March",
            3: "April",
            4: "May",
            5: "June",
            6: "July",
            7: "August",
            8: "September",
            9: "October",
            10: "November",
            11: "December"
        };

        const formatted = `${date.getDate()} ${monthes[month]}, ${date.getHours()}:${date.getMinutes()}`;

        return formatted;
    }

    if (isLoading) {
        return (<></>);
    }

    return (
        <>
            <div className="posts__card">
                <UserPostTitle
                    post={post}
                    dateFormatting={dateFormatting}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post?.content}</div>
                <UserPostReactions
                    userId={userId}
                    post={post}
                    updatePostAsync={updatePostAsync}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    t={t}
                />
            </div>
            {showComments &&
                <>
                    <UserPostComments
                        dateFormatting={dateFormatting}
                        userId={userId}
                        postId={postId}
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
                                <div className="add-comment" onClick={createPostCommentAsync}>{t("Add")}</div>
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

export default memo(UserPost);