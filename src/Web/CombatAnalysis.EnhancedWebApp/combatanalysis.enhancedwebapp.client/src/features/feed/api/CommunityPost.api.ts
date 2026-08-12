import type { CommunityPostModel } from '../types/CommunityPostModel';
import { PostApi } from './Post.api';

export const CommunityPostApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPost: builder.mutation<CommunityPostModel, CommunityPostModel>({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityPost', id: result.id }] : [],
        }),
        updateCommunityPost: builder.mutation<void, CommunityPostModel>({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, post) => [{ type: 'CommunityPost', id: post.id }],
        }),
        removeCommunityPost: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityPost', id }]
        }),
        getCommunityPostById: builder.query<CommunityPostModel, number>({
            query: id => `/CommunityPost/${id}`,
            providesTags: result => result ? [{ type: 'CommunityPost', id: result.id }] : [],
        }),
        getCommunityPostCountByCommunityId: builder.query<number, number>({
            query: communityId => `/CommunityPost/count/${communityId}`,
        }),
        getCommunityPostCountByListOfCommunityId: builder.query<number, string>({
            query: collectionCommunityId => `/CommunityPost/countByListOfCommunityId/${collectionCommunityId}`,
        }),
    })
})

export const {
    useCreateCommunityPostMutation,
    useUpdateCommunityPostMutation,
    useRemoveCommunityPostMutation,
    useGetCommunityPostByIdQuery,
    useLazyGetCommunityPostByIdQuery,
    useGetCommunityPostCountByCommunityIdQuery,
    useGetCommunityPostCountByListOfCommunityIdQuery,
    useLazyGetCommunityPostCountByListOfCommunityIdQuery,
} = CommunityPostApi;