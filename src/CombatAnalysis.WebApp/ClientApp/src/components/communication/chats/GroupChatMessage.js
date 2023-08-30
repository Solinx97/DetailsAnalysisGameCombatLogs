import { useFindUnreadGroupChatMessageQuery, useFindUnreadGroupChatMessageByMessageIdQuery } from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';
import ChatMessage from "./ChatMessage";

const status = {
    "delivery": 0,
    "delivered": 1,
    "read": 2
};

const GroupChatMessage = ({ me, meInChat, message, updateMessageAsync, deleteMessageAsync }) => {
    const { data: unreadMessageForUser, isLoading } = useFindUnreadGroupChatMessageQuery({ messageId: message?.id, groupChatUserId: meInChat?.id });
    const { data: unreadMessageForMessage, isLoading: unreadMessageIsLoading } = useFindUnreadGroupChatMessageByMessageIdQuery(message?.id);

    if (isLoading || unreadMessageIsLoading) {
        return <></>;
    }

    const handleUpdateMessageAsync = async (message, count) => {
        if (count === 0) {
            return;
        }

        if (message.status === status["read"] && unreadMessageForMessage.length > 1) {
            message.status = 1;
        }

        await updateMessageAsync(message, count);
    }

    return (
        <ChatMessage
            me={me}
            message={message}
            messageStatus={unreadMessageForUser !== null ? 1 : 2}
            updateMessageAsync={handleUpdateMessageAsync}
            deleteMessageAsync={deleteMessageAsync}
        />
    );
}

export default GroupChatMessage;