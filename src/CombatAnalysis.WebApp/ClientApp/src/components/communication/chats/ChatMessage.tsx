import { ChatMessageProps } from '../../../types/components/communication/chats/ChatMessageProps';
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

import '../../../styles/communication/chats/chatMessage.scss';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

const ChatMessage: React.FC<ChatMessageProps> = ({ me, meInChatId, reviewerId, messageOwnerId, message, updateMessageAsync, deleteMessageAsync, chatMessagesHubConnection, subscribeToMessageHasBeenRead }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    me={me}
                    meInChatId={meInChatId}
                    reviewerId={reviewerId}
                    messageOwnerId={messageOwnerId}
                    message={message}
                    updateMessageAsync={updateMessageAsync}
                    deleteMessageAsync={deleteMessageAsync}
                    chatMessagesHubConnection={chatMessagesHubConnection}
                    subscribeToMessageHasBeenRead={subscribeToMessageHasBeenRead}
                />
                : message.type === messageType["log"]
                    ? <LogChatMessage
                        message={message.message}
                    />
                    : <SystemChatMessage
                        message={message.message}
                    />
            }
        </>
    );
}

export default ChatMessage;