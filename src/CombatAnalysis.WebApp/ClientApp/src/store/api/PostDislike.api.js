import { ChatApi } from "./ChatApi";

export const PostDislikeApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostDislikeAsync: builder.mutation({
            query: postDislike => ({
                body: postDislike,
                url: '/PostDislike',
                method: 'POST'
            })
        }),
        removePostDislikeAsync: builder.mutation({
            query: id => ({
                url: `/PostDislike/${id}`,
                method: 'DELETE'
            })
        }),
        searchPostDislikeByPostId: builder.query({
            query: (id) => `/PostDislike/searchByPostId/${id}`
        }),
    })
})

export const {
    useCreatePostDislikeAsyncMutation,
    useRemovePostDislikeAsyncMutation,
    useLazySearchPostDislikeByPostIdQuery,
} = PostDislikeApi;