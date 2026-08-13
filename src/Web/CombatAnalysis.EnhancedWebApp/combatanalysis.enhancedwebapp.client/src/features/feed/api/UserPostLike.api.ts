import type { UserFeedModel } from '../types/UserFeedModel';
import type { UserPostModel } from '../types/UserPostModel';
import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi, ReactionType } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const UserPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostLike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostLike => ({
                body: userPostLike,
                url: '/UserPostLike',
                method: 'POST'
            }),
            async onQueryStarted(_like, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdLike } = await queryFulfilled;

                    const checkStatus = (post: UserPostModel | UserFeedModel) => {
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
                            'getUserPostsByUserId',
                            {
                                appUserId: createdLike.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdLike.userPostId
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
                                    x => x.id === createdLike.userPostId
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
        countUserPostLikeByPostId: builder.query<number, number>({
            query: id => `/UserPostLike/count/${id}`,
            providesTags: () => [
                { type: 'UserPostDislike', id: 'LIST' },
                { type: 'UserPostLike', id: 'LIST' }
            ]
        }),
    })
})

export const {
    useCreateUserPostLikeMutation,
    useCountUserPostLikeByPostIdQuery,
} = UserPostLikeApi;