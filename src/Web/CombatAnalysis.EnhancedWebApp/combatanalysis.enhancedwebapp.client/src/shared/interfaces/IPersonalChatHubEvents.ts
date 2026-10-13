import type { PersonalChatMessageModel } from '@/features/chat/types/PersonalChatMessageModel';
import type { PersonalChatModel } from '@/features/chat/types/PersonalChatModel';

export interface IPersonalChatHubEvents {
    connectToChatAsync: () => Promise<void>;
    connectToChatMessagesAsync: (myPersonalChatId: number) => Promise<void>;
    subscribeToChat: (callback: (chat: PersonalChatModel) => void) => void;
    unsubscribeToChat: (callback: (chat: PersonalChatModel) => void) => void;
    subscribeToChatMessages: (callback: (message: PersonalChatMessageModel) => void) => void;
    unsubscribeToChatMessages: (callback: (message: PersonalChatMessageModel) => void) => void;
    subscribeToRemovedFromChat: (callback: (appUserId: string, chatId: number) => void) => void;
    unsubscribeToRemovedFromChat: (callback: (appUserId: string, chatId: number) => void) => void;
    subscribeToChatMessageTextEdit: (callback: (mssageId: string, newMessage: string) => void) => void;
    unsubscribeToChatMessageTextEdit: (callback: (mssageId: string, newMessage: string) => void) => void;
    subscribeToChatMessageMarkEdit: (callback: (mssageId: string, newMark: number) => void) => void;
    unsubscribeToChatMessageMarkEdit: (callback: (mssageId: string, newMark: number) => void) => void;
    subscribeToMessageRead: (callback: (messageId: string) => void) => void;
    unsubscribeToMessageRead: (callback: (messageId: string) => void) => void;
    disconnectFromChatHubAsync: () => Promise<void>;
    disconnectFromChatMessageHubAsync: () => Promise<void>;
}