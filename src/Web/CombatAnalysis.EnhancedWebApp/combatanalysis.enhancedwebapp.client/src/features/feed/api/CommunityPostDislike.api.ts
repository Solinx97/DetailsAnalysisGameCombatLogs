import { checkStatus } from '@/shared/helpers/ApiHelper';
import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const CommunityPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostDislike: builder.mutation<CommunityPostReactionModel, CommunityPostReactionModel>({
            query: communityPostDislike => ({
                body: communityPostDislike,
                url: '/CommunityPostDislike',
                method: 'POST'
            }),
            async onQueryStarted(_like, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdDislike } = await queryFulfilled;

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId: createdDislike.communityId!,
                                appUserId: createdDislike.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdDislike.communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                checkStatus(createdDislike, post);
                            }
                        )
                    );

                    dispatch(
                        UserFeedApi.util.updateQueryData(
                            'getFeed',
                            {
                                appUserId: createdDislike.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdDislike.communityPostId
                                );

                                if (!post) {
                                    return;
                                }

                                checkStatus(createdDislike, post);
                            }
                        )
                    );
                } catch {
                    // creation failed
                }
            },
        }),
        countCommunityPostDislikeByPostId: builder.query<number, number>({
            query: communityPostId => `/CommunityPostDislike/count/${communityPostId}`,
            providesTags: () => [
                { type: 'CommunityPostDislike', id: 'LIST' },
                { type: 'CommunityPostLike', id: 'LIST' }
            ]
        }),
    })
})

export const {
    useCreateCommunityPostDislikeMutation,
    useCountCommunityPostDislikeByPostIdQuery,
} = CommunityPostDislikeApi;