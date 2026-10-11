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
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.chatId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.length = 0;
                    currentCache.push(...newItems);
                    return;
                }

                newItems.forEach(item => {
                    const index = currentCache.findIndex(x => x.id === item.id);
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
            providesTags: result => [
                { type: 'GroupChatMessage', id: 'LIST' },
                ...(result?.map(message => ({
                    type: 'GroupChatMessage' as const,
                    id: message.id
                })) ?? [])
            ]
        }),
        getMessagesByPersonalChatId: builder.query<PersonalChatMessageModel[], { chatId: number, page: number, pageSize: number }>({
            query: ({ chatId, page, pageSize }) => ({
                url: `/PersonalChatMessage/getByChatId/${chatId}?page=${page}&pageSize=${pageSize}`,
            }),
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.chatId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.length = 0;
                    currentCache.push(...newItems);
                    return;
                }

                newItems.forEach(item => {
                    const index = currentCache.findIndex(x => x.id === item.id);
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
            providesTags: result => [
                { type: 'PersonalChatMessage', id: 'LIST' },
                ...(result?.map(message => ({
                    type: 'PersonalChatMessage' as const,
                    id: message.id
                })) ?? [])
            ]
        }),
    })
})

export const {
    useGetMessagesByGroupChatIdQuery,
    useLazyGetMessagesByGroupChatIdQuery,
    useGetMessagesByPersonalChatIdQuery,
    useLazyGetMessagesByPersonalChatIdQuery,
} = ChatApi;