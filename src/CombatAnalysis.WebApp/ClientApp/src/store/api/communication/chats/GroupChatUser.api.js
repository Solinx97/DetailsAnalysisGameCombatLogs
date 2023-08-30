import { ChatApi } from "../../ChatApi";

export const GroupChatUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatUserAsync: builder.mutation({
            query: groupChatUser => ({
                body: groupChatUser,
                url: '/GroupChatUser',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }]
        }),
        removeGroupChatUserAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }],
        }),
        findGroupChatUser: builder.query({
            query: (arg) => {
                const { chatId, userId } = arg;
                return {
                    url: `/GroupChatUser/find?chatId=${chatId}&userId=${userId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        findGroupChatUserByUserId: builder.query({
            query: (id) => `/GroupChatUser/findByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        findGroupChatUserByChatId: builder.query({
            query: (id) => `/GroupChatUser/findByChatId/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
    })
})

export const {
    useCreateGroupChatUserAsyncMutation,
    useRemoveGroupChatUserAsyncMutation,
    useFindGroupChatUserQuery,
    useFindGroupChatUserByUserIdQuery,
    useFindGroupChatUserByChatIdQuery,
} = GroupChatUserApi;