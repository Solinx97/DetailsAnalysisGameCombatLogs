import type { IGroupChatHubEvents } from '@/shared/interfaces/IGroupChatHubEvents';
import type { IPersonalChatHubEvents } from '@/shared/interfaces/IPersonalChatHubEvents';
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
    personalChatEvents: IPersonalChatHubEvents;
    groupChatEvents: IGroupChatHubEvents;
}

const ChatHubContext = React.createContext<ChatHubContextValue | null>(null);

export default ChatHubContext;