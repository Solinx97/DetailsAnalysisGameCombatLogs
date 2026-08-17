import type { AllCommunityPostCommentsModel } from '../types/AllCommunityPostCommentsModel';
import type { CommunityPostCommentModel } from '../types/CommunityPostCommentModel';
import { PostApi } from './Post.api';

export const CommunityPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostComment: builder.mutation<CommunityPostCommentModel, CommunityPostCommentModel>({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'POST'
            }),
            async onQueryStarted({ }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdComment } = await queryFulfilled;

                    dispatch(
                        CommunityPostCommentApi.util.updateQueryData(
                            'getCommunityPostCommentByPostId',
                            {
                                communityPostId: createdComment.communityPostId,
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
        updateCommunityPostComment: builder.mutation<void, { id: number, comment: CommunityPostCommentModel }>({
            query: ({ id, comment }) => ({
                body: comment,
                url: `/CommunityPostComment/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, communityPostComment) => [{ type: 'CommunityPostComment', id: communityPostComment.id }],
        }),
        removeCommunityPostComment: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityPostComment', id }],
        }),
        getCommunityPostCommentByPostId: builder.query<AllCommunityPostCommentsModel, { communityPostId: number, page: number, pageSize: number }>({
            query: ({ communityPostId, page, pageSize }) => `/CommunityPostComment/getByCommunityPostId/${communityPostId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.communityPostId}`,
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
                    currentArg?.communityPostId !== previousArg?.communityPostId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'CommunityPostComment', id: 'LIST' },
                ...(result?.comments.map(post => ({
                    type: 'CommunityPostComment' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
        countCommunityPostCommentByPostId: builder.query<number, number>({
            query: id => `/CommunityPostComment/count/${id}`,
            providesTags: () => [{ type: 'CommunityPostComment', id: 'LIST' }]
        }),
    })
});

export const {
    useCreateCommunityPostCommentMutation,
    useUpdateCommunityPostCommentMutation,
    useRemoveCommunityPostCommentMutation,
    useGetCommunityPostCommentByPostIdQuery,
    useCountCommunityPostCommentByPostIdQuery,
} = CommunityPostCommentApi;