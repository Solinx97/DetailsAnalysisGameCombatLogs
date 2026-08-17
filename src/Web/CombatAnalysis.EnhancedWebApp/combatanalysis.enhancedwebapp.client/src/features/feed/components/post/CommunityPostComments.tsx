import { APP_CONFIG } from '@/config/appConfig';
import { useEffect, useRef, useState } from 'react';
import { useGetCommunityPostCommentByPostIdQuery } from '../../api/CommunityPostComment.api';
import CommunityPostCommentContent from './CommunityPostCommentContent';
import CommunityPostCommentTitle from './CommunityPostCommentTitle';

import './PostComments.scss';

interface CommunityPostCommentsProps {
    userId: string;
    postId: number;
}

const CommunityPostComments: React.FC<CommunityPostCommentsProps> = ({ userId, postId }) => {
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.posCommentSize ?? 5);

    const { data: postComments, isLoading } = useGetCommunityPostCommentByPostIdQuery({ communityPostId: postId, page, pageSize: pageSizeRef.current });

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
                        <CommunityPostCommentTitle
                            userId={userId}
                            comment={comment}
                        />
                        <CommunityPostCommentContent
                            userId={userId}
                            comment={comment}
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