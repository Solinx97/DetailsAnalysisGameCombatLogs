import type { CommunityPostCommentModel } from '../types/CommunityPostCommentModel';
import { PostApi } from './Post.api';

export const CommunityPostCommentApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostComment: builder.mutation<CommunityPostCommentModel, CommunityPostCommentModel>({
            query: communityPostComment => ({
                body: communityPostComment,
                url: '/CommunityPostComment',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityPostComment', id: result.id }] : [],
        }),
        updateCommunityPostComment: builder.mutation<void, { id: number, comment: CommunityPostCommentModel }>({
            query: ({ id, comment }) => ({
                body: comment,
                url: `/CommunityPostComment/${id}`,
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, communityPostComment) => [{ type: 'CommunityPostComment', id: communityPostComment.id }],
        }),
        removeCommunityPostComment: builder.mutation<void, number>({
            query: id => ({
                url: `/CommunityPostComment/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'CommunityPostComment', id }],
        }),
        getCommunityPostCommentByPostId: builder.query<CommunityPostCommentModel[], { communityPostId: number, page: number, pageSize: number }>({
            query: ({ communityPostId, page, pageSize }) => `/CommunityPostComment/getByCommunityPostId/${communityPostId}?page=${page}&pageSize=${pageSize}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(communityPostComment => ({ type: 'CommunityPostComment' as const, id: communityPostComment.id })),
                        { type: 'CommunityPostComment', id: 'LIST' },
                    ]
                    : [{ type: 'CommunityPostComment', id: 'LIST' }]
        }),
        countCommunityPostCommentByPostId: builder.query<number, number>({
            query: id => `/CommunityPostComment/count/${id}`,
            providesTags: () => [{ type: 'CommunityPostComment', id: 'LIST' }]
        }),
    })
});

export const {
    useCreateCommunityPostCommentMutation,
    useUpdateCommunityPostCommentMutation,
    useRemoveCommunityPostCommentMutation,
    useGetCommunityPostCommentByPostIdQuery,
    useCountCommunityPostCommentByPostIdQuery,
} = CommunityPostCommentApi;