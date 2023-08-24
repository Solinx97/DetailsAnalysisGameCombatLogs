import { UserApi } from "./UserApi";

export const FriendApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createFriendAsync: builder.mutation({
            query: friend => ({
                body: friend,
                url: '/Friend',
                method: 'POST'
            }),
            invalidatesTags: (result, error, arg) => [{ type: 'Friend', arg }]
        }),
        removeFriendAsync: builder.mutation({
            query: id => ({
                url: `/Friend/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'Friend', result }],
        }),
        friendSearchMyFriends: builder.query({
            query: (currentUser) => `/Friend/searchMyFriends/${currentUser}`,
            providesTags: (result, error, id) => [{ type: 'Friend', id }],
        }),
    })
})

export const {
    useCreateFriendAsyncMutation,
    useRemoveFriendAsyncMutation,
    useFriendSearchMyFriendsQuery
} = FriendApi;