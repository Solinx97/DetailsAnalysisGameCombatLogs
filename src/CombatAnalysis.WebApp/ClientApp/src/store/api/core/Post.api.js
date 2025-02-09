import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const PostApi = createApi({
    reducerPath: 'postApi',
    tagTyes: [
        'UserPost',
        'NewUserPost',
        'UserPostLike',
        'UserPostDislike',
        'UserPostComment',
        'CommunityPost',
        'NewCommunityPost',
        'CommunityPostLike',
        'CommunityPostDislike',
        'CommunityPostComment',
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
                    ? [...result.map(({ id }) => ({ type: 'NewUserPost', id })), { type: 'NewUserPost' }]
                    : [{ type: 'NewUserPost' }]
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
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost', id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getMoreCommunityPostByListOfCommunityIds: builder.query({
            query: ({ communityIds, offset, pageSize }) => ({
                url: `/CommunityPost/getMoreByListOfCommunityIds?communityIds=${communityIds}&offset=${offset}&pageSize=${pageSize}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost', id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
        getNewCommunityPostByListOfCommunityIds: builder.query({
            query: ({ communityIds, checkFrom }) => ({
                url: `/CommunityPost/getNewByListOfCommunityIds?communityIds=${communityIds}&checkFrom=${checkFrom}`,
            }),
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'NewCommunityPost', id })), { type: 'NewCommunityPost' }]
                    : [{ type: 'NewCommunityPost' }]
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
} = PostApi;