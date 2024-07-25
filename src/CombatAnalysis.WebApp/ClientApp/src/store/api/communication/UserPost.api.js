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
        userPostSearchByPostId: builder.query({
            query: (id) => `/UserPost/searchByPostId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UserPost', id }],
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
    useLazyUserPostSearchByPostIdQuery,
    useRemoveUserPostAsyncMutation,
} = UserPostApi;