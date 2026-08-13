import type { CommunityPostModel } from '../types/CommunityPostModel';
import type { CommunityPostReactionModel } from '../types/CommunityPostReactionModel';
import type { UserFeedModel } from '../types/UserFeedModel';
import { PostApi, ReactionType } from './Post.api';
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

                    const checkStatus = (post: CommunityPostModel | UserFeedModel) => {
                        switch (createdLike.status) {
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
                                communityId: createdLike.communityId!,
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

                                checkStatus(post);
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

                                checkStatus(post);
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