import DefaultChatMessage from './DefaultChatMessage';
import SystemChatMessage from './SystemChatMessage';

import "../../../styles/communication/chats/chatMessage.scss";

const messageType = {
    default: 0,
    system: 1
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
                : <SystemChatMessage
                    message={message}
                />
            }
        </>
    );
}

export default ChatMessage;