import { UserApi } from "./UserApi";

export const FriendApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        createFriendAsync: builder.mutation({
            query: friend => ({
                body: friend,
                url: '/Friend',
                method: 'POST'
            })
        }),
        removeFriendAsync: builder.mutation({
            query: id => ({
                url: `/Friend/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (result, error) => [{ type: 'Friend', result }],
        })
    })
})

export const {
    useCreateFriendAsyncMutation,
    useRemoveFriendAsyncMutation,
} = FriendApi;