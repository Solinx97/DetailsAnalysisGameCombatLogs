import type { RootState } from '@/app/Store';
import Store from '@/app/Store';
import NotificationHubContext from '@/context/NotificationHubContext';
import { NotificationStatus } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { faBell, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useContext, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { NotificationApi, useReadAllNotificationsMutation, useReadNotificationMutation, useRemoveNotificationMutation } from '../api/Notification.api';
import useNotifications from '../hooks/useNotifications';
import type { AppNotificationModel } from '../types/AppNotificationModel';
import type { ReadNotificationModel } from '../types/ReadNotificationModel';

import './Notification.scss';

const Notification: React.FC = () => {
    const { t } = useTranslation('notification');

    const navigate = useNavigate();

    const user = useSelector((state: RootState) => state.user.value);

    const context = useContext(NotificationHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const [readNotification] = useReadNotificationMutation();
    const [readAllNotifications] = useReadAllNotificationsMutation();
    const [removeNotification] = useRemoveNotificationMutation();

    const [showNotifications, setShowNotifications] = useState(false);
    const [showOnlyUnreadNotifications, setShowOnlyUnreadNotifications] = useState(true);
    const [unreadNotificationsCount, setUnreadNotificationsCount] = useState(0);

    const notifications = useNotifications(showOnlyUnreadNotifications, user ? user.id : "0");

    useEffect(() => {
        let isCancelled = false;

        const handle = (notification: AppNotificationModel) => {
            if (!user) {
                return;
            }

            Store.dispatch(
                NotificationApi.util.updateQueryData("getUnreadNotificationsByRecipientId", user.id, (draft) => {
                    draft.push(notification);
                })
            );

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

    useEffect(() => {
        if (!notifications) {
            return;
        }

        setUnreadNotificationsCount(notifications.filter(x => x.status === NotificationStatus["UNREAD"])?.length);
    }, [notifications]);

    const personalChatNotifications = (notification: AppNotificationModel) => {
        const navigateToChats = async () => await seeNotificationAsync();
        const removeNotification = async () => await removeNotificationAsync(notification.id);

        return (
            <li key={notification.id} className={`notification-message ${notification.status == NotificationStatus["UNREAD"] ? 'unread' : 'read'}`}
                onMouseEnter={async () => await readNotificationAsync(notification.id)}>
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

    const readNotificationAsync = async (notificationId: number) => {
        if (!user) {
            return;
        }

        try {
            const notification: ReadNotificationModel = {
                id: notificationId,
                status: NotificationStatus["READ"]
            }

            await readNotification({ id: notificationId, notification }).unwrap();
        } catch (error) {
            logger.error("Failed to remove notification:", error);
        }
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

    const readAllNotificationsAsync = async () => {
        if (!user) {
            return;
        }

        try {
            await readAllNotifications(user.id).unwrap();
        } catch (error) {
            logger.error("Failed to read all notifications:", error);
        }
    }

    return (
        <div className="notifications">
            <div className="notifications__alert">
                <FontAwesomeIcon
                    icon={faBell}
                    color={showNotifications ? 'green' : 'white'}
                    onClick={() => setShowNotifications(!showNotifications)}
                />
                {unreadNotificationsCount > 0 &&
                    <div className="notifications-count">
                        <div>{unreadNotificationsCount > 9 ? '9+' : unreadNotificationsCount}</div>
                    </div>
                }
            </div>
            {showNotifications &&
                <div className="notifications-container">
                    <div className="notifications-container__title">
                        <div className={`${showOnlyUnreadNotifications ? 'active' : 'inactive'}`}
                            onClick={() => setShowOnlyUnreadNotifications(true)}>{t("Notifications")}</div>
                        <div className={`${showOnlyUnreadNotifications ? 'inactive' : 'active'}`}
                            onClick={() => setShowOnlyUnreadNotifications(false)}>{t("AllNotifications")}</div>
                    </div>
                    <ul className="notifications-container__content">
                        {(notifications && notifications.length === 0)
                            ? <li className="notifications-container__empty">{t("Empty")}</li>
                            : notifications && notifications.map((notification) => personalChatNotifications(notification))}
                    </ul>
                    {unreadNotificationsCount > 0 &&
                        <div className="notifications-container__read-all" onClick={readAllNotificationsAsync}>{t("ReadAll")}</div>
                    }
                </div>
            }
        </div>
    );
}

export default Notification;