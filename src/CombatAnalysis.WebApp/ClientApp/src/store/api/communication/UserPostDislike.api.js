import { CommunityApi } from "../CommunityApi";

export const UserPostDislikeApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostDislike: builder.mutation({
            query: userPostDislike => ({
                body: userPostDislike,
                url: '/UserPostDislike',
                method: 'POST'
            })
        }),
        removeUserPostDislike: builder.mutation({
            query: id => ({
                url: `/UserPostDislike/${id}`,
                method: 'DELETE'
            })
        }),
        searchUserPostDislikeByPostId: builder.query({
            query: (id) => `/UserPostDislike/searchByPostId/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [...result.map(({ id }) => ({ type: 'UserPostDislike', id })), { type: 'UserPostDislike' }]
                    : [{ type: 'UserPostDislike' }]
        }),
    })
})

export const {
    useCreateUserPostDislikeMutation,
    useRemoveUserPostDislikeMutation,
    useLazySearchUserPostDislikeByPostIdQuery,
} = UserPostDislikeApi;