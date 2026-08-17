import type { AllCommunityUserModel } from '../types/AllCommunityUserModel';
import type { CommunityUserModel } from '../types/CommunityUserModel';
import { CommunityApi } from './Community.api';

export const CommunityUserApi = CommunityApi.injectEndpoints({
    endpoints: builder => ({
        createCommunityUser: builder.mutation<CommunityUserModel, CommunityUserModel>({
            query: (communityUser) => ({
                body: communityUser,
                url: '/CommunityUser',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'CommunityUser', id: result.id }] : [],
        }),
        removeCommunityUser: builder.mutation<void, { id: string, communityId: number }>({
            query: ({ id, communityId }) => ({
                url: `/CommunityUser/${id}?communityId=${communityId}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'CommunityUser', id: args.id }]
        }),
        leaveCommunityUser: builder.mutation<void, { appUserId: string, communityId: number }>({
            query: ({ appUserId, communityId }) => ({
                url: `/CommunityUser/leave?appUserId=${appUserId}&communityId=${communityId}`,
                method: 'DELETE'
            }),
        }),
        getUsersByCommunityId: builder.query<AllCommunityUserModel, { communityId: number, page: number, pageSize: number }>({
            query: ({ communityId, page, pageSize }) => `/CommunityUser/getByCommunityId/${communityId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.communityId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.users.length = 0;
                    currentCache.users.push(...newItems.users);
                    return;
                }

                newItems.users.forEach(item => {
                    const index = currentCache.users.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.users.push(item);
                    } else {
                        currentCache.users[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.communityId !== previousArg?.communityId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'CommunityUser', id: 'LIST' },
                ...(result?.users.map(post => ({
                    type: 'CommunityUser' as const,
                    id: post.id
                })) ?? [])
            ]
        }),        
        getShortListUsersByCommunityId: builder.query<AllCommunityUserModel, { communityId: number, pageSize: number }>({
            query: ({ communityId, pageSize }) => `/CommunityUser/getShortListByCommunityId/${communityId}?pageSize=${pageSize}`,
            providesTags: () => [{ type: 'CommunityUser', id: 'LIST'}],
        }),
        getCommunityUsersByUserId: builder.query<AllCommunityUserModel, { appUserId: string, page: number, pageSize: number }>({
            query: ({ appUserId, page, pageSize }) => `/CommunityUser/getByUserId/${appUserId}?page=${page}&pageSize=${pageSize}`,
            serializeQueryArgs: ({ endpointName, queryArgs }) => `${endpointName}-${queryArgs.appUserId}`,
            merge: (currentCache, newItems, { arg }) => {
                if (arg.page === 1) {
                    currentCache.users.length = 0;
                    currentCache.users.push(...newItems.users);
                    return;
                }

                newItems.users.forEach(item => {
                    const index = currentCache.users.findIndex(x => x.id === item.id);
                    if (index === -1) {
                        currentCache.users.push(item);
                    } else {
                        currentCache.users[index] = item;
                    }
                });
            },
            forceRefetch: ({ currentArg, previousArg }) => {
                return (
                    currentArg?.appUserId !== previousArg?.appUserId ||
                    currentArg?.page !== previousArg?.page ||
                    currentArg?.pageSize !== previousArg?.pageSize
                );
            },
            providesTags: result => [
                { type: 'CommunityUser', id: 'LIST' },
                ...(result?.users.map(post => ({
                    type: 'CommunityUser' as const,
                    id: post.id
                })) ?? [])
            ]
        }),
    })
})

export const {
    useGetUsersByCommunityIdQuery,
    useGetShortListUsersByCommunityIdQuery,
    useGetCommunityUsersByUserIdQuery,
    useCreateCommunityUserMutation,
    useRemoveCommunityUserMutation,
    useLeaveCommunityUserMutation
} = CommunityUserApi;