import { ChatApi } from "../core/Chat.api";

export const PersonalChatMessageCountApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatMessageCountAsync: builder.mutation({
            query: messagesCount => ({
                body: messagesCount,
                url: '/PersonalChatMessageCount',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessageCount', result }],
        }),
        updatePersonalChatMessageCountAsync: builder.mutation({
            query: messagesCount => ({
                body: messagesCount,
                url: '/PersonalChatMessageCount',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessageCount', result }],
        }),
        removePersonalChatMessageCountAsync: builder.mutation({
            query: id => ({
                url: `/PersonalChatMessageCount/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessageCount', result }],
        }),
        findPersonalChatMessageCount: builder.query({
            query: (arg) => {
                const { chatId, userId } = arg;
                return {
                    url: `/PersonalChatMessageCount/find?chatId=${chatId}&userId=${userId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'PersonalChatMessageCount', id }],
        }),
    })
})

export const {
    useCreatePersonalChatMessageCountAsyncMutation,
    useUpdatePersonalChatMessageCountAsyncMutation,
    useRemovePersonalChatMessageCountAsyncMutation,
    useFindPersonalChatMessageCountQuery,
    useLazyFindPersonalChatMessageCountQuery,
} = PersonalChatMessageCountApi;