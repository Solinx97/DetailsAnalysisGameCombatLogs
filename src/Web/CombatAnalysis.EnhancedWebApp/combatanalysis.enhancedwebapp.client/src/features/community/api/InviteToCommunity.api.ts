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
        removeCommunityInvite: builder.mutation<void, { id: number, communityId: number }>({
            query: ({ id, communityId }) => ({
                url: `/InviteToCommunity/${id}?communityId=${communityId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'InviteToCommunity', id: args.id }],
        }),
        acceptCommunityInvite: builder.mutation<void, { id: number, communityId: number, appUserId: string }>({
            query: ({ id, communityId, appUserId }) => ({
                url: `/InviteToCommunity/accept/${id}?communityId=${communityId}&appUserId=${appUserId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'InviteToCommunity', id: args.id }],
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
    })
})

export const {
    useCreateInviteAsyncMutation,
    useRemoveCommunityInviteMutation,
    useAcceptCommunityInviteMutation,
    useGetInviteToCommunityByIdQuery,
    useInviteGetByUserIdQuery,
} = InviteToCommunityApi;