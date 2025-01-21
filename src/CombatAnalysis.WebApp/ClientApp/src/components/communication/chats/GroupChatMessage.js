import { useGetGroupChatUserByIdQuery } from '../../../store/api/chat/GroupChatUser.api';
import ChatMessage from './ChatMessage';

const GroupChatMessage = ({ me, reviewerId, messageOwnerId, message, updateGroupChatMessageAsync, deleteMessageAsync, chatMessagesHubConnection, subscribeToMessageHasBeenRead }) => {
    const { data: groupChatUser, isLoading } = useGetGroupChatUserByIdQuery(messageOwnerId);

    if (isLoading) {
        return (<></>);
    }

    return (
        <ChatMessage
            me={me}
            meInChatId={groupChatUser.appUserId}
            reviewerId={reviewerId}
            messageOwnerId={messageOwnerId}
            message={message}
            updateChatMessageAsync={updateGroupChatMessageAsync}
            deleteMessageAsync={deleteMessageAsync}
            chatMessagesHubConnection={chatMessagesHubConnection}
            subscribeToMessageHasBeenRead={subscribeToMessageHasBeenRead}
        />
    );
}

export default GroupChatMessage;