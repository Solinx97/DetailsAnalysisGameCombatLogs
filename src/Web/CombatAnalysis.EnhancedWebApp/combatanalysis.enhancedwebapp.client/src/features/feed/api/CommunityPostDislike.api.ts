import type { CommunityPostModel } from '../types/CommunityPostModel';
import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import type { UserFeedModel } from '../types/UserFeedModel';
import { PostApi, ReactionType } from './Post.api';
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

                    const checkStatus = (post: CommunityPostModel | UserFeedModel) => {
                        switch (createdDislike.status) {
                            case ReactionType.Like:
                                post.likeCount++;
                                post.dislikeCount = Math.max(
                                    0,
                                    post.dislikeCount - 1
                                );
                                break;
                            case ReactionType.Dislike:
                                post.dislikeCount++;
                                post.likeCount = Math.max(
                                    0,
                                    post.likeCount - 1
                                );
                                break;
                            case ReactionType.AddLike:
                                post.likeCount++;
                                break;
                            case ReactionType.RemoveLike:
                                post.likeCount--;
                                break;
                            case ReactionType.AddDislike:
                                post.dislikeCount++;
                                break;
                            case ReactionType.RemoveDislike:
                                post.dislikeCount--;
                                break;
                            default:
                                break;
                        }
                    }

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getCommunityPostsByCommunityId',
                            {
                                communityId: createdDislike.communityId!,
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

                                checkStatus(post);
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

                                checkStatus(post);
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