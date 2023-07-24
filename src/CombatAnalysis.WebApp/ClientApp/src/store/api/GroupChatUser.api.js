import { ChatApi } from "./ChatApi";

export const GroupChatUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatUserAsync: builder.mutation({
            query: groupChatUser => ({
                body: groupChatUser,
                url: 'GroupChatUser',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }],
        }),
        getGroupChatUserByChatId: builder.query({
            query: (id) => `/GroupChatUser/findByChatId/${id}`
        }),
        removeGroupChatUserAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }],
        }),
    })
})

export const {
    useCreateGroupChatUserAsyncMutation,
    useGetGroupChatUserByChatIdQuery,
    useRemoveGroupChatUserAsyncMutation,
} = GroupChatUserApi;