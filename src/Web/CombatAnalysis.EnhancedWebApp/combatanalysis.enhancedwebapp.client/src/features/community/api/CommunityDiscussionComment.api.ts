import type { CommunityDiscussionCommentModel } from '../types/CommunityDiscussionCommentModel';
import { CommunityApi } from './Community.api';

export const CommunityDiscussionCommentApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityDiscussionCommentAsync: builder.mutation<CommunityDiscussionCommentModel, CommunityDiscussionCommentModel>({
            query: discussion => ({
                body: discussion,
                url: '/CommunityDiscussionComment',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityDiscussionComment', id: result.id }] : [],
        }),
        updateCommunityDiscussionCommentAsync: builder.mutation<void, { id: number, comment: CommunityDiscussionCommentModel }>({
            query: ({ id, comment }) => ({
                body: comment,
                url: `/CommunityDiscussionComment/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, discussion) => [{ type: 'CommunityDiscussionComment', id: discussion.id }]
        }),
        removeCommunityDiscussionCommentAsync: builder.mutation<void, { id: number, discussionId: number }>({
            query: ({ id, discussionId }) => ({
                url: `/CommunityDiscussionComment/${id}?discussionId=${discussionId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'CommunityDiscussionComment', id: args.id }]
        }),
        getCommunityDiscussionCommentById: builder.query<CommunityDiscussionCommentModel, number>({
            query: id => `/CommunityDiscussionComment/${id}`,
            providesTags: result => result ? [{ type: 'CommunityDiscussionComment', id: result.id }] : [],
        }),
        getCommunityDiscussionCommentByDiscussionId: builder.query<CommunityDiscussionCommentModel[], { discussionId: number, page: number, pageSize: number }>({
            query: ({ discussionId, page, pageSize }) => `/CommunityDiscussionComment/getByDiscussionId/${discussionId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.discussionId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.discussionId !== previousArg?.discussionId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'CommunityDiscussionComment', id: 'LIST' },
                ...(result?.map(comment => ({
                    type: 'CommunityDiscussionComment' as const,
                    id: comment.id
                })) ?? [])
            ]
        }),
    })
})

export const {
    useCreateCommunityDiscussionCommentAsyncMutation,
    useUpdateCommunityDiscussionCommentAsyncMutation,
    useRemoveCommunityDiscussionCommentAsyncMutation,
    useGetCommunityDiscussionCommentByIdQuery,
    useGetCommunityDiscussionCommentByDiscussionIdQuery,
} = CommunityDiscussionCommentApi;