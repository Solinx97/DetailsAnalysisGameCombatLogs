import { ChatApi } from "../core/Chat.api";

export const GroupChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChat: builder.mutation({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChat', result }],
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
        }),
        getGroupChatById: builder.query({
            query: (id) => `/GroupChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChat', id }],
        }),
    })
})

export const {
    useCreateGroupChatMutation,
    useUpdateGroupChatAsyncMutation,
    useRemoveGroupChatAsyncMutation,
    useGetGroupChatByIdQuery,
} = GroupChatApi;