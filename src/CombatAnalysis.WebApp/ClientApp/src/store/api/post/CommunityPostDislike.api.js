import { PostApi } from "../core/Post.api";

export const CommunityPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostDislike: builder.mutation({
            query: communityPostDislike => ({
                body: communityPostDislike,
                url: '/CommunityPostDislike',
                method: 'POST'
            })
        }),
        removeCommunityPostDislike: builder.mutation({
            query: id => ({
                url: `/CommunityPostDislike/${id}`,
                method: 'DELETE'
            })
        }),
        searchCommunityPostDislikeByPostId: builder.query({
            query: (id) => `/CommunityPostDislike/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'CommunityPostDislike', id })), { type: 'CommunityPostDislike' }]
                    : [{ type: 'CommunityPostDislike' }]
        }),
    })
})

export const {
    useCreateCommunityPostDislikeMutation,
    useRemoveCommunityPostDislikeMutation,
    useLazySearchCommunityPostDislikeByPostIdQuery,
} = CommunityPostDislikeApi;