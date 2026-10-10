import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { VoiceChatModel } from '../types/VoiceChatModel';
import type { CreateVoiceChatModel } from '../types/CreateVoiceChatModel';

const apiURL = '/api/v1';

export const VoiceChatApi = createApi({
    reducerPath: 'voidChatApi',
    tagTypes: [
        'VoiceChat',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        createCall: builder.mutation<void, CreateVoiceChatModel>({
            query: groupChat => ({
                body: groupChat,
                url: '/VoiceChat',
                method: 'POST'
            }),
        }),
        removeCall: builder.mutation<void, number>({
            query: id => ({
                url: `/VoiceChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'VoiceChat', id }]
        }),
        getCallByChatId: builder.query<VoiceChatModel, number>({
            query: chatId => `/VoiceChat/getByChatId/${chatId}`,
            providesTags: result => result ? [{ type: 'VoiceChat', id: result.id }] : []
        }),
        isVoiceChatExist: builder.query<boolean, number>({
            query: chatId => `/VoiceChat/isChatExist/${chatId}`,
        }),
    })
})

export const {
    useCreateCallMutation,
    useRemoveCallMutation,
    useGetCallByChatIdQuery,
    useIsVoiceChatExistQuery,
} = VoiceChatApi;