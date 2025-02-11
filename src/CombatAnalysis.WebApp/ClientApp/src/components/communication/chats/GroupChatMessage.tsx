import { useGetGroupChatUserByIdQuery } from '../../../store/api/chat/GroupChatUser.api';
import { GroupChatMessageProps } from '../../../types/components/communication/chats/GroupChatMessageProps';
import ChatMessage from './ChatMessage';

const GroupChatMessage: React.FC<GroupChatMessageProps> = ({ me, reviewerId, messageOwnerId, message, updateMessageAsync, deleteMessageAsync, chatMessagesHubConnection, subscribeToMessageHasBeenRead }) => {
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
            updateMessageAsync={updateMessageAsync}
            deleteMessageAsync={deleteMessageAsync}
            chatMessagesHubConnection={chatMessagesHubConnection}
            subscribeToMessageHasBeenRead={subscribeToMessageHasBeenRead}
        />
    );
}

export default GroupChatMessage;