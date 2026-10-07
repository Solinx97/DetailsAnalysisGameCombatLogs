import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { AppUserModel } from '../types/AppUserModel';
import type { CustomerModel } from '../types/CustomerModel';
import type { IdentityRedirectModel } from '../types/IdentityRedirectModel';

const apiURL = '/api/v1';

export const UserApi = createApi({
    reducerPath: 'userApi',
    tagTypes: [
        'User',
        'Authentication',
        'Customer',
        'Friend',
        'RequestToConnect',
        'Identity',
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        findUser: builder.query<AppUserModel[], string>({
            query: startAt => `/User/find?startAt=${startAt}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(user => ({ type: 'User' as const, id: user.id })),
                        { type: 'User', id: 'LIST' },
                    ]
                    : [{ type: 'User', id: 'LIST' }]
        }),
        getUsers: builder.query<AppUserModel[], void>({
            query: () => '/User',
            providesTags: result =>
                result
                    ? [
                        ...result.map(user => ({ type: 'User' as const, id: user.id })),
                        { type: 'User', id: 'LIST' },
                    ]
                    : [{ type: 'User', id: 'LIST' }]
        }),
        getCustomers: builder.query<CustomerModel[], void>({
            query: () => '/Customer',
            providesTags: result =>
                result
                    ? [
                        ...result.map(customer => ({ type: 'Customer' as const, id: customer.id })),
                        { type: 'Customer', id: 'LIST' },
                    ]
                    : [{ type: 'Customer', id: 'LIST' }]
        }),
        authentication: builder.query<AppUserModel, void>({
            query: () => '/Authentication'
        }),
        authorization: builder.query<{ uri: string }, string>({
            query: identityPath => `/Authentication/authorization?identityPath=${identityPath}`,
        }),
        registration: builder.query<{ uri: string }, string>({
            query: identityPath => `/Authentication/registration?identityPath=${identityPath}`,
        }),
        verifyEmail: builder.query<IdentityRedirectModel, { identityPath: string, email: string }>({
            query: ({ identityPath, email }) => `/Authentication/verifyEmail?identityPath=${identityPath}&email=${email}`,
        }),
        stateValidate: builder.query <void, string>({
            query: state => `/Authentication/stateValidate?state=${state}`,
        }),
        cancelAuthorization: builder.query<void, void>({
            query: () => "/Authentication/cancel",
        }),
    })
})

export const {
    useLazyFindUserQuery,
    useGetUsersQuery,
    useGetCustomersQuery,
    useLazyGetCustomersQuery,
    useAuthenticationQuery,
    useLazyAuthenticationQuery,
    useLazyAuthorizationQuery,
    useLazyRegistrationQuery,
    useLazyVerifyEmailQuery,
    useLazyStateValidateQuery,
    useLazyCancelAuthorizationQuery,
} = UserApi;