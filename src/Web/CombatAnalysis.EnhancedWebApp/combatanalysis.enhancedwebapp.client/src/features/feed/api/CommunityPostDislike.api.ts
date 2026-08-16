import { checkStatus } from '@/shared/helpers/ApiHelper';
import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const CommunityPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityPostDislike: builder.mutation<CommunityPostReactionModel, { feedVersion: number, reaction: CommunityPostReactionModel }>({
            query: ({ reaction }) => ({
                body: reaction,
                url: '/CommunityPostDislike',
                method: 'POST'
            }),
            async onQueryStarted({ feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdDislike } = await queryFulfilled;

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId: createdDislike.communityId!,
                                appUserId: createdDislike.appUserId!,
                                page: 1,
                                pageSize: 10,
                                feedVersion
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
                                pageSize: 10,
                                feedVersion
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