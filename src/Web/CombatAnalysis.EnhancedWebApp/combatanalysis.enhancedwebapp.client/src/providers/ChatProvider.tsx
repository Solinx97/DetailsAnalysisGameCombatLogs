import ChatHubContext from '@/context/ChatHubContext';
import useVoiceChatHub from '@/shared/hooks/useVoiceChatHub';
import useGroupChatHub from '@/shared/hooks/useGroupChatHub';
import usePersonalChatHub from '@/shared/hooks/usePersonalChatHub';
import * as signalR from '@microsoft/signalr';
import { useRef } from 'react';

const ChatProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const personalChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatUnreadMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const groupChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatUnreadMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const voiceChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const personalChatEvents = usePersonalChatHub(personalChatHubConnectionRef, personalChatMessagesHubConnectionRef, personalChatUnreadMessagesHubConnectionRef);

    const groupChatEvents = useGroupChatHub(groupChatHubConnectionRef, groupChatMessagesHubConnectionRef, groupChatUnreadMessagesHubConnectionRef);

    const vocieChatEvents = useVoiceChatHub(voiceChatHubConnectionRef);

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnectionRef, personalChatMessagesHubConnectionRef, personalChatUnreadMessagesHubConnectionRef,
            personalChatEvents,
            groupChatHubConnectionRef, groupChatMessagesHubConnectionRef, groupChatUnreadMessagesHubConnectionRef,
            groupChatEvents,
            voiceChatHubConnectionRef,
            vocieChatEvents
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export default ChatProvider;