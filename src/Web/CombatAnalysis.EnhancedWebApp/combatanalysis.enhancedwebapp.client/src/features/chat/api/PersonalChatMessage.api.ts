import type { ChatMessagePatch } from '../types/patches/ChatMessagePatch';
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
        partialUpdatePersonalChatMessage: builder.mutation<void, { id: string, message: ChatMessagePatch }>({
            query: ({ id, message }) => ({
                body: message,
                url: `/PersonalChatMessage/${id}`,
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
    usePartialUpdatePersonalChatMessageMutation,
    useRemovePersonalChatMessageMutation,
    useRemovePersonalChatMessageByChatIdMutation,
    useCountPersonalChatMessagesQuery,
} = PersonalChatMessageApi;