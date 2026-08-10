import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CommunityPostModel } from '../types/CommunityPostModel';
import type { UserPostModel } from '../types/UserPostModel';

const apiURL = '/api/v1';

export const PostApi = createApi({
    reducerPath: 'postApi',
    tagTypes: [
        'UserPost',
        'UserPostLike',
        'UserPostDislike',
        'UserPostComment',
        'CommunityPost',
        'CommunityPostLike',
        'CommunityPostDislike',
        'CommunityPostComment',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getUserPosts: builder.query<UserPostModel[], void>({
            query: () => '/UserPost',
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getUserPostsByUserId: builder.query<UserPostModel[], { appUserId: string, page: number, pageSize: number }>({
            query: ({ appUserId, page, pageSize }) => `/UserPost/getByUserId/${appUserId}?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPost => ({ type: 'UserPost' as const, id: userPost.id })),
                        { type: 'UserPost', id: 'LIST' },
                    ]
                    : [{ type: 'UserPost', id: 'LIST' }]
        }),
        getCommunityPostsByCommunityId: builder.query<CommunityPostModel[], { communityId: number, page: number, pageSize: number }>({
            query: ({ communityId, page, pageSize }) => `/CommunityPost/getByCommunityId?communityId=${communityId}&page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPost' as const, id })), { type: 'CommunityPost' }]
                    : [{ type: 'CommunityPost' }]
        }),
    })
})

export const {
    useGetUserPostsQuery,
    useLazyGetUserPostsQuery,
    useGetUserPostsByUserIdQuery,
    useLazyGetUserPostsByUserIdQuery,
    useGetCommunityPostsByCommunityIdQuery,
    useLazyGetCommunityPostsByCommunityIdQuery,
} = PostApi;