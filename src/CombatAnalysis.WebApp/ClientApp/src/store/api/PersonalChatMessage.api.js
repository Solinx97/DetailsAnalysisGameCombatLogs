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
            })
        }),
        removePersonalChatMessageAsync: builder.mutation({
            query: id => ({
                url: `/PersonalChatMessage/${id}`,
                method: 'DELETE'
            })
        }),
        removePersonalChatMessageByChatIdAsync: builder.mutation({
            query: id => ({
                url: `/PersonalChatMessage/deleteByChatId/${id}`,
                method: 'DELETE'
            })
        })
    })
})

export const {
    useCreatePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation,
    useRemovePersonalChatMessageAsyncMutation,
    useRemovePersonalChatMessageByChatIdAsyncMutation,
} = PersonalChatMessageApi;