import { ChatApi } from "../ChatApi";

export const PostDislikeApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostDislikeAsync: builder.mutation({
            query: postDislike => ({
                body: postDislike,
                url: '/UserPostDislike',
                method: 'POST'
            })
        }),
        removePostDislikeAsync: builder.mutation({
            query: id => ({
                url: `/UserPostDislike/${id}`,
                method: 'DELETE'
            })
        }),
        searchPostDislikeByPostId: builder.query({
            query: (id) => `/UserPostDislike/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostDislike', id })), { type: 'UserPostDislike' }]
                    : [{ type: 'UserPostDislike' }]
        }),
    })
})

export const {
    useCreatePostDislikeAsyncMutation,
    useRemovePostDislikeAsyncMutation,
    useLazySearchPostDislikeByPostIdQuery,
} = PostDislikeApi;