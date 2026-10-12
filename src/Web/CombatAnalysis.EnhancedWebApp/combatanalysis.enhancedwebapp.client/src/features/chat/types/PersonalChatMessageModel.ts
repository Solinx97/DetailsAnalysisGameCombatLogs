export type PersonalChatMessageModel = {
    id: string;
    username: string;
    message: string;
    time: string;
    status: number;
    type: number;
    markedType: number;
    isEdited: boolean;
    personalChatId: number;
    appUserId: string;
}