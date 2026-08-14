import Loading from '@/shared/components/Loading';
import { useGetCommunityDiscussionCommentByDiscussionIdQuery } from '../../../api/CommunityDiscussionComment.api';
import DiscussionCommentContent from './DiscussionCommentContent';
import DiscussionCommentTitle from './DiscussionCommentTitle';

interface DiscussionCommentsProps {
    userId: string;
    discussionId: number;
}

const DiscussionComments: React.FC<DiscussionCommentsProps> = ({ userId, discussionId }) => {
    const { data: discussionComments, isLoading } = useGetCommunityDiscussionCommentByDiscussionIdQuery({ discussionId, page: 1, pageSize: 5 });

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <ul className="post-comments">
            {discussionComments?.map((item) => (
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
    );
}

export default DiscussionComments;