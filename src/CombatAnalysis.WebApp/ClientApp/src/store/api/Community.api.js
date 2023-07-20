import { ChatApi } from "./ChatApi";

export const CommunityApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityAsync: builder.mutation({
            query: community => ({
                body: community,
                url: 'Community',
                method: 'POST'
            })
        }),
        getCommunityById: builder.query({
            query: (id) => `/Community/${id}`,
        }),
        removeCommunityAsync: builder.mutation({
            query: id => ({
                url: `/Community/${id}`,
                method: 'DELETE'
            })
        })
    })
})

export const {
    useCreateCommunityAsyncMutation,
    useGetCommunityByIdQuery,
    useLazyGetCommunityByIdQuery,
    useRemoveCommunityAsyncMutation
} = CommunityApi;