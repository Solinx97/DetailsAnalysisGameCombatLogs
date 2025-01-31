import { ChatApi } from "../core/Chat.api";

export const UnreadGroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createUnreadGroupChatMessageAsync: builder.mutation({
            query: unreadMessage => ({
                body: unreadMessage,
                url: '/UnreadGroupChatMessage',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'UnreadGroupChatMessage', result }],
        }),
        updateUnreadGroupChatMessageAsync: builder.mutation({
            query: unreadMessage => ({
                body: unreadMessage,
                url: '/UnreadGroupChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'UnreadGroupChatMessage', result }],
        }),
        removeUnreadGroupChatMessageAsync: builder.mutation({
            query: id => ({
                url: `/UnreadGroupChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'UnreadGroupChatMessage', result }],
        }),
        findUnreadGroupChatMessage: builder.query({
            query: (arg) => {
                const { messageId, groupChatUserId } = arg;
                return {
                    url: `/UnreadGroupChatMessage/find?messageId=${messageId}&groupChatUserId=${groupChatUserId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'UnreadGroupChatMessage', id }],
        }),
        findUnreadGroupChatMessageByMessageId: builder.query({
            query: (id) => `/UnreadGroupChatMessage/findByMessageId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UnreadGroupChatMessage', id }],
        }),
    })
})

export const {
    useCreateUnreadGroupChatMessageAsyncMutation,
    useUpdateUnreadGroupChatMessageAsyncMutation,
    useRemoveUnreadGroupChatMessageAsyncMutation,
    useFindUnreadGroupChatMessageQuery,
    useLazyFindUnreadGroupChatMessageQuery,
    useFindUnreadGroupChatMessageByMessageIdQuery,
    useLazyFindUnreadGroupChatMessageByMessageIdQuery,
} = UnreadGroupChatMessageApi;