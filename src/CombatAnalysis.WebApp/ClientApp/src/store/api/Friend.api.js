import { ChatApi } from "./ChatApi";

export const FriendApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createFriendAsync: builder.mutation({
            query: friend => ({
                body: friend,
                url: '/Friend',
                method: 'POST'
            })
        }),
        friendSearchByUserId: builder.query({
            query: (id) => `/Friend/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'Friend', id }],
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
    useFriendSearchByUserIdQuery,
    useRemoveFriendAsyncMutation,
} = FriendApi;