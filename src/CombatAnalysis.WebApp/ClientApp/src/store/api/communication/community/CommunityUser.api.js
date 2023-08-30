import { ChatApi } from "../../ChatApi";

export const CommunityUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        searchByUserIdAsync: builder.query({
            query: (id) => `/CommunityUser/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityUser', id }]
        }),
        createCommunityUserAsync: builder.mutation({
            query: communityUser => ({
                body: communityUser,
                url: '/CommunityUser',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityUser', arg }]
        }),
        removeCommunityUserAsync: builder.mutation({
            query: id => ({
                url: `/CommunityUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityUser', arg }]
        })
    })
})

export const {
    useLazySearchByUserIdAsyncQuery,
    useSearchByUserIdAsyncQuery,
    useCreateCommunityUserAsyncMutation,
    useRemoveCommunityUserAsyncMutation
} = CommunityUserApi;