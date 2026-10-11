import type { RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import type { AppNotificationModel } from '@/features/notification/types/AppNotificationModel';
import logger from '@/utils/Logger';
import * as signalR from '@microsoft/signalr';
import { useRef } from 'react';
import { useSelector } from 'react-redux';
import { NotificationHubContext } from '../shared/hooks/useNotificationHub';

export const NotificationProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const notificationHubURL = `${APP_CONFIG.hubs.url}${APP_CONFIG.hubs.notification}`;

    const user = useSelector((state: RootState) => state.user.value);

    const notificationHubConnectionRef = useRef<signalR.HubConnection | null>(null);

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
            await connection.invoke("JoinToRoom", user?.id);

            notificationHubConnectionRef.current = connection;
        } catch (e) {
            logger.error("Failed to connect to Notification hub", e);
        }
    }

    const subscribeToNotifications = (callback: (notification: AppNotificationModel) => void) => {
        notificationHubConnectionRef.current?.on("ReceiveNotification", (notification: AppNotificationModel) => {
            callback(notification);
        });
    }

    const subscribeToRecipientNotifications = (callback: () => void) => {
        notificationHubConnectionRef.current?.on("ReceiveRequestRecipientNotifications", () => {
            callback();
        });
    }

    const disconnectNotificationsHubAsync = async () => {
        await notificationHubConnectionRef.current?.stop();
        notificationHubConnectionRef.current = null;
    }

    return (
        <NotificationHubContext.Provider value={{
            notificationHubConnectionRef, connectToNotificationAsync, subscribeToNotifications, subscribeToRecipientNotifications, disconnectNotificationsHubAsync
        }}>
            {children}
        </NotificationHubContext.Provider>
    );
}

export default NotificationProvider;