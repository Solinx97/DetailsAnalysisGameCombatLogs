import { useGetCommunityDiscussionCommentByDiscussionIdQuery } from '../../../store/api/communication/community/CommunityDiscussionComment.api';
import Loading from '../../Loading';
import DiscussionCommentContent from './DiscussionCommentContent';
import DiscussionCommentTitle from './DiscussionCommentTitle';

const DiscussionComments = ({ dateFormatting, customerId, discussionId }) => {
    const { data: discussionComments, isLoading } = useGetCommunityDiscussionCommentByDiscussionIdQuery(discussionId);

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <ul className="post-comments">
            {discussionComments?.map((item) => (
                <li key={item.id} className="post-comments__card">
                    <DiscussionCommentTitle
                        meId={customerId}
                        comment={item}
                        dateFormatting={dateFormatting}
                    />
                    <DiscussionCommentContent
                        customerId={customerId}
                        comment={item}
                    />
                </li>
            ))
            }
        </ul>
    );
}

export default DiscussionComments;