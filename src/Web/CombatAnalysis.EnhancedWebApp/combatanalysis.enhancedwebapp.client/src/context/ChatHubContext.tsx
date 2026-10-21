import type { IGroupChatHubEvents } from '@/shared/interfaces/IGroupChatHubEvents';
import type { IPersonalChatHubEvents } from '@/shared/interfaces/IPersonalChatHubEvents';
import type { IVoiceChatHubEvents } from '@/shared/interfaces/IVoiceChatHubEvents';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import React from 'react';

interface ChatHubContextValue {
    personalChatHubConnectionRef: RefObject<signalR.HubConnection | null>;
    personalChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    personalChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    groupChatHubConnectionRef: RefObject<signalR.HubConnection | null>;
    groupChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    groupChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>;
    voiceChatHubConnectionRef: RefObject<signalR.HubConnection | null>;
    personalChatEvents: IPersonalChatHubEvents;
    groupChatEvents: IGroupChatHubEvents;
    vocieChatEvents: IVoiceChatHubEvents;
}

const ChatHubContext = React.createContext<ChatHubContextValue | null>(null);

export default ChatHubContext;