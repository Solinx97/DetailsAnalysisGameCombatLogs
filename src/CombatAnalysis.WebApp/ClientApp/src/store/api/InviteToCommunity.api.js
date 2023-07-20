import { ChatApi } from "./ChatApi";

export const InviteToCommunityApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createInviteAsync: builder.mutation({
            query: invite => ({
                body: invite,
                url: '/InviteToCommunity',
                method: 'POST'
            })
        }),
        inviteSearchByUserId: builder.query({
            query: (id) => `/InviteToCommunity/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UserPost', id }],
        }),
        getInviteToCommunityId: builder.query({
            query: (id) => `/InviteToCommunity/${id}`
        }),
        removeInviteAsync: builder.mutation({
            query: id => ({
                url: `/CommunityUser/${id}`,
                method: 'DELETE'
            })
        })
    })
})

export const {
    useCreateInviteAsyncMutation,
    useInviteSearchByUserIdQuery,
    useGetInviteToCommunityIdQuery,
    useRemoveInviteAsyncMutation,
} = InviteToCommunityApi;