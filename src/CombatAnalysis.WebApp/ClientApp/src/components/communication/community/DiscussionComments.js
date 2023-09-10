import { useGetCommunityDiscussionCommentByDiscussionIdQuery } from '../../../store/api/communication/community/CommunityDiscussionComment.api';
import DiscussionCommentContent from './DiscussionCommentContent';
import DiscussionCommentTitle from './DiscussionCommentTitle';

const DiscussionComments = ({ dateFormatting, customerId, discussionId }) => {
    const { data: discussionComments, isLoading } = useGetCommunityDiscussionCommentByDiscussionIdQuery(discussionId);

    if (isLoading) {
        return <></>;
    }

    return (
        <div>
            <ul className="discussion-comments">
                {discussionComments?.map((item) => (
                    <li key={item.id} className="card">
                        <ul className="list-group list-group-flush">
                            <DiscussionCommentTitle
                                meId={customerId}
                                comment={item}
                                dateFormatting={dateFormatting}
                            />
                            <DiscussionCommentContent
                                customerId={customerId}
                                comment={item}
                            />
                        </ul>
                    </li>
                ))
                }
            </ul>
        </div>
    );
}

export default DiscussionComments;