import { ChatApi } from "./ChatApi";

export const PersonalChatMessageApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPersonalChatMessageAsync: builder.mutation({
            query: message => ({
                body: message,
                url: '/PersonalChatMessage',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        updatePersonalChatMessageAsync: builder.mutation({
            query: message => ({
                body: message,
                url: '/PersonalChatMessage',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        removePersonalChatMessageAsync: builder.mutation({
            query: id => ({
                url: `/PersonalChatMessage/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        }),
        removePersonalChatMessageByChatIdAsync: builder.mutation({
            query: id => ({
                url: `/PersonalChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PersonalChatMessage', result }],
        })
    })
})

export const {
    useCreatePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation,
    useRemovePersonalChatMessageAsyncMutation,
    useRemovePersonalChatMessageByChatIdAsyncMutation,
} = PersonalChatMessageApi;