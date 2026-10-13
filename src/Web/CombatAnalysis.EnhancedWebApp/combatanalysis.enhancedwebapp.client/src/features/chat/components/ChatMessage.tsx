import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import DefaultChatMessage from './DefaultChatMessage';
import LogChatMessage from './LogChatMessage';
import SystemChatMessage from './SystemChatMessage';
import { ChatMessageType } from '@/shared/helpers/EnumHelper';

import './ChatMessage.scss';

interface ChatMessageProps {
    message: PersonalChatMessageModel | GroupChatMessageModel;
    updateMessageTextAsync: (messageId: string, newMessage: string | undefined) => Promise<void>;
    updateMessageMarkAsync: (messageId: string, mark: number) => Promise<void>;
    lastReadMessageId?: string;
}

const ChatMessage: React.FC<ChatMessageProps> = ({ message, updateMessageTextAsync, updateMessageMarkAsync, lastReadMessageId }) => {
    return (
        <>
            {message.type === ChatMessageType["DEFAULT"]
                ? <DefaultChatMessage
                    message={message}
                    updateMessageTextAsync={updateMessageTextAsync}
                    updateMessageMarkAsync={updateMessageMarkAsync}
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