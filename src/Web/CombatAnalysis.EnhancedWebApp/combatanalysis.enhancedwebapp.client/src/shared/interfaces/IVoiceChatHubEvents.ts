export interface IVoiceChatHubEvents {
    connectToChatAsync: (roomId: string) => Promise<void>;
    leaveFromChatAsync: (roomId: string) => Promise<void>;
}