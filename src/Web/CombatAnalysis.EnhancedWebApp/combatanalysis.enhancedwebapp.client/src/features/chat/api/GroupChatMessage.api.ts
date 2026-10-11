import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { ChatMessagePatch } from '../types/patches/ChatMessagePatch';
import type { GroupChatMessageReadPatch } from '../types/patches/GroupChatMessageReadPatch';
import { ChatApi } from './Chat.api';

export const GroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatMessage: builder.mutation<void, GroupChatMessageModel>({
            query: message => ({
                body: message,
                url: '/GroupChatMessage',
                method: 'POST'
            }),
        }),
        partialUpdateGroupChatMessage: builder.mutation<void, { id: string, message: ChatMessagePatch }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/GroupChatMessage/${id}`,
                method: 'PATCH'
            }),
        }),
        readGroupChatMessage: builder.mutation<void, { id: string, message: GroupChatMessageReadPatch }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/GroupChatMessage/read/${id}`,
                method: 'PATCH'
            }),
        }),
        removeGroupChatMessage: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        removeGroupChatMessageByChatId: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        countGroupChatMessages: builder.query<number, number>({
            query: chatId => `/GroupChatMessage/count/${chatId}`,
        }),
    })
})

export const {
    useCreateGroupChatMessageMutation,
    usePartialUpdateGroupChatMessageMutation,
    useReadGroupChatMessageMutation,
    useRemoveGroupChatMessageMutation,
    useRemoveGroupChatMessageByChatIdMutation,
    useCountGroupChatMessagesQuery,
} = GroupChatMessageApi;