import { APP_CONFIG } from '@/config/appConfig';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { PersonalChatMessageModel } from '../../features/chat/types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../features/chat/types/PersonalChatModel';
import type { AppUserModel } from '../../features/user/types/AppUserModel';
import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';

const usePersonalChatHub = (
    myself: AppUserModel | null,
    personalChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    personalChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    personalChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
) => {
    const personalChatHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.personalChat}`;
    const personalChatMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.personalChatMessages}`;
    const personalChatUnreadMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.personalChatUnreadMessage}`;

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
            await connection.invoke("JoinRoom", myself?.id);

            personalChatHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Personal chat hub", e);
        }
    }

    const connectToPersonalChatMessagesAsync = async (myPersonalChatId: number) => {
        try {
            const connection = createHubConnection(personalChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", myPersonalChatId);

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
                await connection.invoke("JoinRoom", myPersonalChatsId[i]);
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

    const subscribeToPersonalChatMessages = (callback: (message: PersonalChatMessageModel) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveMessage", (message: PersonalChatMessageModel) => {
            callback(message);
        });
    }

    const subscribeToPersonalChatMessageEdit = (callback: (mssagePatch: ChatMessagePatch) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveEditedMessage", (mssagePatch: ChatMessagePatch) => {
            callback(mssagePatch);
        });
    }

    const subscribeToPersonalMessageHasBeenRead = (callback: (messageId: number) => void) => {
        personalChatMessagesHubConnectionRef.current?.on("ReceiveMessageHasBeenRead", (messageId: number) => {
            callback(messageId);
        });
    }

    const subscribeToUnreadPersonalMessagesUpdated = (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => {
        personalChatUnreadMessagesHubConnectionRef.current?.on("ReceiveUnreadMessage", (targetChatId: number, targetMeInChatId: string, count: number) => {
            callback(targetChatId, targetMeInChatId, count);
        });
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

    return {
        connectToPersonalChatAsync, connectToPersonalChatMessagesAsync, connectToPersonalChatUnreadMessagesAsync,
        subscribeToPersonalChat, subscribeToPersonalChatMessages, subscribeToPersonalChatMessageEdit, subscribeToPersonalMessageHasBeenRead, subscribeToUnreadPersonalMessagesUpdated,
        disconnectFromPersonalChatHubAsync, disconnectFromPersonalChatMessageHubAsync, disconnectFromPersonalChatUnreadMessagesHubAsync
    }
}

export default usePersonalChatHub;