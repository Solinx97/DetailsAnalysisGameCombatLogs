import type { CommunityUserModel } from '../types/CommunityUserModel';
import { CommunityApi } from './Community.api';

export const CommunityUserApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityUser: builder.mutation<CommunityUserModel, CommunityUserModel>({
            query: (communityUser) => ({
                body: communityUser,
                url: '/CommunityUser',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityUser', id: result.id }] : [],
        }),
        removeCommunityUser: builder.mutation<void, string>({
            query: id => ({
                url: `/CommunityUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityUser', id }]
        }),
        getUsersByCommunityId: builder.query<CommunityUserModel[], number>({
            query: communityId => `/CommunityUser/getByCommunityId/${communityId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityUser => ({ type: 'CommunityUser' as const, id: communityUser.id })),
                        { type: 'CommunityUser', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityUser', id: 'LIST' }]
        }),
        communityUserFindByUserId: builder.query<CommunityUserModel[], string>({
            query: userId => `/CommunityUser/findByUserId/${userId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityUser => ({ type: 'CommunityUser' as const, id: communityUser.id })),
                        { type: 'CommunityUser', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityUser', id: 'LIST' }]
        }),
    })
})

export const {
    useGetUsersByCommunityIdQuery,
    useLazyGetUsersByCommunityIdQuery,
    useCommunityUserFindByUserIdQuery,
    useLazyCommunityUserFindByUserIdQuery,
    useCreateCommunityUserMutation,
    useRemoveCommunityUserMutation
} = CommunityUserApi;