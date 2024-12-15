import { ChatApi } from "../core/Chat.api";

export const VoiceChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCall: builder.mutation({
            query: groupChat => ({
                body: groupChat,
                url: '/VoiceChat',
                method: 'POST'
            }),
        }),
        removeCall: builder.mutation({
            query: id => ({
                url: `/VoiceChat/${id}`,
                method: 'DELETE'
            }),
        }),
        getCall: builder.query({
            query: () => `/VoiceChat`,
        }),
        getCallById: builder.query({
            query: (id) => `/VoiceChat/${id}`,
        }),
    })
})

export const {
    useCreateCallMutation,
    useRemoveCallMutation,
    useGetCallQuery,
    useGetCallByIdQuery,
} = VoiceChatApi;