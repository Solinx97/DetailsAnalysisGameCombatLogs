import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTyes: ['Account', 'Authentication', 'Customer'],
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
            query: (id) => `/Friend/searchByUserId/${id}`,
            keepUnusedDataFor: 5
        }),
        authenticationAsync: builder.query({
            query: () => '/Authentication',
            providesTags: (result, error, id) => [{ type: 'Authentication', id }]
        })
    })
})

export const {
    useGetUsersQuery,
    useGetCustomersQuery,
    useFriendSearchByUserIdQuery,
    useAuthenticationAsyncQuery,
    useLazyAuthenticationAsyncQuery,
} = UserApi;