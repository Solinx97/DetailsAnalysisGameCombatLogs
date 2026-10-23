import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CreateVoiceChatModel } from '../types/CreateVoiceChatModel';
import type { VoiceChatTokenResponse } from '../types/VoiceChatTokenResponse';

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
        getVoiceChatToken: builder.mutation<VoiceChatTokenResponse, { chatId: number, participantName: string }>({
            query: ({ chatId, participantName }) => ({
                body: { Username: participantName },
                url: `/VoiceChat/token/${chatId}`,
                method: 'POST'
            }),
        }),
        isVoiceChatExist: builder.query<boolean, number>({
            query: chatId => `/VoiceChat/isChatExist/${chatId}`,
        }),
    })
})

export const {
    useCreateCallMutation,
    useRemoveCallMutation,
    useGetVoiceChatTokenMutation,
    useIsVoiceChatExistQuery,
} = VoiceChatApi;