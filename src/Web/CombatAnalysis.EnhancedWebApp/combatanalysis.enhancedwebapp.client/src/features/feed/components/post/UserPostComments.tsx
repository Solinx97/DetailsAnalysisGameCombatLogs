import { APP_CONFIG } from '@/config/appConfig';
import { useEffect, useRef, useState } from 'react';
import { useGetUserPostCommentByUserPostIdQuery } from '../../api/UserPostComment.api';
import UserPostCommentContent from './UserPostCommentContent';
import UserPostCommentTitle from './UserPostCommentTitle';

import './PostComments.scss';

interface UserPostCommentsProps {
    userId: string;
    postId: number;
    dateFormatting: (stringOfDate: string) => string;
}

const UserPostComments: React.FC<UserPostCommentsProps> = ({ userId, postId, dateFormatting }) => {
    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const pageSizeRef = useRef<number>(APP_CONFIG.communication.posCommentSize ?? 5);

    const { data: postComments, isLoading } = useGetUserPostCommentByUserPostIdQuery({ userPostId: postId, page, pageSize: pageSizeRef.current });

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
                        <UserPostCommentTitle
                            userId={userId}
                            comment={comment}
                            dateFormatting={dateFormatting}
                        />
                        <UserPostCommentContent
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

export default UserPostComments;