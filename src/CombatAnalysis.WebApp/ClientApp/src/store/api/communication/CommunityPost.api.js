import { CommunityApi } from "../CommunityApi";

export const CommunityPostApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        getCommunityPostCountByCommunityId: builder.query({
            query: (communityId) => `/CommunityPost/count/${communityId}`
        }),
        getCommunityPostById: builder.query({
            query: (id) => `/CommunityPost/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [{ type: 'CommunityPost', id: result.id }]
                    : [{ type: 'CommunityPost' }]
        }),
        createCommunityPost: builder.mutation({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPost', result }],
        }),
        updateCommunityPost: builder.mutation({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPost', result }],
        }),
        removeCommunityPost: builder.mutation({
            query: id => ({
                url: `/CommunityPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityPost', arg }]
        }),
    })
})

export const {
    useGetCommunityPostCountByCommunityIdQuery,
    useLazyGetCommunityPostCountByCommunityIdQuery,
    useGetCommunityPostByIdQuery,
    useLazyGetCommunityPostByIdQuery,
    useCreateCommunityPostMutation,
    useUpdateCommunityPostMutation,
    useRemoveCommunityPostMutation
} = CommunityPostApi;