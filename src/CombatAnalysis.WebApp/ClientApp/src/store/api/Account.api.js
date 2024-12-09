import { UserApi } from "./UserApi";

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query({
            query: (id) => `/Account/${id}`,
            providesTags: (result, error, id) =>
                result ? [{ type: 'Account', id: result.id }] : ['Account'],
        }),
        findByIdenityUserId: builder.query({
            query: (identityUserId) => `/Account/find/${identityUserId}`,
            providesTags: (result, error, id) =>
                result ? [{ type: 'Account', id: result.id }] : ['Account'],
        }),
        checkIfUserExist: builder.query({
            query: (email) => `/Account/checkIfUserExist/${email}`
        }),
        editAsync: builder.mutation({
            query: user => ({
                body: user,
                url: '/Account',
                method: 'PUT'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Account', id: arg.id }],
        }),
        logoutAsync: builder.mutation({
            query: () => ({
                url: '/Account/logout',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Account', id: arg.id }],
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