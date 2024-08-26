import { CommunityApi } from "../CommunityApi";

export const CommunityPostCommentApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostComment: builder.mutation({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPostComment', result }],
        }),
        updateCommunityPostComment: builder.mutation({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPostComment', result }],
        }),
        removeCommunityPostComment: builder.mutation({
            query: id => ({
                url: `/CommunityPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPostComment', result }],
        }),
        searchCommunityPostCommentByPostId: builder.query({
            query: (id) => `/CommunityPostComment/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPostComment', id })), { type: 'CommunityPostComment' }]
                    : [{ type: 'CommunityPostComment' }]
        }),
    })
});

export const {
    useCreateCommunityPostCommentMutation,
    useUpdateCommunityPostCommentMutation,
    useRemoveCommunityPostCommentMutation,
    useSearchCommunityPostCommentByPostIdQuery,
} = CommunityPostCommentApi;