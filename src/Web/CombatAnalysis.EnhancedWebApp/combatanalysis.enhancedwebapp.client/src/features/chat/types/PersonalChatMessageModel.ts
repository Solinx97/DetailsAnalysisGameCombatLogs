export type PersonalChatMessageModel = {
    id: string;
    username: string;
    message: string;
    time: Date;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    personalChatId: number;
    appUserId: string;
}