import { faCloudArrowUp, faGear, faPaperPlane, faPen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFindGroupChatMessageByChatIdQuery } from '../../../store/api/ChatApi';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import {
    useCreateGroupChatMessageCountAsyncMutation, useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useCreateGroupChatMessageAsyncMutation, useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useCreateGroupChatUserAsyncMutation, useFindGroupChatUserByChatIdQuery,
    useFindGroupChatUserQuery
} from '../../../store/api/communication/chats/GroupChatUser.api';
import {
    useCreateUnreadGroupChatMessageAsyncMutation,
    useLazyFindUnreadGroupChatMessageQuery,
    useRemoveUnreadGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';
import AddPeople from '../../AddPeople';
import GroupChatMenu from './GroupChatMenu';
import GroupChatMessage from './GroupChatMessage';

import "../../../styles/communication/chats/groupChat.scss";

const getGroupChatMessagesInterval = 1000;
const messageType = {
    default: 0,
    system: 1
};

const GroupChat = ({ chat, me, setSelectedChat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState([]);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);
    const [userInformation, setUserInformation] = useState(null);
    const [editNameOn, setEditNameOn] = useState(false);
    const [myMessagesCount, setMyMessagesCount] = useState(null);

    const messageInput = useRef(null);
    const chatNameInput = useRef(null);

    const { data: messages, isLoading } = useFindGroupChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getGroupChatMessagesInterval
    });
    const { data: meInChat, isLoading: myUsersIsLoading } = useFindGroupChatUserQuery({ chatId: chat?.id, userId: me?.id });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUserByChatIdQuery(chat.id);
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [createGroupChatCountAsyncMut] = useCreateGroupChatMessageCountAsyncMutation();
    const [createUnreadGroupChatMessageAsyncMut] = useCreateUnreadGroupChatMessageAsyncMutation();
    const [removeUnreadGroupChatMessageAsyncMut] = useRemoveUnreadGroupChatMessageAsyncMutation();
    const [findUnreadGroupChatMessageQ] = useLazyFindUnreadGroupChatMessageQuery();
    const [getMyMessageCountAsync] = useLazyFindGroupChatMessageCountQuery();

    useEffect(() => {
        if (groupChatUsers === undefined) {
            return;
        }

        const customersId = [];
        for (let i = 0; i < groupChatUsers.length; i++) {
            customersId.push(groupChatUsers[i].customerId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatUsers])

    useEffect(() => {
        if (meInChat === undefined) {
            return;
        }

        const getMyMessageCount = async () => {
            const countObject = await getMyMessageCountAsync({ chatId: chat?.id, userId: meInChat?.id });
            if (countObject.data !== undefined) {
                setMyMessagesCount(countObject.data);
            }
        }

        getMyMessageCount();
    }, [meInChat])

    const sendMessageAsync = async () => {
        if (messageInput.current.value.length === 0) {
            return;
        }

        await createMessageAsync(messageInput.current.value, messageType["default"]);
        messageInput.current.value = "";
    }

    const sendMessageByKeyAsync = async (e) => {
        if (messageInput.current.value.length === 0
            || e.code !== "Enter") {
            return;
        }

        await createMessageAsync(messageInput.current.value, messageType["default"]);
        messageInput.current.value = "";
    }

    const createGroupChatUserAsync = async () => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newGroupChatUser = {
                id: " ",
                username: peopleToJoin[i].username,
                customerId: peopleToJoin[i].id,
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

        const createdMessagesCount = await createGroupChatCountAsyncMut(newMessagesCount);
        return createdMessagesCount.data !== undefined;
    }

    const createMessageAsync = async (message, type) => {
        const today = new Date();
        const newMessage = {
            message: message,
            when: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: type,
            groupChatId: chat.id,
            customerId: me?.id
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

    const updateGroupChatNameAsync = async () => {
        setEditNameOn(false);

        chat.name = chatNameInput.current.value;

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

    const updateMyChatMessagesCountAsync = async (count) => {
        const newMessagesCount = Object.assign({}, myMessagesCount);
        newMessagesCount.count = newMessagesCount.count + count;

        await updateGroupChatMessageCountMut(newMessagesCount);
    }

    const removeUnreadMessageAsync = async (message) => {
        const unreadMessage = await findUnreadGroupChatMessageQ({ messageId: message.id, groupChatUserId: meInChat?.id });
        if (unreadMessage.data !== undefined && unreadMessage.data !== null) {
            await removeUnreadGroupChatMessageAsyncMut(unreadMessage.data.id);
        }
    }

    const deleteMessageAsync = async (messageId) => {
        await removeGroupChatMessageAsync(messageId);
    }

    const handleUpdateGroupChatMessageAsync = async (message, count) => {
        const updated = await updateGroupChatMessageAsync(message);
        if (updated.data !== undefined && count !== 0) {
            await updateMyChatMessagesCountAsync(count);
            await removeUnreadMessageAsync(message);
        }
    }

    if (isLoading || usersIsLoading || myUsersIsLoading) {
        return <></>;
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="title__content">
                        {chat?.customerId === me?.id &&
                            <FontAwesomeIcon
                                icon={faPen}
                                title={t("EditName")}
                                className={`settings-handler${editNameOn ? "_active" : ""}`}
                                onClick={() => setEditNameOn((item) => !item)}
                            />
                        }
                        {editNameOn
                            ? <>
                                <input className="form-control" type="text" defaultValue={chat.name} ref={chatNameInput} />
                                <FontAwesomeIcon
                                    icon={faCloudArrowUp}
                                    title={t("Save")}
                                    className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                                    onClick={async () => await updateGroupChatNameAsync()}
                                />
                              </>
                            : <div className="name" title={chat.name}>{chat.name}</div>
                        }
                    </div>
                    <FontAwesomeIcon
                        icon={faGear}
                        title={t("Settings")}
                        className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                        onClick={() => setSettingsIsShow(!settingsIsShow)}
                    />
                </div>
                <ul className="chat-messages">
                    {
                        messages.map((item) => (
                            <li key={item.id}>
                                <GroupChatMessage
                                    me={me}
                                    meInChat={meInChat}
                                    message={item}
                                    updateMessageAsync={handleUpdateGroupChatMessageAsync}
                                    deleteMessageAsync={deleteMessageAsync}
                                />
                            </li>
                        ))
                    }
                </ul>
                <div className="form-group input-message">
                    <input type="text" className="form-control" placeholder={t("TypeYourMessage")}
                        ref={messageInput} onKeyDown={async (event) => await sendMessageByKeyAsync(event)} />
                    <FontAwesomeIcon
                        icon={faPaperPlane}
                        title={t("SendMessage")}
                        onClick={async () => await sendMessageAsync()}
                    />
                </div>
                {showAddPeople &&
                    <div className="add-people-to-chat">
                        <AddPeople
                            customer={me}
                            communityUsersId={groupChatUsersId}
                            peopleToJoin={peopleToJoin}
                            setPeopleToJoin={setPeopleToJoin}
                        />
                        <div className="item-result">
                            <input type="button" value={t("Invite")} className="btn btn-success" onClick={async () => await createGroupChatUserAsync()} />
                            <input type="button" value={t("Cancel")} className="btn btn-light" onClick={() => setShowAddPeople(false)} />
                        </div>
                    </div>
                }
            </div>
            {settingsIsShow &&
                <GroupChatMenu
                    me={me}
                    setUserInformation={setUserInformation}
                    setSelectedChat={setSelectedChat}
                    setShowAddPeople={setShowAddPeople}
                    groupChatUsers={groupChatUsers}
                    meInChat={meInChat}
                    chat={chat}
                />
            }
            {userInformation}
        </div>
    );
}

export default memo(GroupChat);