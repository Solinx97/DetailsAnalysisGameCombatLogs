import { ChatApi } from "./ChatApi";

export const PostApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createPostAsync: builder.mutation({
            query: post => ({
                body: post,
                url: '/Post',
                method: 'POST'
            })
        }),
        getPostById: builder.query({
            query: (id) => `/Post/${id}`
        }),
        searchByUserId: builder.query({
            query: (id) => `/Post/searchByUserId/${id}`
        }),
    })
})

export const {
    useCreatePostAsyncMutation,
    useGetPostByIdQuery,
    useSearchByUserIdQuery
} = PostApi;