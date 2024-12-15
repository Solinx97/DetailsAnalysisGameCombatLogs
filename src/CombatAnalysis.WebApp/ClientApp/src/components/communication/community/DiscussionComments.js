import { useGetCommunityDiscussionCommentByDiscussionIdQuery } from '../../../store/api/community/CommunityDiscussionComment.api';
import Loading from '../../Loading';
import DiscussionCommentContent from './DiscussionCommentContent';
import DiscussionCommentTitle from './DiscussionCommentTitle';

const DiscussionComments = ({ dateFormatting, userId, discussionId }) => {
    const { data: discussionComments, isLoading } = useGetCommunityDiscussionCommentByDiscussionIdQuery(discussionId);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <ul className="post-comments">
            {discussionComments?.map((item) => (
                <li key={item.id} className="post-comments__card">
                    <DiscussionCommentTitle
                        meId={userId}
                        comment={item}
                        dateFormatting={dateFormatting}
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