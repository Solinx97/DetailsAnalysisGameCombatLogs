import { UserApi } from "./UserApi";

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query({
            query: (id) => `/Account/${id}`
        }),
        findByIdenityUserId: builder.query({
            query: (identityUserId) => `/Account/find/${identityUserId}`
        }),
        checkIfUserExist: builder.query({
            query: (email) => `/Account/checkIfUserExist/${email}`
        }),
        editAsync: builder.mutation({
            query: user => ({
                body: user,
                url: '/Account',
                method: 'PUT'
            })
        }),
        logoutAsync: builder.mutation({
            query: () => ({
                url: '/Account/logout',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'Authentication', result }],
        }),
    })
})

export const {
    useGetUserByIdQuery,
    useLazyGetUserByIdQuery,
    useLazyFindByIdenityUserIdQuery,
    useLazyCheckIfUserExistQuery,
    useEditAsyncMutation,
    useLogoutAsyncMutation
} = AccountApi;