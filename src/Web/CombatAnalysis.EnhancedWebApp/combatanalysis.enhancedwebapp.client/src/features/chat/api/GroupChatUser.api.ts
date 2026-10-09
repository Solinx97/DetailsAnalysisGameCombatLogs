import type { CreateGroupChatUserModel } from '../types/CreateGroupChatUserModel';
import type { GroupChatUserModel } from '../types/GroupChatUserModel';
import { ChatApi } from './Chat.api';

export const GroupChatUserApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChatUserAsync: builder.mutation<void, CreateGroupChatUserModel>({
            query: groupChatUser => ({
                body: groupChatUser,
                url: '/GroupChatUser',
                method: 'POST'
            }),
        }),
        removeGroupChatUserAsync: builder.mutation<void, string>({
            query: id => ({
                url: `/GroupChatUser/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [{ type: 'GroupChatUser', id }],
        }),
        getGroupChatUserById: builder.query<GroupChatUserModel, string>({
            query: id => `/GroupChatUser/${id}`,
            providesTags: result => result ? [{ type: 'GroupChatUser', id: result.id }] : [],
        }),
        findChatUser: builder.query<GroupChatUserModel, { chatId: number, appUserId: string }>({
            query: ({ chatId, appUserId }) => `/GroupChatUser/findChatUser?chatId=${chatId}&appUserId=${appUserId}`,
        }),
        findAllChatUsers: builder.query<GroupChatUserModel[], number>({
            query: chatId => `/GroupChatUser/findAllChatUsers/${chatId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(chatUser => ({ type: 'GroupChatUser' as const, id: chatUser.id })),
                        { type: 'GroupChatUser', id: 'LIST' },
                    ]
                    : [{ type: 'GroupChatUser', id: 'LIST' }],
        }),
        findChatUsers: builder.query<GroupChatUserModel[], string>({
            query: appUserId => `/GroupChatUser/findChatUsers/${appUserId}`,
            providesTags: result =>
                result
                    ? [
                        ...result.map(chatUser => ({ type: 'GroupChatUser' as const, id: chatUser.id })),
                        { type: 'GroupChatUser', id: 'LIST' },
                    ]
                    : [{ type: 'GroupChatUser', id: 'LIST' }],
        }),
    })
})

export const {
    useCreateGroupChatUserAsyncMutation,
    useRemoveGroupChatUserAsyncMutation,
    useGetGroupChatUserByIdQuery,
    useFindChatUserQuery,
    useFindChatUsersQuery,
    useLazyFindChatUsersQuery,
    useFindAllChatUsersQuery,
} = GroupChatUserApi;