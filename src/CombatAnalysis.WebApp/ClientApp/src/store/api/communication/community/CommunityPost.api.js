import { ChatApi } from "../../ChatApi";

export const CommunityPostApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostAsync: builder.mutation({
            query: communityPost => ({
                body: communityPost,
                url: '/CommunityPost',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPost', result }],
        }),
        getCommunityPostByPostId: builder.query({
            query: (id) => `/CommunityPost/searchByPostId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityPost', id }]
        }),
        removeCommunityPost: builder.mutation({
            query: id => ({
                url: `/CommunityPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPost', result }],
        })
    })
})

export const {
    useCreateCommunityPostAsyncMutation,
    useLazyGetCommunityPostByPostIdQuery,
    useRemoveCommunityPostMutation,
} = CommunityPostApi;