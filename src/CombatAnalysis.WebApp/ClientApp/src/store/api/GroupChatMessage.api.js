import { ChatApi } from "./ChatApi";

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
            })
        }),
        removeGroupChatMessageAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatMessage/${id}`,
                method: 'DELETE'
            })
        }),
        removeGroupChatMessageByChatIdAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            })
        })
    })
})

export const {
    useCreateGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation,
    useRemoveGroupChatMessageAsyncMutation,
    useRemoveGroupChatMessageByChatIdAsyncMutation,
} = GroupChatMessageApi;