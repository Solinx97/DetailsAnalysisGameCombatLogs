export interface IVoiceChatHubEvents {
    connectToChatAsync: (roomId: string) => Promise<void>;
    connectToVoiceChatUsersCount: (roomId: string) => Promise<number>;
    subscribeToVoiceChatUsersCount: (callback: (count: number) => void) => void;
    unsubscribeToVoiceChatUsersCount: (callback: (count: number) => void) => void;
    leaveFromChatAsync: (roomId: string) => Promise<void>;
    leaveFromChatUsersCountAsync: (roomId: string) => Promise<void>;
}