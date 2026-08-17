import type { AllUserPostCommentsModel } from '../types/AllUserPostCommentsModel';
import type { UserPostCommentModel } from '../types/UserPostCommentModel';
import { PostApi } from './Post.api';

export const UserPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostComment: builder.mutation<UserPostCommentModel, UserPostCommentModel>({
            query: userPostComment => ({
                body: userPostComment,
                url: '/UserPostComment',
                method: 'POST'
            }),
            async onQueryStarted({}, { dispatch, queryFulfilled }) {
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
        removeUserPostComment: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'UserPostComment', id }],
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