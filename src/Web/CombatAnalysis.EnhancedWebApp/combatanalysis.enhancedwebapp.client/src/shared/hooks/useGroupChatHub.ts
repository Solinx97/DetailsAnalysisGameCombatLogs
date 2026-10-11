import { APP_CONFIG } from '@/config/appConfig';
import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';
import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import type { IGroupChatHubEvents } from '../interfaces/IGroupChatHubEvents';

const useGroupChatHub = (
    groupChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
): IGroupChatHubEvents => {
    const groupChatHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.groupChat}`;
    const groupChatMessagesHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.groupChatMessages}`;
    const groupChatUnreadMessagesHubURL = `${APP_CONFIG.chatApiHubs.url}${APP_CONFIG.chatApiHubs.groupChatUnreadMessage}`;

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

    const connectToGroupChatAsync = async () => {
        try {
            const connection = createHubConnection(groupChatHubURL);

            await connection.start();

            groupChatHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat hub", e);
        }
    }

    const connectToGroupChatMessagesAsync = async (chatId: number) => {
        try {
            const connection = createHubConnection(groupChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinToRoom", chatId);

            groupChatMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat messages hub", e);
        }
    }

    const connectToGroupChatUnreadMessagesAsync = async (myGroupChatsId: number[]) => {
        try {
            const connection = createHubConnection(groupChatUnreadMessagesHubURL);

            await connection.start();
            for (let i = 0; i < myGroupChatsId.length; i++) {
                await connection.invoke("JoinToRoom", myGroupChatsId[i]);
            }

            groupChatUnreadMessagesHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat unread messages hub", e);
        }
    }

    const subscribeToGroupChat = (callback: (groupChatUser: GroupChatUserModel) => void) => {
        groupChatHubConnectionRef.current?.on("ReceiveJoinedUser", (groupChatUser: GroupChatUserModel) => {
            callback(groupChatUser);
        });
    }

    const subscribeToRemovedFromGroupChat = (callback: (appUserId: string, chatId: number) => void) => {
        groupChatHubConnectionRef.current?.on("UserRemoved", (appUserId: string, chatId: number) => {
            callback(appUserId, chatId);
        });
    }

    const subscribeToGroupChatMessages = (callback: (message: GroupChatMessageModel) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessage", callback);
    }

    const unsubscribeToGroupChatMessages = (callback: (message: GroupChatMessageModel) => void) => {
        groupChatMessagesHubConnectionRef.current?.off("ReceiveMessage", callback);
    }

    const subscribeToGroupChatMessageEdit = (callback: (mssagePatch: ChatMessagePatch) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveEditedMessage", (mssagePatch: ChatMessagePatch) => {
            callback(mssagePatch);
        });
    }

    const subscribeToGroupMessageDelivered = (chatId: number) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessageDelivered", async () => {
            await groupChatMessagesHubConnectionRef.current?.invoke("SendUnreadMessageUpdated", chatId);
        });
    }

    const subscribeToUnreadGroupMessagesUpdated = (callback: (targetChatId: number, targetMeInChatId: string, count: number) => void) => {
        groupChatUnreadMessagesHubConnectionRef.current?.on("ReceiveUnreadMessage", (targetChatId: number, targetMeInChatId: string, count: number) => {
            callback(targetChatId, targetMeInChatId, count);
        });
    }

    const subscribeToGroupMessageRead = (callback: (messageId: string) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveReadMessage", callback);
    }

    const subscribeToGroupChatMembers = (callback: (members: GroupChatUserModel[]) => void) => {
        groupChatHubConnectionRef.current?.on("ReceiveMembers", (message) => {
            callback(message);
        });
    }

    const disconnectFromGroupChatHubAsync = async () => {
        await groupChatHubConnectionRef.current?.stop();
        groupChatHubConnectionRef.current = null;
    }

    const disconnectFromGroupChatMessageHubAsync = async () => {
        await groupChatMessagesHubConnectionRef.current?.stop();
        groupChatMessagesHubConnectionRef.current = null;
    }

    const disconnectFromGroupChatUnreadMessagesHubAsync = async () => {
        await groupChatUnreadMessagesHubConnectionRef.current?.stop();
        groupChatUnreadMessagesHubConnectionRef.current = null;
    }

    const getEvents = () => {
        const events: IGroupChatHubEvents = {
            connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
            subscribeToGroupChat, subscribeToRemovedFromGroupChat, subscribeToGroupChatMessages, unsubscribeToGroupChatMessages, subscribeToGroupChatMessageEdit, 
            subscribeToGroupMessageDelivered, subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageRead,
            disconnectFromGroupChatHubAsync, disconnectFromGroupChatMessageHubAsync, disconnectFromGroupChatUnreadMessagesHubAsync,
            subscribeToGroupChatMembers,
        }

        return events;
    }

    return getEvents();
}

export default useGroupChatHub;