import { CommunityApi } from "../core/Community.api";

export const InviteToCommunityApi = CommunityApi.injectEndpoints({
    tagTyes: [
        'InviteToCommunity',
    ],
    endpoints: builder => ({
        getInviteToCommunityById: builder.query({
            query: (id) => `/InviteToCommunity/${id}`,
            providesTags: (result, error, arg) =>
                result
                    ? [{ type: 'InviteToCommunity', id: result.id }]
                    : [{ type: 'InviteToCommunity' }]
        }),
        createInviteAsync: builder.mutation({
            query: invite => ({
                body: invite,
                url: '/InviteToCommunity',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'InviteToCommunity', result }],
        }),
        inviteSearchByUserId: builder.query({
            query: (id) => `/InviteToCommunity/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'InviteToCommunity', id }],
        }),
        inviteIsExist: builder.query({
            query: (arg) => {
                const { peopleId, communityId } = arg;
                return {
                    url: `/InviteToCommunity/isExist?peopleId=${peopleId}&communityId=${communityId}`,
                }
            },
            providesTags: (result, error, id) => [{ type: 'RequestToConnect', id }]
        }),
        removeCommunityInvite: builder.mutation({
            query: id => ({
                url: `/InviteToCommunity/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'InviteToCommunity', result }],
        })
    })
})

export const {
    useGetInviteToCommunityByIdQuery,
    useCreateInviteAsyncMutation,
    useInviteSearchByUserIdQuery,
    useLazyInviteIsExistQuery,
    useRemoveCommunityInviteMutation,
} = InviteToCommunityApi;