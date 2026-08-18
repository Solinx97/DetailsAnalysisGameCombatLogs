import type { AllUserPostCommentsModel } from '../types/AllUserPostCommentsModel';
import type { UserPostCommentModel } from '../types/UserPostCommentModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const UserPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostComment: builder.mutation<UserPostCommentModel, { feedVersion: number, comment: UserPostCommentModel }>({
            query: ({ comment }) => ({
                body: comment,
                url: '/UserPostComment',
                method: 'POST'
            }),
            async onQueryStarted({ feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdComment } = await queryFulfilled;

                    dispatch(
                        UserPostCommentApi.util.updateQueryData(
                            'getUserPostCommentByUserPostId',
                            {
                                userPostId: createdComment.userPostId,
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
                            'getUserPostsByUserId',
                            {
                                appUserId: createdComment.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdComment.userPostId
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
                                    x => x.id === createdComment.userPostId
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
        updateUserPostComment: builder.mutation<void, { id: number, comment: UserPostCommentModel }>({
            query: ({ id, comment }) => ({
                body: comment,
                url: `/UserPostComment/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, userPostComment) => [{ type: 'UserPostComment', id: userPostComment.id }],
        }),
        removeUserPostComment: builder.mutation<void, { id: number, userPostId: number, appUserId: string, feedVersion: number }>({
            query: ({ id, userPostId }) => ({
                url: `/UserPostComment/${id}?userPostId=${userPostId}`,
                method: 'DELETE'
            }),
            async onQueryStarted({ id, userPostId, appUserId, feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    await queryFulfilled;

                    dispatch(
                        UserPostCommentApi.util.updateQueryData(
                            'getUserPostCommentByUserPostId',
                            {
                                userPostId,
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
                            'getUserPostsByUserId',
                            {
                                appUserId,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === userPostId
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
                                    x => x.id === userPostId
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
        getUserPostCommentByUserPostId: builder.query<AllUserPostCommentsModel, { userPostId: number, page: number, pageSize: number }>({
            query: ({ userPostId, page, pageSize }) => `/UserPostComment/getByUserPostId/${userPostId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.userPostId}`,
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
                    currentArg?.userPostId !== previousArg?.userPostId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'UserPostComment', id: 'LIST' },
                ...(result?.comments.map(post => ({
                    type: 'UserPostComment' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
        countUserPostCommentByUserPostId: builder.query<number, number>({
            query: id => `/UserPostComment/count/${id}`,
            providesTags: () => [{ type: 'UserPostComment', id: 'LIST' }]
        }),
    })
});

export const {
    useCreateUserPostCommentMutation,
    useUpdateUserPostCommentMutation,
    useRemoveUserPostCommentMutation,
    useGetUserPostCommentByUserPostIdQuery,
    useCountUserPostCommentByUserPostIdQuery,
} = UserPostCommentApi;