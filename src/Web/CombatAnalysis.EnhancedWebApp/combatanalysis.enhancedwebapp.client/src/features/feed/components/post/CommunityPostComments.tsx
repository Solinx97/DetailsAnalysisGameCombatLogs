import { useGetCommunityPostCommentByPostIdQuery } from '../../api/CommunityPostComment.api';
import CommunityPostCommentContent from './CommunityPostCommentContent';
import CommunityPostCommentTitle from './CommunityPostCommentTitle';

import './PostComments.scss';

interface CommunityPostCommentsProps {
    userId: string;
    postId: number;
}

const CommunityPostComments: React.FC<CommunityPostCommentsProps> = ({ userId, postId }) => {
    const { data: postComments, isLoading } = useGetCommunityPostCommentByPostIdQuery({ communityPostId: postId, page: 1, pageSize: 5 });

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    if (!postComments || postComments.length === 0) {
        return (<></>);
    }

    return (
        <ul className="post-comments">
            {postComments.map((comment) => (
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
    );
}

export default CommunityPostComments;