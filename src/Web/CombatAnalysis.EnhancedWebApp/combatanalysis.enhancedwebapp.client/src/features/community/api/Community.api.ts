import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { CommunityModel } from '../types/CommunityModel';
import type { InviteToCommunityModel } from '../types/InviteToCommunityModel';

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
        getMoreCommunitiesWithPagination: builder.query<CommunityModel[], { offset: number, pageSize: number }>({
            query: ({ offset, pageSize }) => `/Community/getMoreWithPagination?offset=${offset}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(community => ({ type: 'Community' as const, id: community.id })),
                        { type: 'Community', id: 'LIST' },
                    ]
                    : [{ type: 'Community', id: 'LIST' }]
        }),
        getCommunityByUserId: builder.query<CommunityModel[], string>({
            query: appUserId => `/Community/getByUserId/${appUserId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(community => ({ type: 'Community' as const, id: community.id })),
                        { type: 'Community', id: 'LIST' },
                    ]
                    : [{ type: 'Community', id: 'LIST' }]
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
    useGetMoreCommunitiesWithPaginationQuery,
    useLazyGetMoreCommunitiesWithPaginationQuery,
    useGetCommunityByUserIdQuery,
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;