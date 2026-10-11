import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';
import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';

export interface IGroupChatHubEvents {
    connectToGroupChatAsync: () => Promise<void>;
    connectToGroupChatMessagesAsync: (chatId: number) => Promise<void>;
    connectToGroupChatUnreadMessagesAsync: (myGroupChatsId: number[]) => Promise<void>;
    subscribeToGroupChat: (callback: (groupChatUser: GroupChatUserModel) => void) => void;
    subscribeToRemovedFromGroupChat: (callback: (appUserId: string, chatId: number) => void) => void;
    subscribeToGroupChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    unsubscribeToGroupChatMessages: (callback: (message: GroupChatMessageModel) => void) => void;
    subscribeToGroupChatMessageEdit: (callback: (mssagePatch: ChatMessagePatch) => void) => void;
    subscribeToGroupMessageDelivered: (chatId: number) => void;
    subscribeToUnreadGroupMessagesUpdated: (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => void;
    subscribeToGroupMessageRead: (callback: (messageId: string) => void) => void;
    disconnectFromGroupChatHubAsync: () => Promise<void>;
    disconnectFromGroupChatMessageHubAsync: () => Promise<void>;
    disconnectFromGroupChatUnreadMessagesHubAsync: () => Promise<void>;
    subscribeToGroupChatMembers: (callback: (members: GroupChatUserModel[]) => void) => void;
}