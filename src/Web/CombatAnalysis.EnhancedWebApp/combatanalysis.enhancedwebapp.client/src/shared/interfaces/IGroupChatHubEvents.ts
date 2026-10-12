import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';
import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';

export interface IGroupChatHubEvents {
    connectToChatAsync: () => Promise<void>;
    connectToChatMessagesAsync: (chatId: number) => Promise<void>;
    subscribeToChat: (callback: (groupChatUser: GroupChatUserModel) => void) => void;
    unsubscribeToChat: (callback: (groupChatUser: GroupChatUserModel) => void) => void;
    subscribeToRemovedFromChat: (callback: (appUserId: string, chatId: number) => void) => void;
    unsubscribeToRemovedFromChat: (callback: (appUserId: string, chatId: number) => void) => void;
    subscribeToChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    unsubscribeToChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    subscribeToChatMessageEdit: (callback: (mssagePatch: ChatMessagePatch) => void) => void;
    unsubscribeToChatMessageEdit: (callback: (mssagePatch: ChatMessagePatch) => void) => void;
    subscribeToMessageRead: (callback: (messageId: string) => void) => void;
    unsubscribeToMessageRead: (callback: (messageId: string) => void) => void;
    disconnectFromChatHubAsync: () => Promise<void>;
    disconnectFromChatMessageHubAsync: () => Promise<void>;
}