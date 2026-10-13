import type { PersonalChatMessageReadPatch } from '../types/patches/PersonalChatMessageReadPatch';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import { ChatApi } from './Chat.api';

export const PersonalChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatMessage: builder.mutation<PersonalChatMessageModel, PersonalChatMessageModel>({
            query: personalMessage => ({
                body: personalMessage,
                url: '/PersonalChatMessage',
                method: 'POST'
            }),
        }),
        updatePersonalChatMessageText: builder.mutation<void, { id: string, newMessage: string }>({
            query: ({ id, newMessage }) => ({
                url: `/PersonalChatMessage/updateMessageText/${id}?newMessage=${newMessage}`,
                method: 'PATCH'
            }),
        }),
        updatePersonalChatMessageMark: builder.mutation<void, { id: string, newMark: number }>({
            query: ({ id, newMark }) => ({
                url: `/PersonalChatMessage/updateMessageMark/${id}?newMark=${newMark}`,
                method: 'PATCH'
            }),
        }),
        readPersonalChatMessage: builder.mutation<void, { id: string, message: PersonalChatMessageReadPatch }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/PersonalChatMessage/read/${id}`,
                method: 'PATCH'
            }),
        }),
        removePersonalChatMessage: builder.mutation<void, string>({
            query: id => ({
                url: `/PersonalChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'PersonalChatMessage', id }],
        }),
        removePersonalChatMessageByChatId: builder.mutation<void, number>({
            query: chatId => ({
                url: `/PersonalChatMessage/deleteByChatId/${chatId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'PersonalChatMessage', id }],
        }),
        countPersonalChatMessages: builder.query<number, number>({
            query: chatId => `/PersonalChatMessage/count/${chatId}`,
        }),
    })
})

export const {
    useCreatePersonalChatMessageMutation,
    useUpdatePersonalChatMessageTextMutation,
    useUpdatePersonalChatMessageMarkMutation,
    useReadPersonalChatMessageMutation,
    useRemovePersonalChatMessageMutation,
    useRemovePersonalChatMessageByChatIdMutation,
    useCountPersonalChatMessagesQuery,
} = PersonalChatMessageApi;