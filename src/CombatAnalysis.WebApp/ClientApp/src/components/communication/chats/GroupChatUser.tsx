import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { GroupChatUserProps } from '../../../types/components/communication/chats/GroupChatUserProps';

const GroupChatUser: React.FC<GroupChatUserProps> = ({ userId }) => {
    const { data: user, isLoading } = useGetUserByIdQuery(userId);

    if (isLoading) {
        return (<></>);
    }

    return (
        <div className="group-chat-user">
            {user?.username}
        </div>
    );
}

export default GroupChatUser;