import { ChatApi } from "./ChatApi";

export const PersonalChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatAsync: builder.mutation({
            query: chat => ({
                body: chat,
                url: '/PersonalChat',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChat', result }],
        }),
        updatePersonalChatAsync: builder.mutation({
            query: chat => ({
                body: chat,
                url: '/PersonalChat',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChat', result }],
        }),
        isExistAsync: builder.query({
            query: (userId, targetUserId) => ({
                url: `/PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
            })
        }),
        removePersonalChatAsync: builder.mutation({
            query: id => ({
                url: `/PersonalChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChat', result }],
        }),
    })
})

export const {
    useCreatePersonalChatAsyncMutation,
    useUpdatePersonalChatAsyncMutation,
    useLazyIsExistAsyncQuery,
    useRemovePersonalChatAsyncMutation,
} = PersonalChatApi;