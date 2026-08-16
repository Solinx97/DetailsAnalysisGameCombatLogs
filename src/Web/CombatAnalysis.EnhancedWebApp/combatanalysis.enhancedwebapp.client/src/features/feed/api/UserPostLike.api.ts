import { checkStatus } from '@/shared/helpers/ApiHelper';
import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const UserPostLikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostLike: builder.mutation<UserPostReactionModel, { feedVersion: number, reaction: UserPostReactionModel }>({
            query: ({ reaction }) => ({
                body: reaction,
                url: '/UserPostLike',
                method: 'POST'
            }),
            async onQueryStarted({ feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdLike } = await queryFulfilled;

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
                                pageSize: 10,
                                feedVersion
                            },
                            draft => {
                                const post = draft.posts.find(
                                    x => x.id === createdLike.userPostId
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