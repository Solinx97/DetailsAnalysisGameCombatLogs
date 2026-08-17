import type { AllDiscussionCommentModel } from '../types/AllDiscussionCommentModel';
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
            async onQueryStarted({ }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdComment } = await queryFulfilled;

                    dispatch(
                        CommunityDiscussionCommentApi.util.updateQueryData(
                            'getCommunityDiscussionCommentByDiscussionId',
                            {
                                discussionId: createdComment.communityDiscussionId,
                                page: 1,
                                pageSize: 5
                            },
                            draft => {
                                const exists = draft.comments.some(
                                    x => x.id === createdComment.id
                                );

                                if (!exists) {
                                    draft.comments.unshift(createdComment);
                                }
                            }
                        )
                    );
                } catch {
                    // DELETE failed
                }
            }
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
        getCommunityDiscussionCommentByDiscussionId: builder.query<AllDiscussionCommentModel, { discussionId: number, page: number, pageSize: number }>({
            query: ({ discussionId, page, pageSize }) => `/CommunityDiscussionComment/getByDiscussionId/${discussionId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.discussionId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.comments.length = 0;
                    currentCache.comments.push(...newItems.comments);
                    return;
                }

                newItems.comments.forEach(item => {
                    const index = currentCache.comments.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.comments.push(item);
                    } else {
                        currentCache.comments[index] = item;
                    }
                });

                currentCache.comments.sort(
                    (a, b) =>
                        new Date(b.createdAt).getTime() -
                        new Date(a.createdAt).getTime()
                );
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
                ...(result?.comments.map(post => ({
                    type: 'CommunityDiscussionComment' as const,
                    id: post.id
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