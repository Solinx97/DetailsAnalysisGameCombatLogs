import type { CommunityDiscussionModel } from '../types/CommunityDiscussionModel';
import { CommunityApi } from './Community.api';

export const CommunityDiscussionApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityDiscussion: builder.mutation<CommunityDiscussionModel, CommunityDiscussionModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussion',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityDiscussion', id: result.id }] : [],
        }),
        updateCommunityDiscussion: builder.mutation<void, { id: number, discussion: CommunityDiscussionModel }>({
            query: ({ id, discussion }) => ({
                body: discussion,
                url: `/CommunityDiscussion/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, discussion) => [{ type: 'CommunityDiscussion', id: discussion.id }]
        }),
        removeCommunityDiscussion: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityDiscussion/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityDiscussion', id }]
        }),
        getCommunityDiscussionById: builder.query<CommunityDiscussionModel, number>({
            query: id => `/CommunityDiscussion/${id}`,
            providesTags: result => result ? [{ type: 'CommunityDiscussion', id: result.id }] : [],
        }),
        getCommunityDiscussionByCommunityId: builder.query<CommunityDiscussionModel[], { communityId: number, page: number, pageSize: number }>({
            query: ({ communityId, page, pageSize }) => `/CommunityDiscussion/getByCommunityId/${communityId}?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityDiscussion => ({ type: 'CommunityDiscussion' as const, id: communityDiscussion.id })),
                        { type: 'CommunityDiscussion', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityDiscussion', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateCommunityDiscussionMutation,
    useUpdateCommunityDiscussionMutation,
    useRemoveCommunityDiscussionMutation,
    useGetCommunityDiscussionByIdQuery,
    useGetCommunityDiscussionByCommunityIdQuery,
    useLazyGetCommunityDiscussionByCommunityIdQuery,
} = CommunityDiscussionApi;