import { ChatApi } from "../ChatApi";

export const PostApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostAsync: builder.mutation({
            query: post => ({
                body: post,
                url: '/Post',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'Post', result }],
        }),
        updatePostAsync: builder.mutation({
            query: post => ({
                body: post,
                url: '/Post',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'Post', result }],
        }),
        getPostById: builder.query({
            query: (id) => `/Post/${id}`,
            providesTags: (result, error, id) => [{ type: 'Post', id }],
        }),
        searchByUserId: builder.query({
            query: (id) => `/Post/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'Post', id }],
        }),
    })
})

export const {
    useCreatePostAsyncMutation,
    useUpdatePostAsyncMutation,
    useLazyGetPostByIdQuery,
    useGetPostByIdQuery,
    useSearchByUserIdQuery
} = PostApi;