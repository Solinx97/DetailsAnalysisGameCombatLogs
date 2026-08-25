import type { AllDiscussionModel } from '../types/AllDiscussionModel';
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
            invalidatesTags: () => [{ type: 'CommunityDiscussion', id: 'LIST' }]
        }),
        updateCommunityDiscussion: builder.mutation<void, { id: number, discussion: CommunityDiscussionModel }>({
            query: ({ id, discussion }) => ({
                body: discussion,
                url: `/CommunityDiscussion/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, discussion) => [{ type: 'CommunityDiscussion', id: discussion.id }]
        }),
        removeCommunityDiscussion: builder.mutation<void, { id: number, communityId: number }>({
            query: ({ id }) => ({
                url: `/CommunityDiscussion/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: () => [{ type: 'CommunityDiscussion', id: 'LIST' }]
        }),
        getCommunityDiscussionById: builder.query<CommunityDiscussionModel, number>({
            query: id => `/CommunityDiscussion/${id}`,
            providesTags: result => result ? [{ type: 'CommunityDiscussion', id: result.id }] : [],
        }),
        getShortListCommunityDiscussionByCommunityId: builder.query<AllDiscussionModel, { communityId: number, pageSize: number }>({
            query: ({ communityId, pageSize }) => `/CommunityDiscussion/getShortListByDiscussionId/${communityId}?pageSize=${pageSize}`,
            providesTags: () => [{ type: 'CommunityDiscussion', id: 'LIST' }],
        }),
        getCommunityDiscussionByCommunityId: builder.query<AllDiscussionModel, { communityId: number, page: number, pageSize: number }>({
            query: ({ communityId, page, pageSize }) => `/CommunityDiscussion/getByCommunityId/${communityId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.communityId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.discussions.length = 0;
                    currentCache.discussions.push(...newItems.discussions);
                    return;
                }

                newItems.discussions.forEach(item => {
                    const index = currentCache.discussions.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.discussions.push(item);
                    } else {
                        currentCache.discussions[index] = item;
                    }
                });

                currentCache.discussions.sort(
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
                { type: 'CommunityDiscussion', id: 'LIST' },
                ...(result?.discussions.map(post => ({
                    type: 'CommunityDiscussion' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
    })
})

export const {
    useCreateCommunityDiscussionMutation,
    useUpdateCommunityDiscussionMutation,
    useRemoveCommunityDiscussionMutation,
    useGetCommunityDiscussionByIdQuery,
    useGetShortListCommunityDiscussionByCommunityIdQuery,
    useGetCommunityDiscussionByCommunityIdQuery,
} = CommunityDiscussionApi;