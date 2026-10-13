import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';

export interface IGroupChatHubEvents {
    connectToChatAsync: () => Promise<void>;
    connectToChatMessagesAsync: (chatId: number) => Promise<void>;
    connectToChatUnreadMessagesAsync: (chatId: number) => Promise<void>;
    subscribeToChat: (callback: (groupChatUser: GroupChatUserModel) => void) => void;
    unsubscribeToChat: (callback: (groupChatUser: GroupChatUserModel) => void) => void;
    subscribeToRemovedFromChat: (callback: (appUserId: string, chatId: number) => void) => void;
    unsubscribeToRemovedFromChat: (callback: (appUserId: string, chatId: number) => void) => void;
    subscribeToChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    unsubscribeToChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    subscribeToChatUnreadMessages: (callback: () => void) => void;
    unsubscribeToChatUnreadMessages: (callback: () => void) => void;
    subscribeToChatMessageTextEdit: (callback: (mssageId: string, newMessage: string) => void) => void;
    unsubscribeToChatMessageTextEdit: (callback: (mssageId: string, newMessage: string) => void) => void;
    subscribeToChatMessageMarkEdit: (callback: (mssageId: string, newMark: number) => void) => void;
    unsubscribeToChatMessageMarkEdit: (callback: (mssageId: string, newMark: number) => void) => void;
    subscribeToMessageRead: (callback: (messageId: string) => void) => void;
    unsubscribeToMessageRead: (callback: (messageId: string) => void) => void;
    disconnectFromChatHubAsync: () => Promise<void>;
    disconnectFromChatMessageHubAsync: () => Promise<void>;
    disconnectFromChatUnreadMessageHubAsync: () => Promise<void>;
}