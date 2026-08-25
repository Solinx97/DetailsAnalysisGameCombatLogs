import { APP_CONFIG } from '@/config/appConfig';
import { useEffect, useRef, useState } from 'react';
import { useGetCommunityPostCommentByPostIdQuery, useRemoveCommunityPostCommentMutation, useUpdateCommunityPostCommentMutation } from '../../api/CommunityPostComment.api';
import PostCommentTitle from './PostCommentTitle';
import PostCommentContent from './PostCommentContent';

import './PostComments.scss';

interface CommunityPostCommentsProps {
    userId: string;
    postId: number;
    feedVersion: number;
}

const CommunityPostComments: React.FC<CommunityPostCommentsProps> = ({ userId, postId, feedVersion }) => {
    const maxContentLength = 256;

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.posCommentSize ?? 5);
    const maxContentLengthRef = useRef<number>(APP_CONFIG.communication.length.communityPostCommentContentMaxLength ?? maxContentLength);

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);
    const [currentContentLength, setCurrentContentLength] = useState(0);

    const { data: postComments, isLoading } = useGetCommunityPostCommentByPostIdQuery({ communityPostId: postId, page, pageSize: pageSizeRef.current });

    const [removePostCommentAsyncMut] = useRemoveCommunityPostCommentMutation();
    const [updatePostComment] = useUpdateCommunityPostCommentMutation();

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
                            removeCommunityPostComment={removePostCommentAsyncMut}
                        />
                        <PostCommentContent
                            userId={userId}
                            comment={comment}
                            setCurrentContentLength={setCurrentContentLength}
                            currentContentLength={currentContentLength}
                            maxContentLength={maxContentLengthRef.current}
                            updateCommunityPostComment={updatePostComment}
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

export default CommunityPostComments;