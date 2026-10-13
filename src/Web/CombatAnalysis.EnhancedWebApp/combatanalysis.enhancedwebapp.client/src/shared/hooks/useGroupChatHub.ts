import { APP_CONFIG } from '@/config/appConfig';
import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { IGroupChatHubEvents } from '../interfaces/IGroupChatHubEvents';

const useGroupChatHub = (
    groupChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>
): IGroupChatHubEvents => {
    const groupChatHubURL = APP_CONFIG.chatApiHubs.groupChat;
    const groupChatMessagesHubURL = APP_CONFIG.chatApiHubs.groupChatMessages;
    const groupChatUnreadMessagesHubURL = APP_CONFIG.chatApiHubs.groupChatUnreadMessages;
    const useLongPoll = import.meta.env.VITE_USE_HUB_LOONG_POLL === "true" ? true : false;

    const createHubConnection = (url: string): signalR.HubConnection => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(url, {
                withCredentials: true,
                transport: useLongPoll
                    ? signalR.HttpTransportType.LongPolling
                    : signalR.HttpTransportType.LongPolling | signalR.HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();

        return hubConnection;
    }

    const connectToChatAsync = async () => {
        try {
            const connection = createHubConnection(groupChatHubURL);

            await connection.start();

            groupChatHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat hub", e);
        }
    }

    const connectToChatMessagesAsync = async (chatId: number) => {
        try {
            const connection = createHubConnection(groupChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinToRoom", chatId);

            groupChatMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat messages hub", e);
        }
    }

    const connectToChatUnreadMessagesAsync = async (chatId: number) => {
        try {
            const connection = createHubConnection(groupChatUnreadMessagesHubURL);
            
            await connection.start();
            await connection.invoke("JoinToRoom", chatId);
            
            groupChatUnreadMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat messages hub", e);
        }
    }

    const subscribeToChat = (callback: (groupChatUser: GroupChatUserModel) => void) => {
        groupChatHubConnectionRef.current?.on("ReceiveJoinedUser", callback);
    }

    const unsubscribeToChat = (callback: (groupChatUser: GroupChatUserModel) => void) => {
        groupChatHubConnectionRef.current?.off("ReceiveJoinedUser", callback);
    }

    const subscribeToRemovedFromChat = (callback: (appUserId: string, chatId: number) => void) => {
        groupChatHubConnectionRef.current?.on("UserRemoved", callback);
    }

    const unsubscribeToRemovedFromChat = (callback: (appUserId: string, chatId: number) => void) => {
        groupChatHubConnectionRef.current?.off("UserRemoved", callback);
    }

    const subscribeToChatMessages = (callback: (message: GroupChatMessageModel) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessage", callback);
    }

    const unsubscribeToChatMessages = (callback: (message: GroupChatMessageModel) => void) => {
        groupChatMessagesHubConnectionRef.current?.off("ReceiveMessage", callback);
    }

    const subscribeToChatUnreadMessages = (callback: () => void) => {
        groupChatUnreadMessagesHubConnectionRef.current?.on("ReceiveUnreadMessage", callback);
    }

    const unsubscribeToChatUnreadMessages = (callback: () => void) => {
        groupChatUnreadMessagesHubConnectionRef.current?.off("ReceiveUnreadMessage", callback);
    }

    const subscribeToChatMessageTextEdit = (callback: (mssageId: string, newMessage: string) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveTextEditedMessage", callback);
    }

    const unsubscribeToChatMessageTextEdit = (callback: (mssageId: string, newMessage: string) => void) => {
        groupChatMessagesHubConnectionRef.current?.off("ReceiveTextEditedMessage", callback);
    }

    const subscribeToChatMessageMarkEdit = (callback: (mssageId: string, newMark: number) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMarkEditedMessage", callback);
    }

    const unsubscribeToChatMessageMarkEdit = (callback: (mssageId: string, newMark: number) => void) => {
        groupChatMessagesHubConnectionRef.current?.off("ReceiveMarkEditedMessage", callback);
    }

    const subscribeToMessageRead = (callback: (messageId: string) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveReadMessage", callback);
    }

    const unsubscribeToMessageRead = (callback: (messageId: string) => void) => {
        groupChatMessagesHubConnectionRef.current?.off("ReceiveReadMessage", callback);
    }

    const disconnectFromChatHubAsync = async () => {
        await groupChatHubConnectionRef.current?.stop();
        groupChatHubConnectionRef.current = null;
    }

    const disconnectFromChatMessageHubAsync = async () => {
        await groupChatMessagesHubConnectionRef.current?.stop();
        groupChatMessagesHubConnectionRef.current = null;
    }

    const disconnectFromChatUnreadMessageHubAsync = async () => {
        await groupChatUnreadMessagesHubConnectionRef.current?.stop();
        groupChatUnreadMessagesHubConnectionRef.current = null;
    }

    const getEvents = () => {
        const events: IGroupChatHubEvents = {
            connectToChatAsync, connectToChatMessagesAsync, connectToChatUnreadMessagesAsync, 
            subscribeToChat, unsubscribeToChat, 
            subscribeToRemovedFromChat, unsubscribeToRemovedFromChat, subscribeToChatMessages, unsubscribeToChatMessages,
            subscribeToChatUnreadMessages, unsubscribeToChatUnreadMessages,
            subscribeToChatMessageTextEdit, unsubscribeToChatMessageTextEdit, subscribeToChatMessageMarkEdit, unsubscribeToChatMessageMarkEdit, 
            subscribeToMessageRead, unsubscribeToMessageRead, disconnectFromChatHubAsync, disconnectFromChatMessageHubAsync, disconnectFromChatUnreadMessageHubAsync
        }

        return events;
    }

    return getEvents();
}

export default useGroupChatHub;