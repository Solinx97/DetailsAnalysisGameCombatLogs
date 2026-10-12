import { useGetNotificationsByRecipientIdQuery, useGetUnreadNotificationsByRecipientIdQuery } from '../api/Notification.api';

const useNotifications = (showOnlyUnreadNotifications: boolean, userId: string) => {
    const { data: unreadNotifciations } = useGetUnreadNotificationsByRecipientIdQuery(userId, {
        skip: !showOnlyUnreadNotifications
    });
    const { data: notifciations } = useGetNotificationsByRecipientIdQuery(userId, {
        skip: showOnlyUnreadNotifications
    });

    return showOnlyUnreadNotifications 
        ? unreadNotifciations 
        : notifciations
}

export default useNotifications;