import { useState } from "react";
import { useTranslation } from 'react-i18next';
import { useLazyGetCommunityPostByIdQuery, useUpdateCommunityPostMutation } from '../../../store/api/communication/CommunityPost.api';
import { useCreateCommunityPostCommentMutation } from '../../../store/api/communication/CommunityPostComment.api';
import CommunityPostComments from './CommunityPostComments';
import CommunityPostReactions from './CommunityPostReactions';
import CommunityPostTitle from './CommunityPostTitle';

import '../../../styles/communication/post.scss';

const CommunityPost = ({ user, communityId, post }) => {
    const { t } = useTranslation("communication/post");

    const [updatePost] = useUpdateCommunityPostMutation();

    const [createPostComment] = useCreateCommunityPostCommentMutation();
    const [getPostByIdAsync] = useLazyGetCommunityPostByIdQuery();

    const [showComments, setShowComments] = useState(false);
    const [postCommentContent, setPostCommentContent] = useState("");
    const [showAddComment, setShowAddComment] = useState(false);
    const [postData, setPostData] = useState(post);

    const updatePostAsync = async (postId, likesCount, dislikesCount, commentsCount) => {
        try {
            let response = await getPostByIdAsync(postId);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            let communityPost = response.data;
            const postForUpdate = {
                id: postId,
                communityName: communityPost.communityName,
                owner: communityPost.owner,
                content: communityPost.content,
                postType: communityPost.postType,
                publicType: communityPost.publicType,
                restrictions: communityPost.restrictions,
                tags: communityPost.tags,
                createdAt: communityPost.createdAt,
                likeCount: communityPost.likeCount + likesCount,
                dislikeCount: communityPost.dislikeCount + dislikesCount,
                commentCount: communityPost.commentCount + commentsCount,
                communityId: communityPost.communityId,
                appUserId: communityPost.appUserId
            }

            response = await updatePost(postForUpdate);
            if (response.error) {
                console.error("Error updating post:", response.error);

                return;
            }

            const updatedPost = Object.assign({}, communityPost);
            updatedPost.likeCount = communityPost.likeCount + likesCount;
            updatedPost.dislikeCount = communityPost.dislikeCount + dislikesCount;
            updatedPost.commentCount = communityPost.commentCount + commentsCount;

            setPostData(updatedPost);
        } catch (e) {
            console.error("Failed to update post:", e);
        }
    }

    const createPostCommentAsync = async () => {
        const newPostComment = {
            content: postCommentContent,
            commentType: 0,
            createdAt: new Date(),
            communityPostId: postData.id,
            communityId: communityId,
            appUserId: user?.id
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
                <CommunityPostTitle
                    post={postData}
                    dateFormatting={dateFormatting}
                />
                <div className="posts__content">{postData?.content}</div>
                <CommunityPostReactions
                    user={user}
                    communityId={communityId}
                    post={postData}
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
                        userId={communityId}
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

export default CommunityPost;