import React from 'react';
import { useSearchCommunityPostCommentByPostIdQuery } from '../../../store/api/communication/CommunityPostComment.api';
import CommunityPostCommentContent from './CommunityPostCommentContent';
import CommunityPostCommentTitle from './CommunityPostCommentTitle';

import '../../../styles/communication/postComments.scss';

const CommunityPostComments = ({ dateFormatting, userId, postId, updatePostAsync }) => {
    const { data: postComments, isLoading } = useSearchCommunityPostCommentByPostIdQuery(postId);

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
                    <CommunityPostCommentTitle
                        userId={userId}
                        comment={comment}
                        dateFormatting={dateFormatting}
                        postId={postId}
                        updatePostAsync={updatePostAsync}
                    />
                    <CommunityPostCommentContent
                        comment={comment}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default CommunityPostComments;