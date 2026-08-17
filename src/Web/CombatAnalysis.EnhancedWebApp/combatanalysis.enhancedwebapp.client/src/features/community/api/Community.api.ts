import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CommunityModel } from '../types/CommunityModel';
import type { InviteToCommunityModel } from '../types/InviteToCommunityModel';
import type { AllCommunityModel } from '../types/AllCommunityModel';

const apiURL = '/api/v1';

export const CommunityApi = createApi({
    reducerPath: 'communityApi',
    tagTypes: [
        'Community',
        'CommunityUser',
        'InviteToCommunity',
        'CommunityDiscussion',
        'CommunityDiscussionComment',
        'InviteToCommunity',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        createCommunity: builder.mutation<CommunityModel, CommunityModel>({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'Community', id: result.id }] : [],
        }),
        updateCommunityAsync: builder.mutation<void, { id: number, community: CommunityModel }>({
            query: ({id, community }) => ({
                body: community,
                url: `/Community/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, community) => [{ type: 'Community', id: community.id }]
        }),
        removeCommunityAsync: builder.mutation<void, number>({
            query: id => ({
                url: `/Community/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'Community', id }]
        }),
        getCommunities: builder.query<CommunityModel[], { page: number, pageSize: number }>({
            query: ({ page, pageSize }) => `/Community?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(community => ({ type: 'Community' as const, id: community.id })),
                        { type: 'Community', id: 'LIST' },
                    ]
                    : [{ type: 'Community', id: 'LIST' }]
        }),
        getCommunityById: builder.query<CommunityModel, number>({
            query: id => `/Community/${id}`,
            providesTags: result => result ? [{ type: 'Community', id: result.id }] : [],
        }),
        getCommunitiesCount: builder.query<number, void>({
            query: () => '/Community/count',
        }),
        getCommunityByUserId: builder.query<AllCommunityModel, { appUserId: string, page: number, pageSize: number }>({
            query: ({ appUserId, page, pageSize }) => `/Community/getByUserId/${appUserId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.appUserId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.communities.length = 0;
                    currentCache.communities.push(...newItems.communities);
                    return;
                }

                newItems.communities.forEach(item => {
                    const index = currentCache.communities.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.communities.push(item);
                    } else {
                        currentCache.communities[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.appUserId !== previousArg?.appUserId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'CommunityDiscussion', id: 'LIST' },
                ...(result?.communities.map(post => ({
                    type: 'CommunityDiscussion' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
        getInviteToCommunityById: builder.query<InviteToCommunityModel, number>({
            query: id => `/InviteToCommunity/${id}`,
            providesTags: result => result ? [{ type: 'InviteToCommunity', id: result.id }] : [],
        }),
    })
})

export const {
    useCreateCommunityMutation,
    useUpdateCommunityAsyncMutation,
    useRemoveCommunityAsyncMutation,
    useGetCommunitiesQuery,
    useLazyGetCommunitiesQuery,
    useGetCommunityByIdQuery,
    useLazyGetCommunityByIdQuery,
    useGetCommunitiesCountQuery,
    useLazyGetCommunitiesCountQuery,
    useGetCommunityByUserIdQuery,
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;