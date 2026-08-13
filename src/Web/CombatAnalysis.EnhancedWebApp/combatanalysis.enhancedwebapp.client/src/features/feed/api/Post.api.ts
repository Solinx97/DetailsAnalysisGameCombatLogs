import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { AllCimmunityPostsModel } from '../types/AllCimmunityPostsModel';
import type { AllUserPostsModel } from '../types/AllUserPostsModel';

const apiURL = '/api/v1';

export const PostApi = createApi({
    reducerPath: 'postApi',
    tagTypes: [
        'UserFeed',
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
        getUserPostsByUserId: builder.query<AllUserPostsModel, { appUserId: string, page: number, pageSize: number }>({
            query: ({ appUserId, page, pageSize }) => `/UserPost/getByUserId/${appUserId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.appUserId}`,
            merge: (currentCache, newItems) => {
                newItems.posts.forEach(item => {
                    const index = currentCache.posts.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.posts.push(item);
                    } else {
                        currentCache.posts[index] = item;
                    }
                });

                currentCache.posts.sort(
                    (a, b) =>
                        new Date(b.createdAt).getTime() -
                        new Date(a.createdAt).getTime()
                );
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.appUserId !== previousArg?.appUserId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'UserPost', id: 'LIST' },
                ...(result?.posts.map(post => ({
                    type: 'UserPost' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
        getCommunityPostsByCommunityId: builder.query<AllCimmunityPostsModel, { communityId: number, page: number, pageSize: number }>({
            query: ({ communityId, page, pageSize }) => `/CommunityPost/getByCommunityId/${communityId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.communityId}`,
            merge: (currentCache, newItems) => {
                newItems.posts.forEach(item => {
                    const index = currentCache.posts.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.posts.push(item);
                    } else {
                        currentCache.posts[index] = item;
                    }
                });

                currentCache.posts.sort(
                    (a, b) =>
                        new Date(b.createdAt).getTime() -
                        new Date(a.createdAt).getTime()
                );
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.communityId !== previousArg?.communityId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'CommunityPost', id: 'LIST' },
                ...(result?.posts.map(post => ({
                    type: 'CommunityPost' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
    })
})

export const {
    useGetUserPostsByUserIdQuery,
    useLazyGetUserPostsByUserIdQuery,
    useGetCommunityPostsByCommunityIdQuery,
    useLazyGetCommunityPostsByCommunityIdQuery,
} = PostApi;