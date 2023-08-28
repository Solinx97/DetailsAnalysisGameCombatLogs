import { ChatApi } from "./ChatApi";

export const GroupChatMessageCountApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatMessageCountAsync: builder.mutation({
            query: messagesCount => ({
                body: messagesCount,
                url: '/GroupChatMessageCount',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessageCount', result }],
        }),
        updateGroupChatMessageCountAsync: builder.mutation({
            query: messagesCount => ({
                body: messagesCount,
                url: '/GroupChatMessageCount',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessageCount', result }],
        }),
        removeGroupChatMessageCountAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatMessageCount/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessageCount', result }],
        }),
        findGroupChatMessageCount: builder.query({
            query: (arg) => {
                const { chatId, userId } = arg;
                return {
                    url: `/GroupChatMessageCount/find?chatId=${chatId}&userId=${userId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'GroupChatMessageCount', id }],
        }),
    })
})

export const {
    useCreateGroupChatMessageCountAsyncMutation,
    useUpdateGroupChatMessageCountAsyncMutation,
    useRemoveGroupChatMessageCountAsyncMutation,
    useFindGroupChatMessageCountQuery,
    useLazyFindGroupChatMessageCountQuery,
} = GroupChatMessageCountApi;