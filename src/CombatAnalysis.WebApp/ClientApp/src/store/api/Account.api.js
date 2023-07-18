import { UserApi } from "./UserApi";

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query({
            query: (id) => `/Account/${id}`
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
            })
        }),
        logoutAsync: builder.mutation({
            query: () => ({
                url: '/Account/logout',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'Authentication', result }],
        })
    })
})

export const { useGetUserByIdQuery, useLoginAsyncMutation, useRegistrationAsyncMutation, useLogoutAsyncMutation } = AccountApi;