import { ChatApi } from "../../ChatApi";

export const CommunityApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityAsync: builder.mutation({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        getCommunityById: builder.query({
            query: (id) => `/Community/${id}`,
            providesTags: (result, error, id) => [{ type: 'Community', id }]
        }),
        getCommunityPostByPostId: builder.query({
            query: (id) => `/CommunityPost/searchByPostId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityPost', id }]
        }),
        updateCommunityAsync: builder.mutation({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        removeCommunityAsync: builder.mutation({
            query: id => ({
                url: `/Community/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
    })
})

export const {
    useCreateCommunityAsyncMutation,
    useGetCommunityByIdQuery,
    useLazyGetCommunityByIdQuery,
    useLazyGetCommunityPostByPostIdQuery,
    useUpdateCommunityAsyncMutation,
    useRemoveCommunityAsyncMutation
} = CommunityApi;