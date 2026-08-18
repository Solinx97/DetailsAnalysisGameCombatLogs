import type { AllCommunityPostCommentsModel } from '../types/AllCommunityPostCommentsModel';
import type { CommunityPostCommentModel } from '../types/CommunityPostCommentModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const CommunityPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostComment: builder.mutation<CommunityPostCommentModel, { feedVersion: number, comment: CommunityPostCommentModel }>({
            query: ({ comment }) => ({
                body: comment,
                url: '/CommunityPostComment',
                method: 'POST'
            }),
            async onQueryStarted({ feedVersion }, { dispatch, queryFulfilled }) {
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

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId: createdComment.communityId!,
                                appUserId: createdComment.appUserId,
                                feedVersion,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdComment.communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                post.commentCount++;
                            }
                        )
                    );

                    dispatch(
                        UserFeedApi.util.updateQueryData(
                            'getFeed',
                            {
                                appUserId: createdComment.appUserId!,
                                page: 1,
                                pageSize: 10,
                                feedVersion
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdComment.communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                post.commentCount++;
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
        removeCommunityPostComment: builder.mutation<void, { id: number, communityPostId: number, communityId: number, appUserId: string, feedVersion: number }>({
            query: ({ id, communityPostId }) => ({
                url: `/CommunityPostComment/${id}?communityPostId=${communityPostId}`,
                method: 'DELETE'
            }),
            async onQueryStarted({ id, communityPostId, communityId, appUserId, feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    await queryFulfilled;

                    dispatch(
                        CommunityPostCommentApi.util.updateQueryData(
                            'getCommunityPostCommentByPostId',
                            {
                                communityPostId,
                                page: 1,
                                pageSize: 5
                            },
                            draft => {
                                const index = draft.comments.findIndex(
                                    post => post.id === id
                                );

                                if (index !== -1) {
                                    draft.comments.splice(index, 1);
                                }
                            }
                        )
                    );

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId,
                                appUserId,
                                feedVersion,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                post.commentCount--;
                            }
                        )
                    );

                    dispatch(
                        UserFeedApi.util.updateQueryData(
                            'getFeed',
                            {
                                appUserId,
                                page: 1,
                                pageSize: 10,
                                feedVersion
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                post.commentCount--;
                            }
                        )
                    );
                } catch {
                    // DELETE failed
                }
            }
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