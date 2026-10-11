import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { ChatMessagePatch } from '../types/patches/ChatMessagePatch';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';
import { ChatMessageType } from '@/shared/helpers/EnumHelper';

import './ChatMessage.scss';

interface ChatMessageProps {
    message: PersonalChatMessageModel | GroupChatMessageModel;
    updateMessageAsync: (message: ChatMessagePatch) => Promise<void>;
    lastReadMessageId?: string;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ message, updateMessageAsync, lastReadMessageId }) => {
    return (
        <>
            {message.type === ChatMessageType["DEFAULT"]
                ? <DefaultChatMessage
                    message={message}
                    updateMessageAsync={updateMessageAsync}
                    lastReadMessageId={lastReadMessageId}
                />
                : message.type === ChatMessageType["LOG"]
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