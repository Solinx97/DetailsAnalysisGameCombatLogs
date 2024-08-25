import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CommunityApi = createApi({
    reducerPath: 'communityApi',
    tagTyes: [
        'UserPost',
        'UserPostLike',
        'UserPostDislike',
        'UserPostComment',
        'Community',
        'CommunityPost',
        'CommunityUser',
        'InviteToCommunity'
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getUserPosts: builder.query({
            query: () => '/UserPost',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        userPostSearchByOwnerId: builder.query({
            query: (id) => `/UserPost/searchByOwnerId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getCommunities: builder.query({
            query: () => '/Community',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community', id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        communityPostSearchByCommunityIdAsync: builder.query({
            query: (id) => `/CommunityPost/searchByCommunityId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost', id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        communityUserSearchByCommunityIdAsync: builder.query({
            query: (id) => `/CommunityUser/searchByCommunityId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityUser', id })), { type: 'CommunityUser' }]
                    : [{ type: 'CommunityUser' }]
        }),
        getInviteToCommunityById: builder.query({
            query: (id) => `/InviteToCommunity/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [{ type: 'InviteToCommunity', id: result.id }]
                    : [{ type: 'InviteToCommunity' }]
        }),
    })
})

export const {
    useGetUserPostsQuery,
    useLazyGetUserPostsQuery,
    useGetCommunitiesQuery,
    useLazyGetCommunitiesQuery,
    useCommunityPostSearchByCommunityIdAsyncQuery,
    useLazyCommunityPostSearchByCommunityIdAsyncQuery,
    useCommunityUserSearchByCommunityIdAsyncQuery,
    useLazyCommunityUserSearchByCommunityIdAsyncQuery,
    useUserPostSearchByOwnerIdQuery,
    useLazyUserPostSearchByOwnerIdQuery,
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;