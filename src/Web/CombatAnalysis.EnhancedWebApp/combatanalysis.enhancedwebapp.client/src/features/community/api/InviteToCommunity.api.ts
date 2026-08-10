import type { InviteToCommunityModel } from '../types/InviteToCommunityModel';
import { CommunityApi } from './Community.api';

export const InviteToCommunityApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createInviteAsync: builder.mutation<InviteToCommunityModel, InviteToCommunityModel>({
            query: invite => ({
                body: invite,
                url: '/InviteToCommunity',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'InviteToCommunity', id: result.id }] : [],
        }),
        removeCommunityInvite: builder.mutation<void, number>({
            query: id => ({
                url: `/InviteToCommunity/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'InviteToCommunity', id }],
        }),
        getInviteToCommunityById: builder.query<InviteToCommunityModel, number>({
            query: id => `/InviteToCommunity/${id}`,
            providesTags: result => result ? [{ type: 'InviteToCommunity', id: result.id }] : [],
        }),
        inviteGetByUserId: builder.query<InviteToCommunityModel[], string>({
            query: appUserId => `/InviteToCommunity/getByUserId/${appUserId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(inviteToCommunity => ({ type: 'InviteToCommunity' as const, id: inviteToCommunity.id })),
                        { type: 'InviteToCommunity', id: 'LIST' },
                    ]
                    : [{ type: 'InviteToCommunity', id: 'LIST' }]
        }),
        inviteIsExist: builder.query<boolean, { appUserId: string, communityId: number }>({
            query: ({ appUserId, communityId }) => `/InviteToCommunity/isExist?appUserId=${appUserId}&communityId=${communityId}`,
            providesTags: (_result, _error, { appUserId, communityId }) => [{ type: 'InviteToCommunity', id: `${appUserId}-${communityId}` }]
        }),
    })
})

export const {
    useCreateInviteAsyncMutation,
    useRemoveCommunityInviteMutation,
    useGetInviteToCommunityByIdQuery,
    useInviteGetByUserIdQuery,
    useLazyInviteIsExistQuery,
} = InviteToCommunityApi;