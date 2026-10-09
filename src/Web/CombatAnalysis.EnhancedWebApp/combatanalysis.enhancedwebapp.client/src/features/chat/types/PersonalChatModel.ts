export type PersonalChatModel = {
    id: number;
    initiatorId: string;
    initiatorUnreadMessages?: number;
    companionId: string;
    companionUnreadMessages?: number;
}