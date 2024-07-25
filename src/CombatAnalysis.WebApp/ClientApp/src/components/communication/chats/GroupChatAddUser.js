import { useState } from 'react';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import {
    useCreateGroupChatMessageCountAsyncMutation, useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useCreateGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useCreateGroupChatUserAsyncMutation
} from '../../../store/api/communication/chats/GroupChatUser.api';
import {
    useCreateUnreadGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';
import AddPeople from '../../AddPeople';

const GroupChatAddUser = ({ chat, me, groupChatUsersId, groupChatUsers, messageType, setShowAddPeople, t }) => {
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [createGroupChatCountAsyncMut] = useCreateGroupChatMessageCountAsyncMutation();
    const [createUnreadGroupChatMessageAsyncMut] = useCreateUnreadGroupChatMessageAsyncMutation();

    const [peopleToJoin, setPeopleToJoin] = useState([]);

    const createGroupChatUserAsync = async () => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newGroupChatUser = {
                id: " ",
                username: peopleToJoin[i].username,
                appUserId: peopleToJoin[i].id,
                groupChatId: chat.id,
            };

            const created = await createGroupChatUserMutAsync(newGroupChatUser);
            if (created.data !== undefined) {
                await createGroupChatCountAsync(chat.id, created.data.id);

                const systemMessage = `'${me?.username}' added '${peopleToJoin[i].username}' to chat`;
                await createMessageAsync(systemMessage, messageType["system"]);
            }
        }

        setPeopleToJoin([]);
        setShowAddPeople(false);
    }

    const createGroupChatCountAsync = async (chatId, customerId) => {
        const newMessagesCount = {
            count: 0,
            groupChatUserId: customerId,
            groupChatId: +chatId,
        };

        await createGroupChatCountAsyncMut(newMessagesCount);
    }

    const createMessageAsync = async (message, type) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: type,
            groupChatId: chat.id,
            appUserId: me?.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined && type !== messageType["system"]) {
            await messageSentSuccessfulAsync(createdMessage.data);
        }
    }

    const messageSentSuccessfulAsync = async (createdMessage) => {
        await updateGroupChatLastMessageAsync(createdMessage.message);

        const updateForMessage = Object.assign({}, createdMessage);
        updateForMessage.status = 1;

        await updateGroupChatMessageAsync(updateForMessage);

        const increaseUnreadMessages = 1;
        await updateGroupChatMessagesCountAsync(increaseUnreadMessages);

        await createUnreadMessageAsync(createdMessage.id);
    }

    const updateGroupChatLastMessageAsync = async (message) => {
        chat.lastMessage = message;

        await updateGroupChatAsyncMut(chat);
    }

    const updateGroupChatMessagesCountAsync = async (count) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === me?.id) {
                continue;
            }

            const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: groupChatUsers[i].id });
            if (messagesCount.data !== undefined) {
                const newMessagesCount = Object.assign({}, messagesCount.data);
                newMessagesCount.count = newMessagesCount.count + count;

                await updateGroupChatMessageCountMut(newMessagesCount);
            }
        }
    }

    const createUnreadMessageAsync = async (messageId) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === me?.id) {
                continue;
            }

            const newUnreadMessage = {
                groupChatUserId: groupChatUsers[i].id,
                groupChatMessageId: messageId
            }

            await createUnreadGroupChatMessageAsyncMut(newUnreadMessage);
        }
    }

    return (
        <div className="add-people-to-chat box-shadow">
            <AddPeople
                customer={me}
                communityUsersId={groupChatUsersId}
                peopleToJoin={peopleToJoin}
                setPeopleToJoin={setPeopleToJoin}
            />
            <div className="item-result">
                <div className="btn-border-shadow invite" onClick={async () => await createGroupChatUserAsync()}>{t("Invite")}</div>
                <div className="btn-border-shadow" onClick={() => setShowAddPeople(false)}>{t("Close")}</div>
            </div>
        </div>
    );
}

export default GroupChatAddUser;