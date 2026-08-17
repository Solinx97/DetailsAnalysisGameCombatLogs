import { APP_CONFIG } from '@/config/appConfig';
import { useGetCommunityDiscussionCommentByDiscussionIdQuery } from '../../../api/CommunityDiscussionComment.api';
import DiscussionCommentContent from './DiscussionCommentContent';
import DiscussionCommentTitle from './DiscussionCommentTitle';
import { useEffect, useRef, useState } from 'react';

interface DiscussionCommentsProps {
    userId: string;
    discussionId: number;
}

const DiscussionComments: React.FC<DiscussionCommentsProps> = ({ userId, discussionId }) => {
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.posCommentSize ?? 5);

    const { data: discussionComments, isLoading } = useGetCommunityDiscussionCommentByDiscussionIdQuery({ discussionId, page: 1, pageSize: 5 });

    useEffect(() => {
        if (!discussionComments) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < discussionComments.count);
    }, [page, discussionComments]);

    if (!discussionComments || isLoading) {
        return (<div>Loading...</div>);
    }

    return (
        <>
            <ul className="post-comments">
                {discussionComments.comments.map((item) => (
                    <li key={item.id} className="post-comments__card">
                        <DiscussionCommentTitle
                            myselfId={userId}
                            discussionId={discussionId}
                            comment={item}
                        />
                        <DiscussionCommentContent
                            userId={userId}
                            comment={item}
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

export default DiscussionComments;