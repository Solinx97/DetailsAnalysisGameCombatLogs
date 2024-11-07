import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTyes: [
        'Account',
        'Authentication',
        'Customer',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getUsers: builder.query({
            query: () => '/Account'
        }),
        getCustomers: builder.query({
            query: () => '/Customer'
        }),
        authenticationAsync: builder.query({
            query: () => '/Authentication',
            providesTags: (result, error, id) => [{ type: 'Authentication', id }]
        }),
        identity: builder.query({
            query: (identityPath) => `/Authentication/authorization?identityPath=${identityPath}`
        }),
        verifyEmail: builder.query({
            query: ({ identityPath, email }) => `/Authentication/verifyEmail?identityPath=${identityPath}&email=${email}`
        }),
        stateValidate: builder.query({
            query: (state) => `/Authentication/stateValidate?state=${state}`
        }),
    })
})

export const {
    useGetUsersQuery,
    useLazyGetUsersQuery,
    useGetCustomersQuery,
    useLazyGetCustomersQuery,
    useAuthenticationAsyncQuery,
    useLazyAuthenticationAsyncQuery,
    useLazyIdentityQuery,
    useLazyVerifyEmailQuery,
    useLazyStateValidateQuery,
} = UserApi;