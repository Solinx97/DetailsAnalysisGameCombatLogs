import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const ChatApi = createApi({
    reducerPath: 'chatApi',
    tagTyes: [
        'Post',
        'Community',
        'CommunityPost',
        'CommunityUser',
        'PersonalChat',
        'GroupChatMessage',
        'UserPost',
        'RequestToConnect',
        'InviteToCommunity'
    ],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getPosts: builder.query({
            query: () => '/Post'
        }),
        getCommunities: builder.query({
            query: () => '/Community'
        }),
        postSearchByCommunityIdAsync: builder.query({
            query: (id) => `/CommunityPost/searchByCommunityId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityPost', id }],
        }),
        searchByCommunityIdAsync: builder.query({
            query: (id) => `/CommunityUser/searchByCommunityId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityUser', id }]
        }),
        findGroupChatMessageByChatId: builder.query({
            query: (id) => `/GroupChatMessage/findByChatId/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        userPostSearchByUserId: builder.query({
            query: (id) => `/UserPost/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UserPost', id }],
        }),
        getInviteToCommunityId: builder.query({
            query: (id) => `/InviteToCommunity/${id}`,
            providesTags: (result, error, id) => [{ type: 'InviteToCommunity', id }],
        }),
    })
})

export const {
    useGetPostQuery,
    useGetCommunitiesQuery,
    useLazyGetCommunitiesQuery,
    useFindGroupChatMessageByChatIdQuery,
    useUserPostSearchByUserIdQuery,
    useSearchByCommunityIdAsyncQuery,
    usePostSearchByCommunityIdAsyncQuery,
    useLazyPostSearchByCommunityIdAsyncQuery,
    useAuthenticationAsyncQuery,
    useGetInviteToCommunityIdQuery,
} = ChatApi;