import { PostApi } from "../core/Post.api";

export const UserPostApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        getUserPostCountByUserId: builder.query({
            query: (appUserId) => `/UserPost/count/${appUserId}`,
        }),
        getUserPostCountByListOfUserId: builder.query({
            query: (appUserIds) => `/UserPost/countByListOfAppUsers/${appUserIds}`,
        }),
        getUserPostById: builder.query({
            query: (id) => `/UserPost/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [{ type: 'UserPost', id: result.id }]
                    : [{ type: 'UserPost' }]
        }),
        createUserPost: builder.mutation({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPost', result }],
        }),
        updateUserPost: builder.mutation({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'PUT'
            }),
            invalidatesTags: (result, error) => [{ type: 'UserPost', result }],
        }),
        removeUserPost: builder.mutation({
            query: id => ({
                url: `/UserPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'UserPost', arg }]
        }),
    })
})

export const {
    useGetUserPostCountByUserIdQuery,
    useLazyGetUserPostCountByUserIdQuery,
    useGetUserPostCountByListOfUserIdQuery,
    useLazyGetUserPostCountByListOfUserIdQuery,
    useGetUserPostByIdQuery,
    useLazyGetUserPostByIdQuery,
    useCreateUserPostMutation,
    useUpdateUserPostMutation,
    useRemoveUserPostMutation
} = UserPostApi;