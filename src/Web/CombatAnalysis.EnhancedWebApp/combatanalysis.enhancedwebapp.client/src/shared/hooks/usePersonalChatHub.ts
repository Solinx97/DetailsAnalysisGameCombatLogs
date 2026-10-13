import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { PersonalChatMessageModel } from '../../features/chat/types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../features/chat/types/PersonalChatModel';
import type { IPersonalChatHubEvents } from '../interfaces/IPersonalChatHubEvents';

const usePersonalChatHub = (
    personalChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    personalChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    personalChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>
): IPersonalChatHubEvents => {
    const personalChatHubURL = APP_CONFIG.chatApiHubs.personalChat;
    const personalChatMessagesHubURL = APP_CONFIG.chatApiHubs.personalChatMessages;
    const personalChatUnreadMessagesHubURL = APP_CONFIG.chatApiHubs.personalChatUnreadMessages;
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
            const connection = createHubConnection(personalChatHubURL);

            await connection.start();

            personalChatHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat hub", e);
        }
    }

    const connectToChatMessagesAsync = async (myPersonalChatId: number) => {
        try {
            const connection = createHubConnection(personalChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinToRoom", myPersonalChatId);

            personalChatMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat messages hub", e);
        }
    }

    const connectToChatUnreadMessagesAsync = async (myPersonalChatId: number) => {
        try {
            const connection = createHubConnection(personalChatUnreadMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinToRoom", myPersonalChatId);

            personalChatUnreadMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat messages hub", e);
        }
    }

    const subscribeToChat = (callback: (chat: PersonalChatModel) => void) => {
        personalChatHubConnectionRef.current?.on("ReceivePersonalChat", callback);
    }
    
    const unsubscribeToChat = (callback: (chat: PersonalChatModel) => void) => {
        personalChatHubConnectionRef.current?.off("ReceivePersonalChat", callback);
    }

    const subscribeToChatMessages = (callback: (message: PersonalChatMessageModel) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveMessage", callback);
    }

    const unsubscribeToChatMessages = (callback: (message: PersonalChatMessageModel) => void) => {
        personalChatMessagesHubConnectionRef.current?.off("ReceiveMessage", callback);
    }

    const subscribeToChatUnreadMessages = (callback: () => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveUnreadMessage", callback);
    }

    const unsubscribeToChatUnreadMessages = (callback: () => void) => {
        personalChatMessagesHubConnectionRef.current?.off("ReceiveUnreadMessage", callback);
    }

    const subscribeToRemovedFromChat = (callback: (appUserId: string, chatId: number) => void) => {
        personalChatHubConnectionRef.current?.on("UserRemoved", callback);
    }

    const unsubscribeToRemovedFromChat = (callback: (appUserId: string, chatId: number) => void) => {
        personalChatHubConnectionRef.current?.off("UserRemoved", callback);
    }

    const subscribeToChatMessageTextEdit = (callback: (mssageId: string, newMessage: string) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveTextEditedMessage", callback);
    }

    const unsubscribeToChatMessageTextEdit = (callback: (mssageId: string, newMessage: string) => void) => {
        personalChatMessagesHubConnectionRef.current?.off("ReceiveTextEditedMessage", callback);
    }

    const subscribeToChatMessageMarkEdit = (callback: (mssageId: string, newMark: number) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveMarkEditedMessage", callback);
    }

    const unsubscribeToChatMessageMarkEdit = (callback: (mssageId: string, newMark: number) => void) => {
        personalChatMessagesHubConnectionRef.current?.off("ReceiveMarkEditedMessage", callback);
    }

    const subscribeToMessageRead = (callback: (messageId: string) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveReadMessage", callback);
    }

    const unsubscribeToMessageRead = (callback: (messageId: string) => void) => {
        personalChatMessagesHubConnectionRef.current?.off("ReceiveReadMessage", callback);
    }

    const disconnectFromChatHubAsync = async () => {
        await personalChatHubConnectionRef.current?.stop();
        personalChatHubConnectionRef.current = null;
    }

    const disconnectFromChatMessageHubAsync = async () => {
        await personalChatMessagesHubConnectionRef.current?.stop();
        personalChatMessagesHubConnectionRef.current = null;
    }

    const disconnectFromChatUnreadMessageHubAsync = async () => {
        await personalChatUnreadMessagesHubConnectionRef.current?.stop();
        personalChatUnreadMessagesHubConnectionRef.current = null;
    }

    const getEvents = () => {
        const events: IPersonalChatHubEvents = {
            connectToChatAsync, connectToChatMessagesAsync, connectToChatUnreadMessagesAsync,
            subscribeToChat, unsubscribeToChat,
            subscribeToChatMessages, unsubscribeToChatMessages, subscribeToChatUnreadMessages, unsubscribeToChatUnreadMessages,
            subscribeToRemovedFromChat, unsubscribeToRemovedFromChat,
            subscribeToChatMessageTextEdit, unsubscribeToChatMessageTextEdit, subscribeToChatMessageMarkEdit, unsubscribeToChatMessageMarkEdit,
            subscribeToMessageRead, unsubscribeToMessageRead, 
            disconnectFromChatHubAsync, disconnectFromChatMessageHubAsync, disconnectFromChatUnreadMessageHubAsync
        }

        return events;
    }

    return getEvents();
}

export default usePersonalChatHub;