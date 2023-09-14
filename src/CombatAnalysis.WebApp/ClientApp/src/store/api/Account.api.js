import { UserApi } from "./UserApi";

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query({
            query: (id) => `/Account/${id}`
        }),
        checkIfUserExist: builder.query({
            query: (email) => `/Account/checkIfUserExist/${email}`
        }),
        loginAsync: builder.mutation({
            query: user => ({
                body: user,
                url: '/Account',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'Authentication', result }],
        }),
        registrationAsync: builder.mutation({
            query: user => ({
                body: user,
                url: '/Account/registration',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'Authentication', result }],
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
    useLazyCheckIfUserExistQuery,
    useLazyGetUserByIdQuery,
    useLoginAsyncMutation,
    useRegistrationAsyncMutation,
    useEditAsyncMutation,
    useLogoutAsyncMutation
} = AccountApi;