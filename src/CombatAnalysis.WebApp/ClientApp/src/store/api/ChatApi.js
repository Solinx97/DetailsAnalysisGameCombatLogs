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
        'PersonalChatMessage',
        'GroupChat',
        'GroupChatMessage',
        'GroupChatUser',
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
        }),
        getPersonalChatsByUserId: builder.query({
            query: (id) => `/PersonalChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'PersonalChat', id }],
        }),
        findPersonalChatMessageByChatId: builder.query({
            query: (id) => `/PersonalChatMessage/findByChatId/${id}`,
            providesTags: (result, error, id) => [{ type: 'PersonalChatMessage', id }],
        }),
        getGroupChatById: builder.query({
            query: (id) => `/GroupChat/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChat', id }],
        }),
        findGroupChatMessageByChatId: builder.query({
            query: (id) => `/GroupChatMessage/findByChatId/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        getGroupChatUserByUserId: builder.query({
            query: (id) => `/GroupChatUser/${id}`,
            providesTags: (result, error, id) => [{ type: 'GroupChatUser', id }],
        }),
        userPostSearchByUserId: builder.query({
            query: (id) => `/UserPost/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UserPost', id }],
        }),
        getInviteToCommunityId: builder.query({
            query: (id) => `/InviteToCommunity/${id}`
        }),
    })
})

export const {
    useGetPostQuery,
    useGetCommunitiesQuery,
    useGetPersonalChatsByUserIdQuery,
    useGetGroupChatByIdQuery,
    useGetGroupChatUserByUserIdQuery,
    useFindPersonalChatMessageByChatIdQuery,
    useFindGroupChatMessageByChatIdQuery,
    useUserPostSearchByUserIdQuery,
    useSearchByCommunityIdAsyncQuery,
    usePostSearchByCommunityIdAsyncQuery,
    useLazyPostSearchByCommunityIdAsyncQuery,
    useAuthenticationAsyncQuery,
    useGetInviteToCommunityIdQuery,
} = ChatApi;