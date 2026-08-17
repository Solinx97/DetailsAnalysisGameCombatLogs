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
            async onQueryStarted({ }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdCommunity } = await queryFulfilled;

                    dispatch(
                        CommunityApi.util.updateQueryData(
                            'getCommunityByUserId',
                            {
                                appUserId: createdCommunity.appUserId,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const exists = draft.communities.some(
                                    x => x.id === createdCommunity.id
                                );

                                if (!exists) {
                                    draft.communities.unshift(createdCommunity);
                                }
                            }
                        )
                    );
                } catch {
                    // DELETE failed
                }
            }
        }),
        updateCommunity: builder.mutation<void, { id: number, community: CommunityModel }>({
            query: ({ id, community }) => ({
                body: community,
                url: `/Community/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, community) => [{ type: 'Community', id: community.id }]
        }),
        updateCommunityRules: builder.mutation<void, { id: number, community: CommunityModel }>({
            query: ({ id, community }) => ({
                body: community,
                url: `/Community/updateRules/${id}`,
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
        getCommunities: builder.query<AllCommunityModel, { page: number, pageSize: number }>({
            query: ({ page, pageSize }) => `/Community?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName }) => `${endpointName}`,
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
    useUpdateCommunityMutation,
    useUpdateCommunityRulesMutation,
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