import type { UserFeedModel } from '../types/UserFeedModel';
import type { UserPostModel } from '../types/UserPostModel';
import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi, ReactionType } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const UserPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostDislike: builder.mutation<UserPostReactionModel, UserPostReactionModel>({
            query: userPostDislike => ({
                body: userPostDislike,
                url: '/UserPostDislike',
                method: 'POST'
            }),
            async onQueryStarted(_like, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdDislike } = await queryFulfilled;

                    const checkStatus = (post: UserPostModel | UserFeedModel) => {
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
                            'getUserPostsByUserId',
                            {
                                appUserId: createdDislike.appUserId!,
                                page: 1,
                                pageSize: 10
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdDislike.userPostId
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
                                    x => x.id === createdDislike.userPostId
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
        countUserPostDislikeByPostId: builder.query<number, number>({
            query: id => `/UserPostDislike/count/${id}`,
            providesTags: () => [
                { type: 'UserPostDislike', id: 'LIST' },
                { type: 'UserPostLike', id: 'LIST' }
            ]
        }),
    })
})

export const {
    useCreateUserPostDislikeMutation,
    useCountUserPostDislikeByPostIdQuery,
} = UserPostDislikeApi;