import type { UserPostReactionModel } from '../types/UserPostReactionModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';
import { checkStatus } from '@/shared/helpers/ApiHelper';

export const UserPostDislikeApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPostDislike: builder.mutation<UserPostReactionModel, { feedVersion: number, reaction: UserPostReactionModel }>({
            query: ({ reaction }) => ({
                body: reaction,
                url: '/UserPostDislike',
                method: 'POST'
            }),
            async onQueryStarted({ feedVersion }, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdDislike } = await queryFulfilled;

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
                                    x => x.id === createdDislike.userPostId
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