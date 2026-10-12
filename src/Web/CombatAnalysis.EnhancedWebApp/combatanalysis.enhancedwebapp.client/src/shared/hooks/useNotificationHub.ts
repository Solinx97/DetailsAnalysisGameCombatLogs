import { APP_CONFIG } from '@/config/appConfig';
import type { AppNotificationModel } from '@/features/notification/types/AppNotificationModel';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import { type RefObject } from 'react';
import type { INotificationHubEvents } from '../interfaces/INotificationHubEvents';

const useNotificationHub = (
    notificationHubConnectionRef: RefObject<signalR.HubConnection | null>,
) : INotificationHubEvents => {
    const notificationHubURL = `${APP_CONFIG.notificationApiHubs.url}${APP_CONFIG.notificationApiHubs.notification}`;

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

    const connectToNotificationAsync = async () => {
        try {
            const connection = createHubConnection(notificationHubURL);

            await connection.start();

            notificationHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Notification hub", e);
        }
    }

    const subscribeToNotifications = (callback: (notification: AppNotificationModel) => void) => {
        notificationHubConnectionRef.current?.on("ReceiveNotification", callback);
    }

    const unsubscribeToNotifications = (callback: (notification: AppNotificationModel) => void) => {
        notificationHubConnectionRef.current?.off("ReceiveNotification", callback);
    }

    const disconnectNotificationsHubAsync = async () => {
        await notificationHubConnectionRef.current?.stop();
        notificationHubConnectionRef.current = null;
    }

    const getEvents = () => {
        const events: INotificationHubEvents = {
            connectToNotificationAsync, subscribeToNotifications, unsubscribeToNotifications, disconnectNotificationsHubAsync
        }

        return events;
    }

    return getEvents();
}

export default useNotificationHub;