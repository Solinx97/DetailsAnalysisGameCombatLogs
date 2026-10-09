import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';

const apiURL = '/api/v1';

export const ChatApi = createApi({
    reducerPath: 'chatApi',
    tagTypes: [
        'PersonalChat',
        'PersonalChatMessage',
        'GroupChat',
        'GroupChatMessage',
        'UnreadGroupChatMessage',
        'GroupChatUser',
        'GroupChatRules',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getMessagesByGroupChatId: builder.query<GroupChatMessageModel[], { chatId: number, page: number, pageSize: number }>({
            query: ({ chatId, page, pageSize }) => ({
                url: `/GroupChatMessage/getByChatId/${chatId}?page=${page}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: GroupChatMessageModel[]) => response.reverse(),
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.chatId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(d => d.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.chatId !== previousArg?.chatId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result =>
                result
                    ? [
                        ...result.map(chatMessage => ({ type: 'GroupChatMessage' as const, id: chatMessage.id })),
                        { type: 'GroupChatMessage', id: 'LIST' },
                    ]
                    : [{ type: 'GroupChatMessage', id: 'LIST' }],
        }),
        getMessagesByPersonalChatId: builder.query<PersonalChatMessageModel[], { chatId: number, page: number, pageSize: number }>({
            query: ({ chatId, page, pageSize }) => ({
                url: `/PersonalChatMessage/getByChatId/${chatId}?page=${page}&pageSize=${pageSize}`,
            }),
            transformResponse: (response: PersonalChatMessageModel[]) => response.reverse(),
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.chatId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(d => d.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.chatId !== previousArg?.chatId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result =>
                result
                    ? [
                        ...result.map(chatMessage => ({ type: 'PersonalChatMessage' as const, id: chatMessage.id })),
                        { type: 'PersonalChatMessage', id: 'LIST' },
                    ]
                    : [{ type: 'PersonalChatMessage', id: 'LIST' }],
        }),
    })
})

export const {
    useGetMessagesByGroupChatIdQuery,
    useLazyGetMessagesByGroupChatIdQuery,
    useGetMessagesByPersonalChatIdQuery,
    useLazyGetMessagesByPersonalChatIdQuery,
} = ChatApi;