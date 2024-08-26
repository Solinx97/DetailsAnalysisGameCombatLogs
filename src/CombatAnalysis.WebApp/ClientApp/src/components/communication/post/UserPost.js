import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useCreateUserPostCommentMutation } from '../../../store/api/communication/UserPostComment.api';
import { useLazyGetUserPostByIdQuery, useUpdateUserPostMutation } from '../../../store/api/communication/UserPost.api';
import UserPostComments from './UserPostComments';
import UserPostReactions from './UserPostReactions';
import UserPostTitle from './UserPostTitle';

import '../../../styles/communication/post.scss';

const UserPost = ({ user, post }) => {
    const { t } = useTranslation("communication/post");

    const [updatePost] = useUpdateUserPostMutation();

    const [createPostComment] = useCreateUserPostCommentMutation();
    const [getPostByIdAsync] = useLazyGetUserPostByIdQuery();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);
    const [postData, setPostData] = useState(post);

    useEffect(() => {
        setIsMyPost(postData?.appUserId === user?.id);
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
                id: postId,
                owner: userPost.owner,
                content: userPost.content,
                publicType: userPost.publicType,
                tags: userPost.tags,
                createdAt: userPost.createdAt,
                likeCount: userPost.likeCount + likesCount,
                dislikeCount: userPost.dislikeCount + dislikesCount,
                commentCount: userPost.commentCount + commentsCount,
                appUserId: userPost.appUserId
            }

            response = await updatePost(postForUpdate);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            const updatedPost = Object.assign({}, userPost);
            updatedPost.likeCount = userPost.likeCount + likesCount;
            updatedPost.dislikeCount = userPost.dislikeCount + dislikesCount;
            updatedPost.commentCount = userPost.commentCount + commentsCount;

            setPostData(updatedPost);
        } catch (e) {
            console.error("Failed to update post:", e);
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            createdAt: new Date(),
            userPostId: postData.id,
            appUserId: user.id
        }

        const response = await createPostComment(newPostComment);
        if (response.data) {
            setPostCommentContent("");

            await updatePostAsync(postData.id, 0, 0, 1);
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

    return (
        <>
            <div className="posts__card">
                <UserPostTitle
                    post={postData}
                    dateFormatting={dateFormatting}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{postData?.content}</div>
                <UserPostReactions
                    user={user}
                    post={postData}
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
                        userId={user.id}
                        postId={postData.id}
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

export default UserPost;