import { ChatApi } from "../../ChatApi";

export const GroupChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatMessageAsync: builder.mutation({
            query: message => ({
                body: message,
                url: '/GroupChatMessage',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessage', result }],
        }),
        updateGroupChatMessageAsync: builder.mutation({
            query: message => ({
                body: message,
                url: '/GroupChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessage', result }],
        }),
        removeGroupChatMessageAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessage', result }],
        }),
        removeGroupChatMessageByChatIdAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatMessage', result }],
        })
    })
})

export const {
    useCreateGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation,
    useRemoveGroupChatMessageAsyncMutation,
    useRemoveGroupChatMessageByChatIdAsyncMutation,
} = GroupChatMessageApi;