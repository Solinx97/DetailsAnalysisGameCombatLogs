import React, { memo } from 'react';
import { useSearchPostCommentByPostIdQuery } from '../../store/api/communication/PostComment.api';
import PostCommentContent from './PostCommentContent';
import PostCommentTitle from './PostCommentTitle';

import '../../styles/communication/postComments.scss';

const PostComments = ({ dateFormatting, userId, postId, updatePostAsync }) => {
    const { data: postComments, isLoading } = useSearchPostCommentByPostIdQuery(postId);

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    if (postComments?.length === 0) {
        return (<></>);
    }

    return (
        <ul className="post-comments">
            {postComments?.map((item) => (
                    <li key={item.id} className="post-comments__card">
                        <PostCommentTitle
                            userId={userId}
                            comment={item}
                            dateFormatting={dateFormatting}
                            postId={postId}
                            updatePostAsync={updatePostAsync}
                        />
                        <PostCommentContent
                            userId={userId}
                            comment={item}
                        />
                    </li>
                ))
            }
        </ul>
    );
}

export default memo(PostComments);