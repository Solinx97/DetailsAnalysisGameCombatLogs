export type GroupChatMessageModel = {
    id: number;
    username: string;
    message: string;
    time: Date;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    groupChatId: number;
    groupChatUserId: string;
}