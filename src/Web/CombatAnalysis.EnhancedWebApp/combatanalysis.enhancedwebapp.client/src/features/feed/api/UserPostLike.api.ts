import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';

export const UserPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostLike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostLike => ({
                body: userPostLike,
                url: '/UserPostLike',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'UserPostLike', id: result.id }] : [],
        }),
        removeUserPostLike: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostLike/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'UserPostLike', id }],
        }),
        countUserPostLikeByPostId: builder.query<number, number>({
            query: id => `/UserPostLike/count/${id}`,
            providesTags: () => [{ type: 'UserPostLike', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateUserPostLikeMutation,
    useRemoveUserPostLikeMutation,
    useCountUserPostLikeByPostIdQuery,
} = UserPostLikeApi;