import { checkStatus } from '@/shared/helpers/ApiHelper';
import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const CommunityPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostLike: builder.mutation<CommunityPostReactionModel, CommunityPostReactionModel>({
            query: communityPostLike => ({
                body: communityPostLike,
                url: '/CommunityPostLike',
                method: 'POST'
            }),
            async onQueryStarted(_like, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdLike } = await queryFulfilled;

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId: createdLike.communityId!,
                                appUserId: createdLike.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdLike.communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                checkStatus(createdLike, post);
                            }
                        )
                    );

                    dispatch(
                        UserFeedApi.util.updateQueryData(
                            'getFeed',
                            {
                                appUserId: createdLike.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdLike.communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                checkStatus(createdLike, post);
                            }
                        )
                    );
                } catch {
                    // creation failed
                }
            },
        }),
        countCommunityPostLikeByPostId: builder.query<number, number>({
            query: communityPostId => `/CommunityPostLike/count/${communityPostId}`,
            providesTags: () => [
                { type: 'CommunityPostDislike', id: 'LIST' },
                { type: 'CommunityPostLike', id: 'LIST' }
            ]
        }),
    })
})

export const {
    useCreateCommunityPostLikeMutation,
    useCountCommunityPostLikeByPostIdQuery,
} = CommunityPostLikeApi;