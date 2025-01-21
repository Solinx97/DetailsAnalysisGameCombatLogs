import "../../../styles/communication/chats/chatMessage.scss";
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const ChatMessage = ({ me, meInChatId, reviewerId, messageOwnerId, message, updateChatMessageAsync, deleteMessageAsync, chatMessagesHubConnection, subscribeToMessageHasBeenRead }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    me={me}
                    meInChatId={meInChatId}
                    reviewerId={reviewerId}
                    messageOwnerId={messageOwnerId}
                    message={message}
                    updateChatMessageAsync={updateChatMessageAsync}
                    deleteMessageAsync={deleteMessageAsync}
                    chatMessagesHubConnection={chatMessagesHubConnection}
                    subscribeToMessageHasBeenRead={subscribeToMessageHasBeenRead}
                />
                : message.type === messageType["log"]
                    ? <LogChatMessage
                        message={message}
                    />
                    : <SystemChatMessage
                        message={message}
                    />
            }
        </>
    );
}

export default ChatMessage;