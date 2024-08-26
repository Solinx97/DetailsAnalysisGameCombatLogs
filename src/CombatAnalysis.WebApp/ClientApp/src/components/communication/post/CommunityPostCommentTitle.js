
const CommunityPostCommentTitle = ({ userId, comment, dateFormatting, postId, updatePostAsync }) => {
    return (
        <div className="post-comments__title">
            <div className="user">
                <div className="when">{dateFormatting(comment.createdAt)}</div>
            </div>
        </div>
    );
}

export default CommunityPostCommentTitle;