import { ChatApi } from "../../ChatApi";

export const VoiceChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        getCall: builder.query({
            query: () => `VoiceChat`,
            providesTags: (result, error) => [{ type: 'VoiceChat'}],
        }),
    })
})

export const {
    useLazyGetCallQuery,
} = VoiceChatApi;