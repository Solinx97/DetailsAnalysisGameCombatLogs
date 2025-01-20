import * as signalR from '@microsoft/signalr';
import React, { createContext, useContext, useState } from 'react';
import { useSelector } from 'react-redux';

const ChatHubContext = createContext();

const messageType = {
    default: 0,
    system: 1
};

export const ChatHubProvider = ({ children }) => {
    const personalChatHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_ADDRESS}`;
    const personalChatMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_MESSAGES_ADDRESS}`;
    const personalChatUnreadMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_UNREAD_MESSAGES_ADDRESS}`;
    const groupChatHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_ADDRESS}`;
    const groupChatMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_MESSAGES_ADDRESS}`;
    const groupChatUnreadMessagesHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_UNREAD_MESSAGES_ADDRESS}`;

    const me = useSelector((state) => state.user.value);

    const [personalChatHubConnection, setPersonalChatHubConnection] = useState(null);
    const [personalChatMessagesHubConnection, setPersonalChatMessagesHubConnection] = useState(null);
    const [personalChatUnreadMessagesHubConnection, setPersonalChatUnreadMessagesHubConnection] = useState(null);
    const [groupChatHubConnection, setGroupChatHubConnection] = useState(null);
    const [groupChatMessagesHubConnection, setGroupChatMessagesHubConnection] = useState(null);
    const [groupChatUnreadMessagesHubConnection, setGroupChatUnreadMessagesHubConnection] = useState(null);

    const connectToPersonalChatAsync = async (meId) => {
        if (personalChatHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(personalChatHubURL)
                .withAutomaticReconnect()
                .build();
            setPersonalChatHubConnection(hubConnection);

            await hubConnection.start();
            await hubConnection.invoke("JoinRoom", meId);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatMessagesAsync = async (chatId) => {
        if (personalChatMessagesHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(personalChatMessagesHubURL)
                .withAutomaticReconnect()
                .build();
            setPersonalChatMessagesHubConnection(hubConnection);

            await hubConnection.start();
            await hubConnection.invoke("JoinRoom", chatId);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatUnreadMessagesAsync = async (meInChats) => {
        if (personalChatUnreadMessagesHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(personalChatUnreadMessagesHubURL)
                .withAutomaticReconnect()
                .build();
            setPersonalChatUnreadMessagesHubConnection(hubConnection);

            await hubConnection.start();

            for (let i = 0; i < meInChats.length; i++) {
                await hubConnection.invoke("JoinRoom", meInChats[i].chatId);
            }
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatAsync = async (meId) => {
        if (groupChatHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(groupChatHubURL)
                .withAutomaticReconnect()
                .build();
            setGroupChatHubConnection(hubConnection);

            await hubConnection.start();
            await hubConnection.invoke("JoinRoom", meId);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatMessagesAsync = async (chatId) => {
        if (groupChatMessagesHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(groupChatMessagesHubURL)
                .withAutomaticReconnect()
                .build();
            setGroupChatMessagesHubConnection(hubConnection);

            await hubConnection.start();
            await hubConnection.invoke("JoinRoom", chatId);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatUnreadMessagesAsync = async (meInChats) => {
        if (groupChatUnreadMessagesHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(groupChatUnreadMessagesHubURL)
                .withAutomaticReconnect()
                .build();
            setGroupChatUnreadMessagesHubConnection(hubConnection);

            await hubConnection.start();

            for (let i = 0; i < meInChats.length; i++) {
                await hubConnection.invoke("JoinRoom", meInChats[i].chatId);
            }
        } catch (e) {
            console.error(e);
        }
    }

    const subscribeToPersonalChat = (callback) => {
        personalChatHubConnection?.on("ReceivePersonalChat", (chat) => {
            callback(chat);
        });
    }

    const subscribeToPersonalChatMessages = (callback) => {
        personalChatMessagesHubConnection?.on("ReceiveMessage", (message) => {
            callback(message);
        });
    }

    const subscribeToPersonalMessageHasBeenRead = (chatId, reviewerId) => {
        personalChatMessagesHubConnection?.on("ReceiveMessageHasBeenRead", async () => {
            await personalChatUnreadMessagesHubConnection?.invoke("RequestUnreadMessages", chatId, reviewerId);
        });
    }

    const subscribeToPersonalMessageDelivered = (chatId) => {
        personalChatMessagesHubConnection?.on("ReceiveMessageDelivered", async () => {
            await personalChatUnreadMessagesHubConnection?.invoke("SendUnreadMessageUpdated", chatId);
        });
    }

    const subscribeToUnreadPersonalMessagesUpdated = (meId, callback) => {
        personalChatUnreadMessagesHubConnection?.on("ReceiveUnreadMessageUpdated", async (chatId) => {
            await personalChatUnreadMessagesHubConnection?.invoke("RequestUnreadMessages", chatId, meId);
        });

        personalChatUnreadMessagesHubConnection?.on("ReceiveUnreadMessage", (targetChatId, targetMeInChatId, count) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToGroupChat = (callback) => {
        groupChatHubConnection?.on("ReceiveGroupChat", async (chatId, appUserId) => {
            await groupChatHubConnection?.invoke("RequestJoinedUser", chatId, appUserId);
        });

        groupChatHubConnection?.on("ReceiveJoinedUser", (groupChatUser) => {
            callback(groupChatUser);
        });
    }

    const subscribeGroupChatUser = () => {
        groupChatHubConnection?.on("ReceiveAddedUserToChat", async (groupChatUser) => {
            const systemMessage = `'${me?.username}' added '${groupChatUser.username}' to chat`;
            await groupChatMessagesHubConnection?.invoke("SendMessage", systemMessage, groupChatUser.chatId, messageType["system"], groupChatUser.id, groupChatUser.username);
            await groupChatHubConnection?.invoke("RequestJoinedUser", groupChatUser.chatId, groupChatUser.appUserId);
        });
    }

    const subscribeToGroupChatMessages = (callback) => {
        groupChatMessagesHubConnection?.on("ReceiveMessage", (message) => {
            callback(message);
        });
    }

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnection, personalChatMessagesHubConnection, personalChatUnreadMessagesHubConnection,
            connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
            subscribeToPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalMessageHasBeenRead, subscribeToPersonalMessageDelivered, subscribeToUnreadPersonalMessagesUpdated,
            subscribeToGroupChat, subscribeGroupChatUser, subscribeToGroupChatMessages,
            groupChatHubConnection, groupChatMessagesHubConnection, groupChatUnreadMessagesHubConnection,
            connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export const useChatHub = () => useContext(ChatHubContext);