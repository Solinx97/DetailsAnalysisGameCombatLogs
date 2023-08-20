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
    useRemoveInviteAsyncMutation,
} = InviteToCommunityApi;