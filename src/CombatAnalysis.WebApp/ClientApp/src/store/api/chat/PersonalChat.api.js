import { ChatApi } from "../core/Chat.api";

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
            query: (arg) => {
                const { userId, targetUserId } = arg;
                return {
                    url: `/PersonalChat/isExist?initiatorId=${userId}&companionId=${targetUserId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'PersonalChat', id }],
        }),
        getByUserIdAsync: builder.query({
            query: (id) => `/PersonalChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'PersonalChat', id }],
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
    useGetByUserIdAsyncQuery,
    useLazyGetByUserIdAsyncQuery,
    useRemovePersonalChatAsyncMutation,
} = PersonalChatApi;