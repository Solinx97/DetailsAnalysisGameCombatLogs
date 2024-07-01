import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTyes: [
        'Account',
        'Authentication',
        'Customer',
        'Friend',
        'RequestToConnect',
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
    })
})

export const {
    useGetUsersQuery,
    useLazyGetUsersQuery,
    useGetCustomersQuery,
    useLazyGetCustomersQuery,
    useAuthenticationAsyncQuery,
    useLazyAuthenticationAsyncQuery,
} = UserApi;