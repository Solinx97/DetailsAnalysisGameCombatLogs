export interface GroupChatMessage {
    id: number;
    username: string;
    message: string;
    time: string;
    status: number;
    type: number;
    chatId: number;
    groupChatUserId: string;
}