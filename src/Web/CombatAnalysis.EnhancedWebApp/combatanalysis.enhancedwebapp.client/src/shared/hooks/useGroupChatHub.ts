import { APP_CONFIG } from '@/config/appConfig';
import type { GroupChatMessageModel } from '@/features/chat/types/GroupChatMessageModel';
import type { GroupChatUserModel } from '@/features/chat/types/GroupChatUserModel';
import type { ChatMessagePatch } from '@/features/chat/types/patches/ChatMessagePatch';
import type { AppUserModel } from '@/features/user/types/AppUserModel';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';

const useGroupChatHub = (
    myself: AppUserModel | null,
    groupChatHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
    groupChatUnreadMessagesHubConnectionRef: RefObject<signalR.HubConnection | null>,
) => {
    const groupChatHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChat}`;
    const groupChatMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChatMessages}`;
    const groupChatUnreadMessagesHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.groupChatUnreadMessage}`;

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
            await connection.invoke("JoinRoom", myself?.id);

            groupChatHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Group chat hub", e);
        }
    }

    const connectToGroupChatMessagesAsync = async (chatId: number) => {
        try {
            const connection = createHubConnection(groupChatMessagesHubURL);

            await connection.start();
            await connection.invoke("JoinRoom", chatId);

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
                await connection.invoke("JoinRoom", myGroupChatsId[i]);
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

    const subscribeToGroupChatMessages = (callback: (message: GroupChatMessageModel) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessage", (message: GroupChatMessageModel) => {
            callback(message);
        });
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

    const subscribeToGroupMessageHasBeenRead = (callback: (messageId: number) => void) => {
        groupChatMessagesHubConnectionRef.current?.on("ReceiveMessageHasBeenRead", (messageId: number) => {
            callback(messageId);
        });
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

    return {
        connectToGroupChatAsync, connectToGroupChatMessagesAsync, connectToGroupChatUnreadMessagesAsync,
        subscribeToGroupChat, subscribeToGroupChatMessages, subscribeToGroupChatMessageEdit, subscribeToGroupMessageDelivered,
        subscribeToUnreadGroupMessagesUpdated, subscribeToGroupMessageHasBeenRead,
        disconnectFromGroupChatHubAsync, disconnectFromGroupChatMessageHubAsync, disconnectFromGroupChatUnreadMessagesHubAsync,
        subscribeToGroupChatMembers,
    }
}

export default useGroupChatHub;