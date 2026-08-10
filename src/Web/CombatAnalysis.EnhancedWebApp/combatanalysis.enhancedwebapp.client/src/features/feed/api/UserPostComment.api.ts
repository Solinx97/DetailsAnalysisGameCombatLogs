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
            invalidatesTags: result => result ? [{ type: 'UserPostComment', id: result.id }] : [],
        }),
        updateUserPostComment: builder.mutation<void, UserPostCommentModel>({
            query: userPostComment => ({
                body: userPostComment,
                url: '/UserPostComment',
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
        getUserPostCommentByUserPostId: builder.query<UserPostCommentModel[], { userPostId: number, page: number, pageSize: number }>({
            query: ({ userPostId, page, pageSize }) => `/UserPostComment/getByUserPostId/${userPostId}?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(userPostComment => ({ type: 'UserPostComment' as const, id: userPostComment.id })),
                        { type: 'UserPostComment', id: 'LIST' },
                    ]
                    : [{ type: 'UserPostComment', id: 'LIST' }]
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