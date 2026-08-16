import type { AllUserFeedModel } from '../types/AllUserFeedModel';
import { PostApi } from './Post.api';

export const UserFeedApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        countFeedNewPosts: builder.query<number, { appUserId: string, lastCheck: string }>({
            query: ({ appUserId, lastCheck }) => `/UserFeed/countNewPosts/${appUserId}?lastCheck=${lastCheck}`,
        }),
        getFeed: builder.query<AllUserFeedModel, { appUserId: string, page: number, pageSize: number, feedVersion: number }>({
            query: ({ appUserId, page, pageSize }) => `/UserFeed/${appUserId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.appUserId}-${queryArgs.feedVersion}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.posts.length = 0;
                    currentCache.posts.push(...newItems.posts);
                    return;
                }

                newItems.posts.forEach(item => {
                    const index = currentCache.posts.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.posts.push(item);
                    } else {
                        currentCache.posts[index] = item;
                    }
                });

                currentCache.posts.sort(
                    (a, b) =>
                        new Date(b.createdAt).getTime() -
                        new Date(a.createdAt).getTime()
                );
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.appUserId !== previousArg?.appUserId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize || 
                    currentArg?.feedVersion !== previousArg?.feedVersion
                );
            },
            providesTags: result => [
                { type: 'UserFeed', id: 'LIST' },
                ...(result?.posts.map(post => ({
                    type: 'UserFeed' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
    })
})

export const {
    useCountFeedNewPostsQuery,
    useGetFeedQuery,
} = UserFeedApi;