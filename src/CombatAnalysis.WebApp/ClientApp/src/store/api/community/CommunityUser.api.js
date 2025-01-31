import { CommunityApi } from "../core/Community.api";

export const CommunityUserApi = CommunityApi.injectEndpoints({
    tagTyes: [
        'CommunityUser',
    ],
    endpoints: builder => ({
        communityUserSearchByCommunityId: builder.query({
            query: (communityId) => `/CommunityUser/searchByCommunityId/${communityId}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityUser', id })), { type: 'CommunityUser' }]
                    : [{ type: 'CommunityUser' }]
        }),
        communityUserSearchByUserId: builder.query({
            query: (userId) => `/CommunityUser/searchByUserId/${userId}`,
            providesTags: (result, error, id) => [{ type: 'CommunityUser', id }]
        }),
        createCommunityUser: builder.mutation({
            query: communityUser => ({
                body: communityUser,
                url: '/CommunityUser',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityUser', arg }]
        }),
        removeCommunityUser: builder.mutation({
            query: id => ({
                url: `/CommunityUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'CommunityUser', arg }]
        })
    })
})

export const {
    useCommunityUserSearchByCommunityIdQuery,
    useLazyCommunityUserSearchByCommunityIdQuery,
    useCommunityUserSearchByUserIdQuery,
    useLazyCommunityUserSearchByUserIdQuery,
    useCreateCommunityUserMutation,
    useRemoveCommunityUserMutation
} = CommunityUserApi;