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
        friendSearchByUserId: builder.query({
            query: (id) => `/Friend/searchByUserId/${id}`
        }),
        authenticationAsync: builder.query({
            query: () => '/Authentication',
            providesTags: (result, error, id) => [{ type: 'Authentication', id }]
        }),
        searchByToUserId: builder.query({
            query: (id) => `/RequestToConnect/searchByToUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }],
        }),
    })
})

export const {
    useGetUsersQuery,
    useGetCustomersQuery,
    useLazyGetCustomersQuery,
    useFriendSearchByUserIdQuery,
    useAuthenticationAsyncQuery,
    useLazyAuthenticationAsyncQuery,
    useSearchByToUserIdQuery,
} = UserApi;