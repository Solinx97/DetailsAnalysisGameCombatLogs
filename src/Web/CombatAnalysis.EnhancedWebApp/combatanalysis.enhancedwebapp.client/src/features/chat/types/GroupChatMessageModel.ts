export type GroupChatMessageModel = {
    id: string;
    username: string;
    message: string;
    time: string;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    groupChatId: number;
    groupChatUserId: string;
}