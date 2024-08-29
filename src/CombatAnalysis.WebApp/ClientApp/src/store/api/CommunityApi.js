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
        'CommunityPostLike',
        'CommunityPostDislike',
        'CommunityPostComment',
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
        getUserPostsByUserId: builder.query({
            query: ({ appUserId, pageSize }) => ({
                url: `/UserPost/getByUserId?appUserId=${appUserId}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getMoreUserPostsByUserId: builder.query({
            query: ({ appUserId, offset, pageSize }) => ({
                url: `/UserPost/getMoreByUserId?appUserId=${appUserId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getNewUserPostsByUserId: builder.query({
            query: ({ appUserId, checkFrom }) => ({
                url: `/UserPost/getNewPosts?appUserId=${appUserId}&checkFrom=${checkFrom}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getUserPostByListOfUserIds: builder.query({
            query: ({ appUserIds, pageSize }) => ({
                url: `/UserPost/getByListOfUserIds?appUserIds=${appUserIds}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getMoreUserPostByListOfUserIds: builder.query({
            query: ({ appUserIds, offset, pageSize }) => ({
                url: `/UserPost/getMoreByListOfUserIds?appUserIds=${appUserIds}&offset=${offset}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getNewUserPostByListOfUserIds: builder.query({
            query: ({ appUserIds, checkFrom }) => ({
                url: `/UserPost/getNewByListOfUserIds?appUserIds=${appUserIds}&checkFrom=${checkFrom}`,
            }),
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
        getCommunityPostsByCommunityId: builder.query({
            query: ({ communityId, pageSize }) => ({
                url: `/CommunityPost/getByCommunityId?communityId=${communityId}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost', id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getMoreCommunityPostsByCommunityId: builder.query({
            query: ({ communityId, offset, pageSize }) => ({
                url: `/CommunityPost/getMoreByCommunityId?communityId=${communityId}&offset=${offset}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost', id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getNewCommunityPostsByCommunityId: builder.query({
            query: ({ communityId, checkFrom }) => ({
                url: `/CommunityPost/getNewPosts?communityId=${communityId}&checkFrom=${checkFrom}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost', id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getCommunityPostByListOfCommunityIds: builder.query({
            query: ({ communityIds, pageSize }) => ({
                url: `/CommunityPost/getByListOfCommunityIds?communityIds=${communityIds}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getMoreCommunityPostByListOfCommunityIds: builder.query({
            query: ({ communityIds, offset, pageSize }) => ({
                url: `/CommunityPost/getMoreByListOfCommunityIds?communityIds=${communityIds}&offset=${offset}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
        }),
        getNewCommunityPostByListOfCommunityIds: builder.query({
            query: ({ communityIds, checkFrom }) => ({
                url: `/CommunityPost/getNewByListOfCommunityIds?communityIds=${communityIds}&checkFrom=${checkFrom}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPost', id })), { type: 'UserPost' }]
                    : [{ type: 'UserPost' }]
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
    useGetUserPostsByUserIdQuery,
    useLazyGetUserPostsByUserIdQuery,
    useGetMoreUserPostsByUserIdQuery,
    useLazyGetMoreUserPostsByUserIdQuery,
    useGetNewUserPostsByUserIdQuery,
    useLazyGetNewUserPostsByUserIdQuery,
    useGetUserPostByListOfUserIdsQuery,
    useLazyGetUserPostByListOfUserIdsQuery,
    useGetMoreUserPostByListOfUserIdsQuery,
    useLazyGetMoreUserPostByListOfUserIdsQuery,
    useGetNewUserPostByListOfUserIdsQuery,
    useLazyGetNewUserPostByListOfUserIdsQuery,
    useGetCommunitiesQuery,
    useLazyGetCommunitiesQuery,
    useGetCommunityPostsByCommunityIdQuery,
    useLazyGetCommunityPostsByCommunityIdQuery,
    useGetMoreCommunityPostsByCommunityIdQuery,
    useLazyGetMoreCommunityPostsByCommunityIdQuery,
    useGetNewCommunityPostsByCommunityIdQuery,
    useLazyGetNewCommunityPostsByCommunityIdQuery,
    useGetCommunityPostByListOfCommunityIdsQuery,
    useLazyGetCommunityPostByListOfCommunityIdsQuery,
    useGetMoreCommunityPostByListOfCommunityIdsQuery,
    useLazyGetMoreCommunityPostByListOfCommunityIdsQuery,
    useGetNewCommunityPostByListOfCommunityIdsQuery,
    useLazyGetNewCommunityPostByListOfCommunityIdsQuery,
    useCommunityUserSearchByCommunityIdAsyncQuery,
    useLazyCommunityUserSearchByCommunityIdAsyncQuery,
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;