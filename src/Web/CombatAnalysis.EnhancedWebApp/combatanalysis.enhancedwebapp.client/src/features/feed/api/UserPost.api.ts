import type { UserPostModel } from '../types/UserPostModel';
import { PostApi } from './Post.api';
import { UserFeedApi } from './UserFeed.api';

export const UserPostApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        createUserPost: builder.mutation<UserPostModel, UserPostModel>({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'POST'
            }),
            async onQueryStarted(_post, { dispatch, queryFulfilled }) {
                try {
                    const { data: createdPost } = await queryFulfilled;

                    dispatch(
                        PostApi.util.updateQueryData(
                            'getUserPostsByUserId',
                            {
                                appUserId: createdPost.appUserId!,
                                page: 1,
                                pageSize: 10
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

                    dispatch(
                        UserFeedApi.util.updateQueryData(
                            'getFeed',
                            {
                                appUserId: createdPost.appUserId!,
                                page: 1,
                                pageSize: 10
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
        updateUserPost: builder.mutation<void, UserPostModel>({
            query: post => ({
                body: post,
                url: '/UserPost',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, post) => [{ type: 'UserPost', id: post.id }],
        }),
        removeUserPost: builder.mutation<void, number>({
            query: id => ({
                url: `/UserPost/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'UserPost', id }]
        }),
        getUserPostById: builder.query<UserPostModel, number>({
            query: id => `/UserPost/${id}`,
            providesTags: result => result ? [{ type: 'UserPost', id: result.id }] : [],
        }),
        getUserPostCountByUserId: builder.query<number, string>({
            query: appUserId => `/UserPost/count/${appUserId}`,
        }),
        getUserPostCountByListOfUserId: builder.query<number, string>({
            query: collectionUserId => `/UserPost/countByListOfUserId/${collectionUserId}`,
        }),
    })
})

export const {
    useCreateUserPostMutation,
    useUpdateUserPostMutation,
    useRemoveUserPostMutation,
    useGetUserPostCountByUserIdQuery,
    useGetUserPostCountByListOfUserIdQuery,
    useLazyGetUserPostCountByListOfUserIdQuery,
    useGetUserPostByIdQuery,
    useLazyGetUserPostByIdQuery,
} = UserPostApi;