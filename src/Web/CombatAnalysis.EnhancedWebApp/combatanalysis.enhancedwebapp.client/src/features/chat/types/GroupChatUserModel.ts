export type GroupChatUserModel = {
    id: string;
    username: string;
    unreadMessages: number;
    lastReadMessageId?: number;
    groupChatId: number;
    appUserId: string;
}