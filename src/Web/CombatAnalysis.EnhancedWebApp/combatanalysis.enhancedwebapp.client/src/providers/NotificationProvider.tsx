import NotificationHubContext from '@/context/NotificationHubContext';
import * as signalR from '@microsoft/signalr';
import { useRef } from 'react';
import useNotificationHub from '../shared/hooks/useNotificationHub';

export const NotificationProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const notificationHubConnectionRef = useRef<signalR.HubConnection | null>(null);

    const notificationEvents = useNotificationHub(notificationHubConnectionRef);

    return (
        <NotificationHubContext.Provider value={{
            notificationHubConnectionRef, notificationEvents
        }}>
            {children}
        </NotificationHubContext.Provider>
    );
}

export default NotificationProvider;