import { ChatApi } from "../ChatApi";

export const PostCommentApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostCommentAsync: builder.mutation({
            query: postComment => ({
                body: postComment,
                url: '/UserPostComment',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPostComment', result }],
        }),
        updatePostCommentAsync: builder.mutation({
            query: postComment => ({
                body: postComment,
                url: '/UserPostComment',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPostComment', result }],
        }),
        removePostCommentAsync: builder.mutation({
            query: id => ({
                url: `/UserPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPostComment', result }],
        }),
        searchPostCommentByPostId: builder.query({
            query: (id) => `/UserPostComment/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostComment', id })), { type: 'UserPostComment' }]
                    : [{ type: 'UserPostComment' }]
        }),
    })
})

export const {
    useCreatePostCommentAsyncMutation,
    useUpdatePostCommentAsyncMutation,
    useRemovePostCommentAsyncMutation,
    useSearchPostCommentByPostIdQuery,
} = PostCommentApi;