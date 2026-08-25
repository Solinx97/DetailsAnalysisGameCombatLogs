import type { CommunityPostModel } from '../types/CommunityPostModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const CommunityPostApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPost: builder.mutation<CommunityPostModel, { feedVersion: number, post: CommunityPostModel }>({
            query: ({ post }) => ({
                body: post,
                url: '/CommunityPost',
                method: 'POST'
            }),

            async onQueryStarted({ feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdPost } = await queryFulfilled;

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId: createdPost.communityId!,
                                appUserId: createdPost.appUserId!,
                                page: 1,
                                pageSize: 10,
                                feedVersion
                            },
                            draft => {
                                const exists = draft.posts.some(
                                    x => x.id === createdPost.id
                                );

                                if (!exists) {
                                    draft.posts.unshift(createdPost);
                                }
                            }
                        )
                    );
                } catch {
                    // creation failed
                }
            }
        }),
        updateCommunityPost: builder.mutation<void, CommunityPostModel>({
            query: post => ({
                body: post,
                url: '/CommunityPost',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, post) => [{ type: 'CommunityPost', id: post.id }],
        }),
        removeCommunityPost: builder.mutation<void, { id: number, communityId: number, appUserId: string, feedVersion: number }>({
            query: ({ id }) => ({
                url: `/CommunityPost/${id}`,
                method: 'DELETE'
            }),
            async onQueryStarted({ id, communityId, appUserId, feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    await queryFulfilled;

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId,
                                appUserId: appUserId!,
                                page: 1,
                                pageSize: 10,
                                feedVersion
                            },
                            draft => {
                                const index = draft.posts.findIndex(
                                    post => post.id === id
                                );

                                if (index !== -1) {
                                    draft.posts.splice(index, 1);
                                }
                            }
                        )
                    );

                    dispatch(
                        UserFeedApi.util.updateQueryData(
                            'getFeed',
                            {
                                appUserId,
                                page: 1,
                                pageSize: 10,
                                feedVersion
                            },
                            draft => {
                                const index = draft.posts.findIndex(
                                    post => post.id === id
                                );

                                if (index !== -1) {
                                    draft.posts.splice(index, 1);
                                }
                            }
                        )
                    );
                } catch {
                    // DELETE failed
                }
            }
        }),
        countCommunityNewPosts: builder.query<number, { communityId: number, lastCheck: string }>({
            query: ({ communityId, lastCheck }) => `/CommunityPost/countNewPosts/${communityId}?lastCheck=${lastCheck}`,
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
    useCountCommunityNewPostsQuery,
    useGetCommunityPostByIdQuery,
    useLazyGetCommunityPostByIdQuery,
    useGetCommunityPostCountByCommunityIdQuery,
    useGetCommunityPostCountByListOfCommunityIdQuery,
    useLazyGetCommunityPostCountByListOfCommunityIdQuery,
} = CommunityPostApi;