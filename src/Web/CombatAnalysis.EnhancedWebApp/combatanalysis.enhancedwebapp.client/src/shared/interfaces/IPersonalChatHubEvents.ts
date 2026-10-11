import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';
import type { PersonalChatMessageModel } from '@/features/chat/types/PersonalChatMessageModel';
import type { PersonalChatModel } from '@/features/chat/types/PersonalChatModel';

export interface IPersonalChatHubEvents {
    connectToPersonalChatAsync: () => Promise<void>;
    connectToPersonalChatMessagesAsync: (myPersonalChatId: number) => Promise<void>;
    connectToPersonalChatUnreadMessagesAsync: (myPersonalChatsId: number[]) => Promise<void>;
    subscribeToPersonalChat: (callback: (chat: PersonalChatModel) => void) => void;
    subscribeToRemovedFromPersonalChat: (callback: (appUserId: string, chatId: number) => void) => void;
    subscribeToPersonalChatMessages: (callback: (message: PersonalChatMessageModel) => void) => void;
    unsubscribeToPersonalChatMessages: (callback: (message: PersonalChatMessageModel) => void) => void;
    subscribeToPersonalChatMessageEdit: (callback: (mssagePatch: ChatMessagePatch) => void) => void;
    subscribeToPersonalMessageRead: (callback: (messageId: string) => void) => void;
    subscribeToUnreadPersonalMessagesUpdated: (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => void;
    disconnectFromPersonalChatHubAsync: () => Promise<void>;
    disconnectFromPersonalChatMessageHubAsync: () => Promise<void>;
    disconnectFromPersonalChatUnreadMessagesHubAsync: () => Promise<void>;
}