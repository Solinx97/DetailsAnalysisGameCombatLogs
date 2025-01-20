import "../../../styles/communication/chats/chatMessage.scss";
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const ChatMessage = ({ me, reviewerId, messageOwnerId, message, updateChatMessageAsync, deleteMessageAsync }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    me={me}
                    reviewerId={reviewerId}
                    messageOwnerId={messageOwnerId}
                    message={message}
                    updateChatMessageAsync={updateChatMessageAsync}
                    deleteMessageAsync={deleteMessageAsync}
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