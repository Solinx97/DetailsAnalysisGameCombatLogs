import type { AppUserModel } from '../types/AppUserModel';
import { UserApi } from './User.api';

export const AccountApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getUserById: builder.query<AppUserModel, string>({
            query: id => `/User/${id}`,
            providesTags: result => result ? [{ type: 'User', id: result.id }] : [],
        }),
        getUserByIdentityUserId: builder.query<AppUserModel, string>({
            query: identityUserId => `/User/getByIdentityUserId/${identityUserId}`,
            providesTags: result => result ? [{ type: 'User', id: result.id }] : [],
        }),
        editAccount: builder.mutation<void, { id: string, user: AppUserModel }>({
            query: ({ id, user }) => ({
                body: user,
                url: `/User/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'User', id: args.id }],
        }),
    })
})

export const {
    useGetUserByIdQuery,
    useLazyGetUserByIdQuery,
    useGetUserByIdentityUserIdQuery,
    useEditAccountMutation,
} = AccountApi;