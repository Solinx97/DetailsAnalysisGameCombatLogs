import type { RootState } from '@/app/Store';
import Store from '@/app/Store';
import NotificationHubContext from '@/context/NotificationHubContext';
import logger from '@/utils/Logger';
import { faBell, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { NotificationApi, useGetNotificationsByRecipientIdQuery, useRemoveNotificationMutation } from '../api/Notification.api';
import type { AppNotificationModel } from '../types/AppNotificationModel';

const Notification: React.FC = () => {
    const { t } = useTranslation('notification');

    const navigate = useNavigate();

    const user = useSelector((state: RootState) => state.user.value);

    const context = useContext(NotificationHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const { data: notifciations } = useGetNotificationsByRecipientIdQuery(user ? user.id : "0");

    const [removeNotification] = useRemoveNotificationMutation();

    const [showNotifications, setShowNotifications] = useState(false);

    useEffect(() => {
        let isCancelled = false;

        const handle = (notification: AppNotificationModel) => {
            if (!user) {
                return;
            }

            Store.dispatch(
                NotificationApi.util.updateQueryData("getNotificationsByRecipientId", user.id, (draft) => {
                    draft.push(notification);
                })
            );
        }

        const connect = async () => {
            if (isCancelled) {
                return;
            }

            await context.notificationEvents.connectToNotificationAsync();
            context.notificationEvents.subscribeToNotifications(handle);
        }

        connect();

        return () => {
            isCancelled = true;

            context.notificationEvents.unsubscribeToNotifications(handle);
            context.notificationEvents.disconnectNotificationsHubAsync();
        };
    }, []);

    const personalChatNotifications = (notification: AppNotificationModel) => {
        const navigateToChats = async () => await seeNotificationAsync();
        const removeNotification = async () => await removeNotificationAsync(notification.id);

        return (
            <li className="notification-message" key={notification.id}>
                <div className="notification-message__time">{getNotificationTime(notification)}</div>
                <div>{t("PersonalChatNotification")} '{notification.initiatorName}'. <span className="notifications-container__see" onClick={navigateToChats}>{t("See")}</span></div>
                <FontAwesomeIcon
                    icon={faXmark}
                    onClick={removeNotification}
                />
            </li>
        );
    }

    const getNotificationTime = (notification: AppNotificationModel) => {
        const date = new Date(notification.createdAt);
        const time = `${date.getHours()}:${date.getMinutes()}`;

        return time;
    }

    const seeNotificationAsync = async () => {
        if (!user) {
            return;
        }

        navigate('/chats');
    }

    const removeNotificationAsync = async (notificationId: number) => {
        if (!user) {
            return;
        }

        try {
            await removeNotification(notificationId).unwrap();

            Store.dispatch(
                NotificationApi.util.updateQueryData("getNotificationsByRecipientId", user.id, draft => {
                    const notification = draft.find(x => x.id === notificationId);
                    if (notification) {
                        draft.unshift(notification);
                    }
                })
            );
        } catch (error) {
            logger.error("Failed to remove notification:", error);
        }
    }

    const readAllNotifications = async () => {
        await context.notificationHubConnectionRef.current?.invoke("ReadRecipientNotifications", user?.id);
    }

    return (
        <div className="notifications">
            <div className="notifications__alert">
                <FontAwesomeIcon
                    icon={faBell}
                    title={t("Notifications") || ""}
                    onClick={() => setShowNotifications(!showNotifications)}
                />
                {(notifciations && notifciations.length > 0) &&
                    <div className="notifications-count">
                        <div>{notifciations.length > 9 ? '9+' : notifciations.length}</div>
                    </div>
                }
            </div>
            {showNotifications &&
                <div className="notifications-container">
                    <div className="notifications-container__title">{t("Notifications")}</div>
                    <ul className="notifications-container__content">
                        {!notifciations || notifciations.length === 0
                            ? <li className="notifications-container__empty">{t("Empty")}</li>
                            : notifciations && notifciations.map((notification) => personalChatNotifications(notification))}
                    </ul>
                    {(notifciations && notifciations.length > 0) &&
                        <div className="notifications-container__read-all" onClick={readAllNotifications}>{t("ClearAll")}</div>
                    }
                </div>
            }
        </div>
    );
}

export default Notification;