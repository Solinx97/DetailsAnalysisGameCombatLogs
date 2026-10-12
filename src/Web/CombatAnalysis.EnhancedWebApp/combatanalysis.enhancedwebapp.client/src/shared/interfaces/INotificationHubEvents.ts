import type { AppNotificationModel } from '@/features/notification/types/AppNotificationModel';

export interface INotificationHubEvents {
    connectToNotificationAsync: () => Promise<void>;
    subscribeToNotifications: (callback: (notification: AppNotificationModel) => void) => void;
    subscribeToRecipientNotifications: (callback: () => void) => void;
    disconnectNotificationsHubAsync: () => Promise<void>;
}