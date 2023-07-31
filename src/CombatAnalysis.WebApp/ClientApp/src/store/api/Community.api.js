import { ChatApi } from "./ChatApi";

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
        removeCommunityAsync: builder.mutation({
            query: id => ({
                url: `/Community/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        })
    })
})

export const {
    useCreateCommunityAsyncMutation,
    useGetCommunityByIdQuery,
    useLazyGetCommunityByIdQuery,
    useRemoveCommunityAsyncMutation
} = CommunityApi;