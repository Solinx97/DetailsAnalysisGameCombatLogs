import ChatHubContext from '@/context/ChatHubContext';
import useGroupChatHub from '@/shared/hooks/useGroupChatHub';
import usePersonalChatHub from '@/shared/hooks/usePersonalChatHub';
import * as signalR from '@microsoft/signalr';
import { useRef } from 'react';

const ChatProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const personalChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const personalChatEvents = usePersonalChatHub(personalChatHubConnectionRef, personalChatMessagesHubConnectionRef);

    const groupChatEvents = useGroupChatHub(groupChatHubConnectionRef, groupChatMessagesHubConnectionRef);

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnectionRef, personalChatMessagesHubConnectionRef,
            personalChatEvents,
            groupChatHubConnectionRef, groupChatMessagesHubConnectionRef,
            groupChatEvents
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export default ChatProvider;