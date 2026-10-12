import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { AppNotificationModel } from '../types/AppNotificationModel';

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
    useGetNotificationsByRecipientIdQuery,
    useLazyGetUnreadNotificationsByRecipientIdQuery,
} = NotificationApi;