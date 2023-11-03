import "../../../styles/communication/chats/chatMessage.scss";
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const ChatMessage = ({ me, message, messageStatus, updateMessageAsync, deleteMessageAsync }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    me={me}
                    message={message}
                    messageStatus={messageStatus}
                    updateMessageAsync={updateMessageAsync}
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