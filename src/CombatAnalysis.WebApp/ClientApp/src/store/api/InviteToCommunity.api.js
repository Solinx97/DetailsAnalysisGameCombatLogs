import { ChatApi } from "./ChatApi";

export const InviteToCommunityApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
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
        removeInviteAsync: builder.mutation({
            query: id => ({
                url: `/InviteToCommunity/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'InviteToCommunity', result }],
        })
    })
})

export const {
    useCreateInviteAsyncMutation,
    useInviteSearchByUserIdQuery,
    useLazyInviteIsExistQuery,
    useRemoveInviteAsyncMutation,
} = InviteToCommunityApi;