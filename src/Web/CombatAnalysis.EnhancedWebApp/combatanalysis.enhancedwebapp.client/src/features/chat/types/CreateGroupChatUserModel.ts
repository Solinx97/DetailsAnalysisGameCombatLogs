export type CreateGroupChatUserModel = {
    id: string;
    username: string;
    unreadMessages: number;
    lastReadMessageId?: string;
    groupChatId: number;
    appUserId: string;
    whoAddId: string;
}