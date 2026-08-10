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
    const { data: postComments, isLoading } = useGetUserPostCommentByUserPostIdQuery({ userPostId: postId, page: 1, pageSize: 5 });

    if (isLoading) {
        return (<div>Loading...</div>);
    }

    if (postComments?.length === 0) {
        return (<></>);
    }

    return (
        <ul className="post-comments">
            {postComments?.map((comment) => (
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
    );
}

export default UserPostComments;