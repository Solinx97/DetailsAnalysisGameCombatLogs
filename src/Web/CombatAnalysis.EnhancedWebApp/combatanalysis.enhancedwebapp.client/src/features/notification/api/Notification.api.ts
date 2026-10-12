import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { AppNotificationModel } from '../types/AppNotificationModel';
import type { ReadNotificationModel } from '../types/ReadNotificationModel';

const apiURL = '/api/v1';

export const NotificationApi = createApi({
    reducerPath: 'notificationApi',
    tagTypes: [
        'Notification',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        removeNotification: builder.mutation<void, number>({
            query: id => ({
                url: `/Notification/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'Notification', id }],
        }),
        readNotification: builder.mutation<void, { id: number, notification: ReadNotificationModel }>({
            query: ({ id, notification }) => ({
                body: notification,
                url: `/Notification/read/${id}`,
                method: 'PATCH'
            }),
            invalidatesTags: () => [{ type: 'Notification', id: 'LIST' }]
        }),
        readAllNotifications: builder.mutation<void, string>({
            query: appUserId => ({
                url: `/Notification/readAll/${appUserId}`,
                method: 'PATCH'
            }),
            invalidatesTags: () => [{ type: 'Notification', id: 'LIST' }]
        }),
        getNotificationsByRecipientId: builder.query<AppNotificationModel[], string>({
            query: recipientId => ({
                url: `/Notification/getByRecipientId/${recipientId}`,
            }),
            providesTags: result =>
                result
                    ? [
                        ...result.map(notification => ({ type: 'Notification' as const, id: notification.id })),
                        { type: 'Notification', id: 'LIST' },
                    ]
                    : [{ type: 'Notification', id: 'LIST' }]
        }),
        getUnreadNotificationsByRecipientId: builder.query<AppNotificationModel[], string>({
            query: recipientId => ({
                url: `/Notification/getUnreadByRecipientId/${recipientId}`,
            }),
            providesTags: result =>
                result
                    ? [
                        ...result.map(notification => ({ type: 'Notification' as const, id: notification.id })),
                        { type: 'Notification', id: 'LIST' },
                    ]
                    : [{ type: 'Notification', id: 'LIST' }]
        }),
    })
})

export const {
    useRemoveNotificationMutation,
    useReadNotificationMutation,
    useReadAllNotificationsMutation,
    useGetNotificationsByRecipientIdQuery,
    useGetUnreadNotificationsByRecipientIdQuery,
} = NotificationApi;