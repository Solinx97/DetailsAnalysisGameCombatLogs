import type { INotificationHubEvents } from '@/shared/interfaces/INotificationHubEvents';
import * as signalR from '@microsoft/signalr';
import type { RefObject } from 'react';
import React from 'react';

interface NotificationHubContextValue {
    notificationHubConnectionRef: RefObject<signalR.HubConnection | null>;
    notificationEvents: INotificationHubEvents;
}

const NotificationHubContext = React.createContext<NotificationHubContextValue | null>(null);

export default NotificationHubContext;