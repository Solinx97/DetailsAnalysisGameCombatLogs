import { ChatApi } from "../ChatApi";

export const PostLikeApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostLikeAsync: builder.mutation({
            query: postLike => ({
                body: postLike,
                url: '/PostLike',
                method: 'POST'
            })
        }),
        removePostLikeAsync: builder.mutation({
            query: id => ({
                url: `/PostLike/${id}`,
                method: 'DELETE'
            })
        }),
        searchPostLikeByPostId: builder.query({
            query: (id) => `/PostLike/searchByPostId/${id}`
        }),
    })
})

export const {
    useCreatePostLikeAsyncMutation,
    useRemovePostLikeAsyncMutation,
    useLazySearchPostLikeByPostIdQuery,
} = PostLikeApi;