import { ChatApi } from "./ChatApi";

export const GroupChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatAsync: builder.mutation({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }],
        }),
        updateGroupChatAsync: builder.mutation({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChat', result }],
        }),
        removeGroupChatAsync: builder.mutation({
            query: id => ({
                url: `/GroupChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'GroupChat', arg }]
        })
    })
})

export const {
    useCreateGroupChatAsyncMutation,
    useUpdateGroupChatAsyncMutation,
    useRemoveGroupChatAsyncMutation
} = GroupChatApi;