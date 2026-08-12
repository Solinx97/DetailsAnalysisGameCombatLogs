import type { UserFeedModel } from '../types/UserFeedModel';
import { PostApi } from './Post.api';

export const UserFeedApi = PostApi.injectEndpoints({
    endpoints: builder => ({
        getFeed: builder.query<UserFeedModel[], { appUserId: string, page: number, pageSize: number }>({
            query: ({ appUserId, page, pageSize }) => `/UserFeed/${appUserId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.appUserId}`,
            merge: (currentCache, newItems) => {
                newItems.forEach(item => {
                    const index = currentCache.findIndex(d => d.id === item.id);
                    if (index === -1) {
                        currentCache.push(item);
                    } else {
                        currentCache[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => currentArg?.page !== previousArg?.page,
            providesTags: result =>
                result
                    ? [
                        ...result.map(feed => ({ type: 'UserFeed' as const, id: feed.id })),
                        { type: 'UserFeed', id: 'LIST' },
                    ]
                    : [{ type: 'UserFeed', id: 'LIST' }]
        }),
    })
})

export const {
    useGetFeedQuery,
} = UserFeedApi;