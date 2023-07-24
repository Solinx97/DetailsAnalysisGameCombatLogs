import { ChatApi } from "./ChatApi";

export const GroupChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatAsync: builder.mutation({
            query: groupChat => ({
                body: groupChat,
                url: 'GroupChat',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChatUser', result }],
        }),
        updateGroupChatAsync: builder.mutation({
            query: groupChat => ({
                body: groupChat,
                url: 'GroupChat',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'GroupChat', result }],
        })
    })
})

export const {
    useCreateGroupChatAsyncMutation,
    useUpdateGroupChatAsyncMutation,
} = GroupChatApi;