import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';

export const UserPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostDislike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostDislike => ({
                body: userPostDislike,
                url: '/UserPostDislike',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'UserPostDislike', id: result.id }] : [],
        }),
        removeUserPostDislike: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPostDislike/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'UserPostDislike', id }],
        }),
        countUserPostDislikeByPostId: builder.query<number, number>({
            query: id => `/UserPostDislike/count/${id}`,
            providesTags: () => [{ type: 'UserPostLike', id: 'LIST' }]
        }),
    })
})

export const {
    useCreateUserPostDislikeMutation,
    useRemoveUserPostDislikeMutation,
    useCountUserPostDislikeByPostIdQuery,
} = UserPostDislikeApi;