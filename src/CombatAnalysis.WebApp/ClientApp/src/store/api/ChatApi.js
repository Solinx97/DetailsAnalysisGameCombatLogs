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
        'PersonalChatMessage',
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
        getMessagesByGroupChatId: builder.query({
            query: (arg) => {
                const { chatId, pageSize } = arg;
                return {
                    url: `/GroupChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
                }
            },
            transformResponse: (response) => {
                return response.reverse();
            },
            providesTags: (result, error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        getMoreMessagesByGroupChatId: builder.query({
            query: (arg) => {
                const { chatId, offset, pageSize } = arg;
                return {
                    url: `/GroupChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
                }
            },
            transformResponse: (response) => {
                return response.reverse();
            },
            providesTags: (result, error, id) => [{ type: 'GroupChatMessage', id }],
        }),
        getMessagesByPersonalChatId: builder.query({
            query: (arg) => {
                const { chatId, pageSize } = arg;
                return {
                    url: `/PersonalChatMessage/getByChatId?chatId=${chatId}&pageSize=${pageSize}`,
                }
            },
            transformResponse: (response) => {
                return response.reverse();
            },
            providesTags: (result, error, id) => [{ type: 'PersonalChatMessage', id }],
        }),
        getMoreMessagesByPersonalChatId: builder.query({
            query: (arg) => {
                const { chatId, offset, pageSize } = arg;
                return {
                    url: `/PersonalChatMessage/getMoreByChatId?chatId=${chatId}&offset=${offset}&pageSize=${pageSize}`,
                }
            },
            transformResponse: (response) => {
                return response.reverse();
            },
            providesTags: (result, error, id) => [{ type: 'PersonalChatMessage', id }],
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
    useGetMessagesByGroupChatIdQuery,
    useLazyGetMoreMessagesByGroupChatIdQuery,
    useGetMessagesByPersonalChatIdQuery,
    useLazyGetMoreMessagesByPersonalChatIdQuery,
    useLazyUserPostSearchByUserIdQuery,
    useUserPostSearchByUserIdQuery,
    useSearchByCommunityIdAsyncQuery,
    useLazySearchByCommunityIdAsyncQuery,
    usePostSearchByCommunityIdAsyncQuery,
    useLazyPostSearchByCommunityIdAsyncQuery,
    useAuthenticationAsyncQuery,
    useGetInviteToCommunityIdQuery,
} = ChatApi;