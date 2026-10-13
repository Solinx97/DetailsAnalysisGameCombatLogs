import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
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
        updateGroupChatMessageText: builder.mutation<void, { id: string, newMessage: string }>({
            query: ({ id, newMessage }) => ({
                url: `/GroupChatMessage/updateMessageText/${id}?newMessage=${newMessage}`,
                method: 'PATCH'
            }),
        }),
        updateGroupChatMessageMark: builder.mutation<void, { id: string, newMark: number }>({
            query: ({ id, newMark }) => ({
                url: `/GroupChatMessage/updateMessageMark/${id}?newMark=${newMark}`,
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
    useUpdateGroupChatMessageTextMutation,
    useUpdateGroupChatMessageMarkMutation,
    useReadGroupChatMessageMutation,
    useRemoveGroupChatMessageMutation,
    useRemoveGroupChatMessageByChatIdMutation,
    useCountGroupChatMessagesQuery,
} = GroupChatMessageApi;