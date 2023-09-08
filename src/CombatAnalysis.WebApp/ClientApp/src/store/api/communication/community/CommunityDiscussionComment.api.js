import { ChatApi } from "../../ChatApi";

export const CommunityDiscussionCommentApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityDiscussionCommentAsync: builder.mutation({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussionComment',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussionComment', arg }]
        }),
        getCommunityDiscussionCommentById: builder.query({
            query: (id) => `/CommunityDiscussionComment/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussionComment', id }]
        }),
        getCommunityDiscussionCommentByDiscussionId: builder.query({
            query: (id) => `/CommunityDiscussionComment/findByDiscussionId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussionComment', id }]
        }),
        updateCommunityDiscussionCommentAsync: builder.mutation({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussionComment',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussionComment', arg }]
        }),
        removeCommunityDiscussionCommentAsync: builder.mutation({
            query: id => ({
                url: `/CommunityDiscussionComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussionComment', arg }]
        }),
    })
})

export const {
    useCreateCommunityDiscussionCommentAsyncMutation,
    useGetCommunityDiscussionCommentByIdQuery,
    useGetCommunityDiscussionCommentByDiscussionIdQuery,
    useUpdateCommunityDiscussionCommentAsyncMutation,
    useRemoveCommunityDiscussionCommentAsyncMutation
} = CommunityDiscussionCommentApi;