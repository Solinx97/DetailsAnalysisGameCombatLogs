import { useGetGroupChatUserByIdQuery } from '../../../store/api/chat/GroupChatUser.api';
import ChatMessage from './ChatMessage';

const GroupChatMessage = ({ me, meInChat, messageOwnerId, message, updateGroupChatMessageAsync, deleteMessageAsync, hubConnection, unreadMessageHubConnection }) => {
    const { data: groupChatUser, isLoading } = useGetGroupChatUserByIdQuery(messageOwnerId);

    if (isLoading) {
        return <></>;
    }

    return (
        <ChatMessage
            me={me}
            reviewerId={meInChat}
            messageOwnerId={groupChatUser.appUserId}
            message={message}
            updateChatMessageAsync={updateGroupChatMessageAsync}
            deleteMessageAsync={deleteMessageAsync}
            hubConnection={hubConnection}
            unreadMessageHubConnection={unreadMessageHubConnection}
        />
    );
}

export default GroupChatMessage;