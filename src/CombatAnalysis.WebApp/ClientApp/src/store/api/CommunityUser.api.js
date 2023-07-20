import { ChatApi } from "./ChatApi";

export const CommunityUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        searchByUserIdAsync: builder.query({
            query: (id) => `/CommunityUser/searchByUserId/${id}`,
        }),
        createCommunityUserAsync: builder.mutation({
            query: communityUser => ({
                body: communityUser,
                url: 'CommunityUser',
                method: 'POST'
            })
        }),
        removeCommunityUserAsync: builder.mutation({
            query: id => ({
                url: `/CommunityUser/${id}`,
                method: 'DELETE'
            })
        })
    })
})

export const {
    useLazySearchByUserIdAsyncQuery,
    useSearchByUserIdAsyncQuery,
    useCreateCommunityUserAsyncMutation,
    useRemoveCommunityUserAsyncMutation
} = CommunityUserApi;