import type { RootState } from '@/app/Store';
import { ChatHubContext } from '@/shared/hooks/useChatHub';
import useGroupChatHub from '@/shared/hooks/useGroupChatHub';
import usePersonalChatHub from '@/shared/hooks/usePersonalChatHub';
import * as signalR from '@microsoft/signalr';
import { useRef } from 'react';
import { useSelector } from 'react-redux';

const ChatHubProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const user = useSelector((state: RootState) => state.user.value);

    const personalChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const personalChatUnreadMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);
    const groupChatUnreadMessagesHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const {
        connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
        subscribeToPersonalChat, subscribeToRemovedFromPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalChatMessageEdit, subscribeToPersonalMessageHasBeenRead, subscribeToUnreadPersonalMessagesUpdated,
        disconnectFromPersonalChatHubAsync, disconnectFromPersonalChatMessageHubAsync, disconnectFromPersonalChatUnreadMessagesHubAsync,
    } = usePersonalChatHub(user, personalChatHubConnectionRef, personalChatMessagesHubConnectionRef, personalChatUnreadMessagesHubConnectionRef);

    const {
        connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
        subscribeToGroupChat, subscribeToRemovedFromGroupChat, subscribeToGroupChatMessages, subscribeToGroupChatMessageEdit, subscribeToGroupMessageDelivered,
        subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageHasBeenRead,
        disconnectFromGroupChatHubAsync, disconnectFromGroupChatMessageHubAsync, disconnectFromGroupChatUnreadMessagesHubAsync,
        subscribeToGroupChatMembers,
    } = useGroupChatHub(user, groupChatHubConnectionRef, groupChatMessagesHubConnectionRef, groupChatUnreadMessagesHubConnectionRef);

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnectionRef, personalChatMessagesHubConnectionRef, personalChatUnreadMessagesHubConnectionRef,
            connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
            subscribeToPersonalChat, subscribeToRemovedFromPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalChatMessageEdit, subscribeToPersonalMessageHasBeenRead, subscribeToUnreadPersonalMessagesUpdated,
            disconnectFromPersonalChatHubAsync, disconnectFromPersonalChatMessageHubAsync, disconnectFromPersonalChatUnreadMessagesHubAsync,
            groupChatHubConnectionRef, groupChatMessagesHubConnectionRef, groupChatUnreadMessagesHubConnectionRef,
            connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
            subscribeToGroupChat, subscribeToRemovedFromGroupChat, subscribeToGroupChatMessages, subscribeToGroupChatMessageEdit, subscribeToGroupMessageHasBeenRead, subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageDelivered,
            disconnectFromGroupChatHubAsync, disconnectFromGroupChatMessageHubAsync, disconnectFromGroupChatUnreadMessagesHubAsync,
            subscribeToGroupChatMembers,
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export default ChatHubProvider;