import * as signalR from '@microsoft/signalr';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { ChatMessagePatch } from '../types/patches/ChatMessagePatch';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';

import './ChatMessage.scss';

const messageType = {
    default: 0,
    system: 1,
    log: 2
};

interface ChatMessageProps {
    message: PersonalChatMessageModel | GroupChatMessageModel;
    updateMessageAsync: (message: ChatMessagePatch) => Promise<void>;
    hubConnection: signalR.HubConnection | null;
    subscribeToChatMessageHasBeenRead: (callback: (messageId: string) => void) => void;
    lastReadMessageId?: string;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ message, updateMessageAsync, hubConnection, subscribeToChatMessageHasBeenRead, lastReadMessageId }) => {
    return (
        <>
            {message.type === messageType["default"]
                ? <DefaultChatMessage
                    message={message}
                    updateMessageAsync={updateMessageAsync}
                    subscribeToChatMessageHasBeenRead={subscribeToChatMessageHasBeenRead}
                    hubConnection={hubConnection}
                    lastReadMessageId={lastReadMessageId}
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