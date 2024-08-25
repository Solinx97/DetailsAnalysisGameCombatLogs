import { CommunityApi } from "../CommunityApi";

export const PostLikeApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createPostLikeAsync: builder.mutation({
            query: postLike => ({
                body: postLike,
                url: '/UserPostLike',
                method: 'POST'
            })
        }),
        removePostLikeAsync: builder.mutation({
            query: id => ({
                url: `/UserPostLike/${id}`,
                method: 'DELETE'
            })
        }),
        searchPostLikeByPostId: builder.query({
            query: (id) => `/UserPostLike/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostLike', id })), { type: 'UserPostLike' }]
                    : [{ type: 'UserPostLike' }]
        }),
    })
})

export const {
    useCreatePostLikeAsyncMutation,
    useRemovePostLikeAsyncMutation,
    useLazySearchPostLikeByPostIdQuery,
} = PostLikeApi;