import { PostApi } from "../core/Post.api";

export const UserPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostLike: builder.mutation({
            query: userPostLike => ({
                body: userPostLike,
                url: '/UserPostLike',
                method: 'POST'
            })
        }),
        removeUserPostLike: builder.mutation({
            query: id => ({
                url: `/UserPostLike/${id}`,
                method: 'DELETE'
            })
        }),
        searchUserPostLikeByPostId: builder.query({
            query: (id) => `/UserPostLike/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostLike', id })), { type: 'UserPostLike' }]
                    : [{ type: 'UserPostLike' }]
        }),
    })
})

export const {
    useCreateUserPostLikeMutation,
    useRemoveUserPostLikeMutation,
    useLazySearchUserPostLikeByPostIdQuery,
} = UserPostLikeApi;