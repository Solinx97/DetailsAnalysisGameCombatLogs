import { PostApi } from "../core/Post.api";

export const UserPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostComment: builder.mutation({
            query: userPostComment => ({
                body: userPostComment,
                url: '/UserPostComment',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPostComment', result }],
        }),
        updateUserPostComment: builder.mutation({
            query: userPostComment => ({
                body: userPostComment,
                url: '/UserPostComment',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPostComment', result }],
        }),
        removeUserPostComment: builder.mutation({
            query: id => ({
                url: `/UserPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPostComment', result }],
        }),
        searchUserPostCommentByPostId: builder.query({
            query: (id) => `/UserPostComment/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostComment', id })), { type: 'UserPostComment' }]
                    : [{ type: 'UserPostComment' }]
        }),
    })
});

export const {
    useCreateUserPostCommentMutation,
    useUpdateUserPostCommentMutation,
    useRemoveUserPostCommentMutation,
    useSearchUserPostCommentByPostIdQuery,
} = UserPostCommentApi;