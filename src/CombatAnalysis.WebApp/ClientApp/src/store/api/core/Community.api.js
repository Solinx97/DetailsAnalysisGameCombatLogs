import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const CommunityApi = createApi({
    reducerPath: 'communityApi',
    tagTyes: [
        'Community',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getCommunities: builder.query({
            query: () => '/Community',
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community', id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getCommunityById: builder.query({
            query: (id) => `/Community/${id}`,
            providesTags: (result, error, id) => [{ type: 'Community', id }]
        }),
        getCommunitiesCount: builder.query({
            query: () => '/Community/count'
        }),
        getCommunitiesWithPagination: builder.query({
            query: (pageSize) => `/Community/getWithPagination?pageSize=${pageSize}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community', id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        getMoreCommunitiesWithPagination: builder.query({
            query: ({ offset, pageSize }) => `/Community/getMoreWithPagination?offset=${offset}&pageSize=${pageSize}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'Community', id })), { type: 'Community' }]
                    : [{ type: 'Community' }]
        }),
        createCommunity: builder.mutation({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        updateCommunityAsync: builder.mutation({
            query: community => ({
                body: community,
                url: '/Community',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
        }),
        removeCommunityAsync: builder.mutation({
            query: id => ({
                url: `/Community/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Community', arg }]
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
    useGetCommunitiesQuery,
    useLazyGetCommunitiesQuery,
    useGetCommunityByIdQuery,
    useLazyGetCommunityByIdQuery,
    useGetCommunitiesCountQuery,
    useLazyGetCommunitiesCountQuery,
    useGetCommunitiesWithPaginationQuery,
    useLazyGetCommunitiesWithPaginationQuery,
    useGetMoreCommunitiesWithPaginationQuery,
    useLazyGetMoreCommunitiesWithPaginationQuery,
    useCreateCommunityMutation,
    useUpdateCommunityAsyncMutation,
    useRemoveCommunityAsyncMutation,
    useGetInviteToCommunityByIdQuery,
    useLazyGetInviteToCommunityByIdQuery,
} = CommunityApi;