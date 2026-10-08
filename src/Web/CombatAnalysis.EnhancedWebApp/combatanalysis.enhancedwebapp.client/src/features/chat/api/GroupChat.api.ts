import type { CreateGroupChatModel } from '../types/CreateGroupChatModel';
import type { GroupChatModel } from '../types/GroupChatModel';
import type { GroupChatPatch } from '../types/patches/GroupChatPatch';
import { ChatApi } from './Chat.api';

export const GroupChatApi = ChatApi.injectEndpoints({
    endpoints: builder => ({
        createGroupChat: builder.mutation<void, CreateGroupChatModel>({
            query: groupChat => ({
                body: groupChat,
                url: '/GroupChat',
                method: 'POST'
            }),
        }),
        partialUpdateGroupChat: builder.mutation<void, { id: number, groupChat: GroupChatPatch }>({
            query: ({ id, groupChat }) => ({
                body: groupChat,
                url: `/GroupChat/${id}`,
                method: 'PATCH'
            }),
            invalidatesTags: (_result, _error, args) => [{ type: 'GroupChat', id: args.id }],
        }),
        removeGroupChat: builder.mutation<void, number>({
            query: id => ({
                url: `/GroupChat/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: () => [{ type: 'GroupChatUser' }]
        }),
        getGroupChatById: builder.query<GroupChatModel, number>({
            query: id => `/GroupChat/${id}`,
            providesTags: result => result ? [{ type: 'GroupChat', id: result.id }] : [],
        }),
    })
})

export const {
    useCreateGroupChatMutation,
    usePartialUpdateGroupChatMutation,
    useRemoveGroupChatMutation,
    useGetGroupChatByIdQuery,
    useLazyGetGroupChatByIdQuery,
} = GroupChatApi;