import React, { memo } from 'react';
import { useSearchPostCommentByPostIdQuery } from '../../store/api/PostComment.api';
import PostCommentTitle from './PostCommentTitle';

import '../../styles/communication/postComments.scss';
import PostCommentContent from './PostCommentContent';

const PostComments = ({ dateFormatting, customerId, postId, updatePostAsync }) => {
    const { data: postComments, isLoading } = useSearchPostCommentByPostIdQuery(postId);

    if (isLoading) {
        return <></>;
    }

    return (
        <ul className="post-comments">
            {
                postComments?.map((item) => (
                    <li key={item.id} className="card">
                        <ul className="list-group list-group-flush">
                            <PostCommentTitle
                                customerId={customerId}
                                comment={item}
                                dateFormatting={dateFormatting}
                                postId={postId}
                                updatePostAsync={updatePostAsync}
                            />
                            <PostCommentContent
                                customerId={customerId}
                                comment={item}
                            />
                        </ul>
                    </li>
                ))
            }
        </ul>
    );
}

export default memo(PostComments);