import { useEffect, useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetCommunityPostByIdQuery, useUpdateCommunityPostMutation } from '../../../store/api/post/CommunityPost.api';
import { useCreateCommunityPostCommentMutation } from '../../../store/api/post/CommunityPostComment.api';
import CommunityPostComments from './CommunityPostComments';
import CommunityPostReactions from './CommunityPostReactions';
import CommunityPostTitle from './CommunityPostTitle';

import '../../../styles/communication/post.scss';

const CommunityPost = ({ userId, communityId, post }) => {
    const { t } = useTranslation("communication/post");

    const [updatePost] = useUpdateCommunityPostMutation();

    const [createPostComment] = useCreateCommunityPostCommentMutation();
    const [getPostByIdAsync] = useLazyGetCommunityPostByIdQuery();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [isMyPost, setIsMyPost] = useState(false);

    useEffect(() => {
        setIsMyPost(post?.appUserId === userId);
    }, [post]);

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        try {
            let response = await getPostByIdAsync(postId);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            let communityPost = response.data;
            const postForUpdate = {
                ...communityPost,
                likeCount: communityPost.likeCount + likesCount,
                dislikeCount: communityPost.dislikeCount + dislikesCount,
                commentCount: communityPost.commentCount + commentsCount
            };

            response = await updatePost(postForUpdate);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }
        } catch (e) {
            console.error("Failed to update post:", e);
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            commentType: 0,
            createdAt: new Date(),
            communityPostId: post.id,
            communityId: communityId,
            appUserId: userId
        }

        const response = await createPostComment(newPostComment);
        if (response.data) {
            setPostCommentContent("");

            await updatePostAsync(post.id, 0, 0, 1);
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
                <CommunityPostTitle
                    post={post}
                    dateFormatting={dateFormatting}
                    isMyPost={isMyPost}
                />
                <div className="posts__content">{post?.content}</div>
                <CommunityPostReactions
                    userId={userId}
                    communityId={communityId}
                    post={post}
                    updatePostAsync={updatePostAsync}
                    setShowComments={setShowComments}
                    showComments={showComments}
                    t={t}
                />
            </div>
            {showComments &&
                <>
                    <CommunityPostComments
                        dateFormatting={dateFormatting}
                        userId={userId}
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

export default CommunityPost;