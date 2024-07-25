import "../../../styles/communication/chats/chatMessage.scss";
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const ChatMessage = ({ me, message, messageStatus, updateChatMessageAsync, deleteMessageAsync, decreaseChatMessagesCountAsync }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    me={me}
                    message={message}
                    messageStatus={messageStatus}
                    updateChatMessageAsync={updateChatMessageAsync}
                    deleteMessageAsync={deleteMessageAsync}
                    decreaseChatMessagesCountAsync={decreaseChatMessagesCountAsync}
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