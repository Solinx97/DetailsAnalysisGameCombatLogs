import { APP_CONFIG } from '@/config/appConfig';
import { useEffect, useRef, useState } from 'react';
import { useGetUserPostCommentByUserPostIdQuery, useRemoveUserPostCommentMutation, useUpdateUserPostCommentMutation } from '../../api/UserPostComment.api';
import PostCommentContent from './PostCommentContent';
import PostCommentTitle from './PostCommentTitle';

import './PostComments.scss';

interface UserPostCommentsProps {
    userId: string;
    postId: number;
    feedVersion: number;
}

const UserPostComments: React.FC<UserPostCommentsProps> = ({ userId, postId, feedVersion }) => {
    const maxContentLength = 256;

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.posCommentSize ?? 5);
    const maxContentLengthRef = useRef<number>(APP_CONFIG.communication.length.userPostCommentContentMaxLength ?? maxContentLength);

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const [currentContentLength, setCurrentContentLength] = useState(0);

    const { data: postComments, isLoading } = useGetUserPostCommentByUserPostIdQuery({ userPostId: postId, page, pageSize: pageSizeRef.current });

    const [removePostCommentAsyncMut] = useRemoveUserPostCommentMutation();
    const [updatePostComment] = useUpdateUserPostCommentMutation();

    useEffect(() => {
        if (!postComments) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < postComments.count);
    }, [page, postComments]);

    if (!postComments || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <ul className="post-comments">
                {postComments.comments.map((comment) => (
                    <li key={comment.id} className="post-comments__card">
                        <PostCommentTitle
                            userId={userId}
                            comment={comment}
                            feedVersion={feedVersion}
                            removeUserPostComment={removePostCommentAsyncMut}
                        />
                        <PostCommentContent
                            userId={userId}
                            comment={comment}
                            currentContentLength={currentContentLength}
                            setCurrentContentLength={setCurrentContentLength}
                            maxContentLength={maxContentLengthRef.current}
                            updateUserPostComment={updatePostComment}
                        />
                    </li>
                ))
                }
            </ul>
            {hasMore &&
                <div onClick={() => setPage(prev => prev + 1)} className="post-comments__load-more">Load more</div>
            }
        </>
    );
}

export default UserPostComments;