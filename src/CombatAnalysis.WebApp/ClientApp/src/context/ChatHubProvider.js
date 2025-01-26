import * as signalR from '@microsoft/signalr';
import React, { createContext, useContext, useEffect, useState } from 'react';
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

    useEffect(() => {
        if (!me) {
            return;
        }

        connectToPersonalChatAsync().then(async () => {
            await connectToGroupChatAsync();
        });

        return () => {
            const stopConnection = async () => {
                if (personalChatHubConnection) {
                    await personalChatHubConnection.stop();
                }

                if (groupChatHubConnection) {
                    await groupChatHubConnection.stop();
                }
            }

            stopConnection();
        }
    }, [me]);

    const createHubConnection = (url) => {
        return new signalR.HubConnectionBuilder()
            .withUrl(url, {
                withCredentials: true,
                transports: ['websocket', 'polling'],
            })
            .withAutomaticReconnect()
            .build();
    }

    const connectToPersonalChatAsync = async () => {
        try {
            const connection = createHubConnection(personalChatHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", me.id);

            setPersonalChatHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatMessagesAsync = async (chatId) => {
        try {
            const connection = createHubConnection(personalChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", chatId);

            setPersonalChatMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToPersonalChatUnreadMessagesAsync = async (meInChats) => {
        try {
            const connection = createHubConnection(personalChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < meInChats.length; i++) {
                await connection.invoke("JoinRoom", meInChats[i].id);
            }

            setPersonalChatUnreadMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatAsync = async () => {
        try {
            const connection = createHubConnection(groupChatHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", me.id);

            setGroupChatHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatMessagesAsync = async (chatId) => {
        try {
            const connection = createHubConnection(groupChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", chatId);

            setGroupChatMessagesHubConnection(connection);
        } catch (e) {
            console.error(e);
        }
    }

    const connectToGroupChatUnreadMessagesAsync = async (meInChats) => {
        try {
            const connection = createHubConnection(groupChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < meInChats.length; i++) {
                await connection.invoke("JoinRoom", meInChats[i].chatId);
            }

            setGroupChatUnreadMessagesHubConnection(connection);
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

    const subscribeToGroupMessageDelivered = (chatId) => {
        groupChatMessagesHubConnection?.on("ReceiveMessageDelivered", async () => {
            await groupChatUnreadMessagesHubConnection?.invoke("SendUnreadMessageUpdated", chatId);
        });
    }

    const subscribeToUnreadGroupMessagesUpdated = (meInChatId, callback) => {
        groupChatUnreadMessagesHubConnection?.on("ReceiveUnreadMessageUpdated", async (chatId) => {
            await groupChatUnreadMessagesHubConnection?.invoke("RequestUnreadMessages", chatId, meInChatId);
        });

        groupChatUnreadMessagesHubConnection?.on("ReceiveUnreadMessage", (targetChatId, targetMeInChatId, count) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToGroupMessageHasBeenRead = (chatId, reviewerId) => {
        groupChatMessagesHubConnection?.on("ReceiveMessageHasBeenRead", async () => {
            await groupChatUnreadMessagesHubConnection?.invoke("RequestUnreadMessages", chatId, reviewerId);
        });
    }

    return (
        <ChatHubContext.Provider value={{
            personalChatHubConnection, personalChatMessagesHubConnection, personalChatUnreadMessagesHubConnection,
            connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
            subscribeToPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalMessageHasBeenRead, subscribeToPersonalMessageDelivered, subscribeToUnreadPersonalMessagesUpdated,
            subscribeToGroupChat, subscribeGroupChatUser, subscribeToGroupChatMessages, subscribeToGroupMessageHasBeenRead, subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageDelivered,
            groupChatHubConnection, groupChatMessagesHubConnection, groupChatUnreadMessagesHubConnection,
            connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync
        }}>
            {children}
        </ChatHubContext.Provider>
    );
}

export const useChatHub = () => useContext(ChatHubContext);