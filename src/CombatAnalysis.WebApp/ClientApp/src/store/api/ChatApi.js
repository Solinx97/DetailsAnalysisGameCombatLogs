import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const ChatApi = createApi({
    reducerPath: 'chatApi',
    tagTypes: [
        'GroupChatMessage',
        'PersonalChatMessage',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getMessagesByGroupChatId: builder.query({
            query: ({ chatId, pageSize }) => ({
                url: `/GroupChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
            }),
            transformResponse: (response) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage', id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMoreMessagesByGroupChatId: builder.query({
            query: ({ chatId, offset, pageSize }) => ({
                url: `/GroupChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            transformResponse: (response) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'GroupChatMessage', id })), { type: 'GroupChatMessage' }]
                    : [{ type: 'GroupChatMessage' }]
        }),
        getMessagesByPersonalChatId: builder.query({
            query: ({ chatId, pageSize }) => ({
                url: `/PersonalChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
            }),
            transformResponse: (response) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PersonalChatMessage', id })), { type: 'PersonalChatMessage' }]
                    : [{ type: 'PersonalChatMessage' }]
        }),
        getMoreMessagesByPersonalChatId: builder.query({
            query: ({ chatId, offset, pageSize }) => ({
                url: `/PersonalChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            transformResponse: (response) => response.reverse(),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'PersonalChatMessage', id })), { type: 'PersonalChatMessage' }]
                    : [{ type: 'PersonalChatMessage' }]
        }),
    })
})

export const {
    useGetMessagesByGroupChatIdQuery,
    useLazyGetMessagesByGroupChatIdQuery,
    useGetMoreMessagesByGroupChatIdQuery,
    useLazyGetMoreMessagesByGroupChatIdQuery,
    useGetMessagesByPersonalChatIdQuery,
    useLazyGetMessagesByPersonalChatIdQuery,
    useGetMoreMessagesByPersonalChatIdQuery,
    useLazyGetMoreMessagesByPersonalChatIdQuery,
} = ChatApi;