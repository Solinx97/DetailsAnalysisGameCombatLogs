import { ChatApi } from "./ChatApi";

export const CommunityPostApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostAsync: builder.mutation({
            query: communityPost => ({
                body: communityPost,
                url: '/CommunityPost',
                method: 'POST'
            }),
            invalidatesTags: (result, error) => [{ type: 'CommunityPost', result }],
        })
    })
})

export const {
    useCreateCommunityPostAsyncMutation
} = CommunityPostApi;