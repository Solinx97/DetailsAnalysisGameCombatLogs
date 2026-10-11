import { APP_CONFIG } from '@/config/appConfig';
import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { PersonalChatMessageModel } from '../../features/chat/types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../features/chat/types/PersonalChatModel';
import type { IPersonalChatHubEvents } from '../interfaces/IPersonalChatHubEvents';

const usePersonalChatHub = (
    personalChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    personalChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    personalChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
): IPersonalChatHubEvents => {
    const personalChatHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.personalChat}`;
    const personalChatMessagesHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.personalChatMessages}`;
    const personalChatUnreadMessagesHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.personalChatUnreadMessage}`;

    const createHubConnection = (url: string): signalR.HubConnection => {
        const hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(url, {
                withCredentials: true,
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
            })
            .withAutomaticReconnect()
            .build();

        return hubConnection;
    }

    const connectToPersonalChatAsync = async () => {
        try {
            const connection = createHubConnection(personalChatHubURL);

            await connection.start();

            personalChatHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat hub", e);
        }
    }

    const connectToPersonalChatMessagesAsync = async (myPersonalChatId: number) => {
        try {
            const connection = createHubConnection(personalChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinToRoom", myPersonalChatId);

            personalChatMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat messages hub", e);
        }
    }

    const connectToPersonalChatUnreadMessagesAsync = async (myPersonalChatsId: number[]) => {
        try {
            const connection = createHubConnection(personalChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < myPersonalChatsId.length; i++) {
                await connection.invoke("JoinToRoom", myPersonalChatsId[i]);
            }

            personalChatUnreadMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat unread messages hub", e);
        }
    }

    const subscribeToPersonalChat = (callback: (chat: PersonalChatModel) => void) => {
        personalChatHubConnectionRef.current?.on("ReceivePersonalChat", (chat: PersonalChatModel) => {
            callback(chat);
        });
    }

    const subscribeToRemovedFromPersonalChat = (callback: (appUserId: string, chatId: number) => void) => {
        personalChatHubConnectionRef.current?.on("UserRemoved", (appUserId: string, chatId: number) => {
            callback(appUserId, chatId);
        });
    }

    const subscribeToPersonalChatMessages = (callback: (message: PersonalChatMessageModel) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveMessage", callback);
    }

    const unsubscribeToPersonalChatMessages = (callback: (message: PersonalChatMessageModel) => void) => {
        personalChatMessagesHubConnectionRef.current?.off("ReceiveMessage", callback);
    }

    const subscribeToPersonalChatMessageEdit = (callback: (mssagePatch: ChatMessagePatch) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveEditedMessage", (mssagePatch: ChatMessagePatch) => {
            callback(mssagePatch);
        });
    }

    const subscribeToUnreadPersonalMessagesUpdated = (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => {
        personalChatUnreadMessagesHubConnectionRef.current?.on("ReceiveUnreadMessage", (targetChatId: number, targetMeInChatId: string, count: number) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToPersonalMessageRead = (callback: (messageId: string) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveReadMessage", callback);
    }

    const disconnectFromPersonalChatHubAsync = async () => {
        await personalChatHubConnectionRef.current?.stop();
        personalChatHubConnectionRef.current = null;
    }

    const disconnectFromPersonalChatMessageHubAsync = async () => {
        await personalChatMessagesHubConnectionRef.current?.stop();
        personalChatMessagesHubConnectionRef.current = null;
    }

    const disconnectFromPersonalChatUnreadMessagesHubAsync = async () => {
        await personalChatUnreadMessagesHubConnectionRef.current?.stop();
        personalChatUnreadMessagesHubConnectionRef.current = null;
    }

    const getEvents = () => {
        const events: IPersonalChatHubEvents = {
            connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
            subscribeToPersonalChat, subscribeToRemovedFromPersonalChat, subscribeToPersonalChatMessages, unsubscribeToPersonalChatMessages, subscribeToPersonalChatMessageEdit, subscribeToPersonalMessageRead, 
            subscribeToUnreadPersonalMessagesUpdated, disconnectFromPersonalChatHubAsync, disconnectFromPersonalChatMessageHubAsync, disconnectFromPersonalChatUnreadMessagesHubAsync
        }

        return events;
    }

    return getEvents();
}

export default usePersonalChatHub;