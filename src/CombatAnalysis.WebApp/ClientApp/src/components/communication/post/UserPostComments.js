import React from 'react';
import { useSearchUserPostCommentByPostIdQuery } from '../../../store/api/post/UserPostComment.api';
import UserPostCommentContent from './UserPostCommentContent';
import UserPostCommentTitle from './UserPostCommentTitle';

import '../../../styles/communication/postComments.scss';

const UserPostComments = ({ dateFormatting, userId, postId, updatePostAsync }) => {
    const { data: postComments, isLoading } = useSearchUserPostCommentByPostIdQuery(postId);

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    if (postComments?.length === 0) {
        return (<></>);
    }

    return (
        <ul className="post-comments">
            {postComments?.map((comment) => (
                    <li key={comment.id} className="post-comments__card">
                        <UserPostCommentTitle
                            userId={userId}
                            comment={comment}
                            dateFormatting={dateFormatting}
                            postId={postId}
                            updatePostAsync={updatePostAsync}
                        />
                        <UserPostCommentContent
                            userId={userId}
                            comment={comment}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default UserPostComments;