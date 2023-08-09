import { ChatApi } from "./ChatApi";

export const PostCommentApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostCommentAsync: builder.mutation({
            query: postComment => ({
                body: postComment,
                url: '/PostComment',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'PostComment', result }],
        }),
        updatePostCommentAsync: builder.mutation({
            query: postComment => ({
                body: postComment,
                url: '/PostComment',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'PostComment', result }],
        }),
        removePostCommentAsync: builder.mutation({
            query: id => ({
                url: `/PostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'PostComment', result }],
        }),
        searchPostCommentByPostId: builder.query({
            query: (id) => `/PostComment/searchByPostId/${id}`,
            providesTags: (result, error, id) => [{ type: 'PostComment', id }],
        }),
    })
})

export const {
    useCreatePostCommentAsyncMutation,
    useUpdatePostCommentAsyncMutation,
    useRemovePostCommentAsyncMutation,
    useSearchPostCommentByPostIdQuery,
} = PostCommentApi;