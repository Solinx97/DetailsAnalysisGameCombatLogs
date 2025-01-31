import { ChatApi } from "../core/Chat.api";

export const GroupChatRulesApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatRulesAsync: builder.mutation({
            query: groupChatRules => ({
                body: groupChatRules,
                url: '/GroupChatRules',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatRules', result }],
        }),
        updateGroupChatRulesAsync: builder.mutation({
            query: groupChatRules => ({
                body: groupChatRules,
                url: '/GroupChatRules',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatRules', result }],
        }),
        removeGroupChatRulesAsync: builder.mutation({
            query: id => ({
                url: `/GroupChatRules/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'GroupChatRules', arg }]
        }),
        getGroupChatRulesById: builder.query({
            query: (id) => `/GroupChatRules/findByChatId/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatRules', id }],
        }),
    })
})

export const {
    useCreateGroupChatRulesAsyncMutation,
    useUpdateGroupChatRulesAsyncMutation,
    useRemoveGroupChatRulesAsyncMutation,
    useGetGroupChatRulesByIdQuery,
} = GroupChatRulesApi;