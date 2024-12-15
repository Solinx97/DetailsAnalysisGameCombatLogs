import { PostApi } from "../core/Post.api";

export const CommunityPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostLike: builder.mutation({
            query: communityPostLike => ({
                body: communityPostLike,
                url: '/CommunityPostLike',
                method: 'POST'
            })
        }),
        removeCommunityPostLike: builder.mutation({
            query: id => ({
                url: `/CommunityPostLike/${id}`,
                method: 'DELETE'
            })
        }),
        searchCommunityPostLikeByPostId: builder.query({
            query: (id) => `/CommunityPostLike/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPostLike', id })), { type: 'CommunityPostLike' }]
                    : [{ type: 'CommunityPostLike' }]
        }),
    })
})

export const {
    useCreateCommunityPostLikeMutation,
    useRemoveCommunityPostLikeMutation,
    useLazySearchCommunityPostLikeByPostIdQuery,
} = CommunityPostLikeApi;