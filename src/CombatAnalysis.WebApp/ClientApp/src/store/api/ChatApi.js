import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const apiURL = '/api/v1';

export const ChatApi = createApi({
    reducerPath: 'chatApi',
    tagTyes: ['Post', 'CommunityPost', 'CommunityUser', 'UserPost'],
    baseQuery: fetchBaseQuery({
        baseUrl: apiURL
    }),
    endpoints: builder => ({
        getPosts: builder.query({
            query: () => 'Post'
        }),
        getCommunities: builder.query({
            query: () => 'Community'
        }),
        userPostSearchByUserId: builder.query({
            query: (id) => `/UserPost/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'UserPost', id }],
        }),
        searchByCommunityIdAsync: builder.query({
            query: (id) => `/CommunityUser/searchByCommunityId/${id}`,
        }),
        postSearchByCommunityIdAsync: builder.query({
            query: (id) => `/CommunityPost/searchByCommunityId/${id}`,
            providesTags: (result, error, id) => [{ type: 'CommunityPost', id }],
        }),
    })
})

export const {
    useGetPostQuery,
    useGetCommunitiesQuery,
    useUserPostSearchByUserIdQuery,
    useSearchByCommunityIdAsyncQuery,
    usePostSearchByCommunityIdAsyncQuery,
    useLazyPostSearchByCommunityIdAsyncQuery,
    useAuthenticationAsyncQuery
} = ChatApi;