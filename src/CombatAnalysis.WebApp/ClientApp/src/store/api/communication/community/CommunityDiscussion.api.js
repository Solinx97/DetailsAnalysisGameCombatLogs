import { ChatApi } from "../../ChatApi";

export const CommunityDiscussionApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityDiscussionAsync: builder.mutation({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussion',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussion', arg }]
        }),
        getCommunityDiscussionById: builder.query({
            query: (id) => `/CommunityDiscussion/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussion', id }]
        }),
        getCommunityDiscussionByCommunityId: builder.query({
            query: (id) => `/CommunityDiscussion/findByCommunityId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityDiscussion', id }]
        }),
        updateCommunityDiscussionAsync: builder.mutation({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussion',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussion', arg }]
        }),
        removeCommunityDiscussionAsync: builder.mutation({
            query: id => ({
                url: `/CommunityDiscussion/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityDiscussion', arg }]
        }),
    })
})

export const {
    useCreateCommunityDiscussionAsyncMutation,
    useGetCommunityDiscussionByIdQuery,
    useGetCommunityDiscussionByCommunityIdQuery,
    useUpdateCommunityDiscussionAsyncMutation,
    useRemoveCommunityDiscussionAsyncMutation
} = CommunityDiscussionApi;