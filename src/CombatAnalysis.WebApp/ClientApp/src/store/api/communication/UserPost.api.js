import { ChatApi } from "../ChatApi";

export const UserPostApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostAsync: builder.mutation({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPost', result }],
        }),
        removeUserPostAsync: builder.mutation({
            query: id => ({
                url: `/UserPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPost', result }],
        }),
    })
})

export const {
    useCreateUserPostAsyncMutation,
    useRemoveUserPostAsyncMutation,
} = UserPostApi;