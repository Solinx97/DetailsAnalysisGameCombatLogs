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
        removeCommunityUser: builder.mutation<void, { id: string, communityId: number }>({
            query: ({ id, communityId }) => ({
                url: `/CommunityUser/${id}?communityId=${communityId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'CommunityUser', id: args.id }]
        }),
        leaveCommunityUser: builder.mutation<void, { appUserId: string, communityId: number }>({
            query: ({ appUserId, communityId }) => ({
                url: `/CommunityUser/leave?appUserId=${appUserId}&communityId=${communityId}`,
                method: 'DELETE'
            }),
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
    useRemoveCommunityUserMutation,
    useLeaveCommunityUserMutation
} = CommunityUserApi;