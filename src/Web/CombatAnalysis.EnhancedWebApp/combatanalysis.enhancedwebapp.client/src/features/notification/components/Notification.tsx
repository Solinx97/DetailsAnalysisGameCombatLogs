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
import { NotificationApi, useGetNotificationsByRecipientIdQuery } from '../api/Notification.api';
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

    const [showNotifications, setShowNotifications] = useState(false);

    useEffect(() => {
        (async () => {
            await context.notificationEvents.connectToNotificationAsync();

            context.notificationEvents.subscribeToNotifications((notification: AppNotificationModel) => {
                if (user) {
                    Store.dispatch(
                        NotificationApi.util.updateQueryData("getNotificationsByRecipientId", user.id, (draft) => {
                            draft.push(notification);
                        })
                    );
                }
            });
        })();

        return () => {
            (async () => {
                await context.notificationEvents.disconnectNotificationsHubAsync();
            })();
        }
    }, []);

    const personalChatNotifications = (notification: AppNotificationModel) => {
        const navigateToChats = async () => await seeNotificationAsync(notification);
        const removeNotification = async () => await removeNotificationAsync(notification);

        return (
            <li className="notification-message" key={notification.id}>
                <div className="notification-message__time">{getNotificationTime(notification)}</div>
                <div>{t("PersonalChatNotification")} '{notification.initiatorName}'. <span className="notifications-container__see" onClick={navigateToChats}>{t("See")}</span></div>
                <FontAwesomeIcon
                    icon={faXmark}
                    title={t("Remove") || ""}
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

    const seeNotificationAsync = async (notification: AppNotificationModel) => {
        if (!user) {
            return;
        }

        context.notificationHubConnectionRef.current?.invoke("ReadNotification", notification.id, user.id)
            .then(() => navigate(`/chats?personal=${notification.initiatorId}`))
            .catch((e) => logger.error("Faield to read notification", e));
    }

    const removeNotificationAsync = async (notification: AppNotificationModel) => {
        if (!user) {
            return;
        }

        await context.notificationHubConnectionRef.current?.invoke("RemoveNotification", notification.id, user.id);
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