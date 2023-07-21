import { ChatApi } from "./ChatApi";

export const PersonalChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatAsync: builder.mutation({
            query: chat => ({
                body: chat,
                url: '/PersonalChat',
                method: 'POST'
            })
        }),
        isExistAsync: builder.query({
            query: (userId, targetUserId) => ({
                url: `/PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            })
        }),
    })
})

export const {
    useCreatePersonalChatAsyncMutation,
    useLazyIsExistAsyncQuery,
} = PersonalChatApi;