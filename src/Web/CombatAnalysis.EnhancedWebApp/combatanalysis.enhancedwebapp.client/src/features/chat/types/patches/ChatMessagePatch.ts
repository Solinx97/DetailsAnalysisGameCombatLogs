export type ChatMessagePatch = {
    id: string;
    chatId: number;
    message?: string;
    status?: number;
    markedType?: number;
}