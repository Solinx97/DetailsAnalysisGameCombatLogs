import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';

export interface IMessageEvents {
    groupChatEvents: { 
        handleMessageRead: (messageId: string) => void;
        handleMessage: (message: GroupChatMessageModel) => void;
        handleMessageTextEdit: (mssageId: string, newMessage: string) => void;
        handleMessageMarkEdit: (mssageId: string, newMark: number) => void;
    },
    personalChatEvents: { 
        handleMessageRead: (messageId: string) => void;
        handleMessage: (message: PersonalChatMessageModel) => void;
        handleMessageTextEdit: (mssageId: string, newMessage: string) => void;
        handleMessageMarkEdit: (mssageId: string, newMark: number) => void;
    }
}