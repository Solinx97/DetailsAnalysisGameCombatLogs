import { faGear, faMinus, faPaperPlane, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFindGroupChatMessageByChatIdQuery } from '../../../store/api/ChatApi';
import { useRemoveGroupChatAsyncMutation, useUpdateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import {
    useCreateGroupChatMessageCountAsyncMutation, useFindGroupChatMessageCountQuery,
    useLazyFindGroupChatMessageCountQuery, useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useCreateGroupChatMessageAsyncMutation, useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useCreateGroupChatUserAsyncMutation, useFindGroupChatUserByChatIdQuery,
    useFindGroupChatUserQuery, useRemoveGroupChatUserAsyncMutation
} from '../../../store/api/communication/chats/GroupChatUser.api';
import {
    useCreateUnreadGroupChatMessageAsyncMutation,
    useLazyFindUnreadGroupChatMessageQuery,
    useRemoveUnreadGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';
import AddPeople from '../../AddPeople';
import User from '../User';
import GroupChatMessage from './GroupChatMessage';

import "../../../styles/communication/chats/groupChat.scss";

const getGroupChatMessagesInterval = 1000;

const GroupChat = ({ chat, me, setSelectedChat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [peopleIdToJoin, setPeopleToJoin] = useState([]);
    const [peopleIdToRemove, setPeopleToRemove] = useState([]);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);
    const [userInformation, setUserInformation] = useState(null);

    const messageInput = useRef(null);

    const { data: messages, isLoading } = useFindGroupChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getGroupChatMessagesInterval
    });
    const { data: meInChat, isLoading: myUsersIsLoading } = useFindGroupChatUserQuery({ chatId: chat?.id, userId: me?.id });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUserByChatIdQuery(chat.id);
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [removeGroupChatAsyncMut] = useRemoveGroupChatAsyncMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [createGroupChatCountAsyncMut] = useCreateGroupChatMessageCountAsyncMutation();
    const [createUnreadGroupChatMessageAsyncMut] = useCreateUnreadGroupChatMessageAsyncMutation();
    const [removeUnreadGroupChatMessageAsyncMut] = useRemoveUnreadGroupChatMessageAsyncMutation();
    const [findUnreadGroupChatMessageQ] = useLazyFindUnreadGroupChatMessageQuery();
    const { data: myMessagesCount, isLoading: myMessagesCountLoading } = useFindGroupChatMessageCountQuery({ chatId: chat?.id, userId: me?.id });

    useEffect(() => {
        if (groupChatUsers === undefined) {
            return;
        }

        const usersId = [];
        for (var i = 0; i < groupChatUsers.length; i++) {
            usersId.push(groupChatUsers[i].userId);
        }

        setGroupChatUsersId(usersId);
    }, [groupChatUsers])

    const sendMessageAsync = async () => {
        if (messageInput.current.value.length === 0) {
            return;
        }

        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const sendMessageByKeyAsync = async (e) => {
        if (messageInput.current.value.length === 0
            || e.code !== "Enter") {
            return;
        }

        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const createGroupChatUserAsync = async () => {
        for (let i = 0; i < peopleIdToJoin.length; i++) {
            const newGroupChatUser = {
                id: "",
                userId: peopleIdToJoin[i],
                groupChatId: chat.id,
            };

            const created = await createGroupChatUserMutAsync(newGroupChatUser);
            if (created.data !== undefined) {
                await createGroupChatCountAsync(chat.id, peopleIdToJoin[i]);
            }
        }

        setShowAddPeople(false);
    }

    const createGroupChatCountAsync = async (chatId, userId) => {
        const newMessagesCount = {
            count: 0,
            userId: userId,
            groupChatId: +chatId,
        };

        const createdMessagesCount = await createGroupChatCountAsyncMut(newMessagesCount);
        return createdMessagesCount.data !== undefined;
    }

    const addPeopleForRemove = (id) => {
        const people = peopleIdToRemove;
        people.push(id);

        setPeopleToRemove(people);
    }

    const removePeopleForRemove = (id) => {
        const people = peopleIdToRemove.filter((item) => item !== id);

        setPeopleToRemove(people);
    }

    const removeGroupChatUserAsync = async () => {
        for (var i = 0; i < peopleIdToRemove.length; i++) {
            await removeGroupChatUserAsyncMut(peopleIdToRemove[i]);
        }

        setPeopleInspectionMode(false);
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            groupChatId: chat.id,
            ownerId: me?.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.error !== undefined) {
            return;
        }

        await updateGroupChatAsync(message);

        const updateForMessage = Object.assign({}, createdMessage.data);
        updateForMessage.status = 1;

        await updateGroupChatMessageAsync(updateForMessage);

        const increaseUnreadMessages = 1;
        await updateChatMessagesCountAsync(increaseUnreadMessages);

        await createUnreadMessageAsync(createdMessage.data.id);
    }

    const updateGroupChatAsync = async (message) => {
        chat.lastMessage = message;

        await updateGroupChatAsyncMut(chat);
    }

    const updateChatMessagesCountAsync = async (count) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].userId === me?.id) {
                continue;
            }

            const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: groupChatUsers[i].userId });
            const newMessagesCount = Object.assign({}, messagesCount.data);
            newMessagesCount.count = newMessagesCount.count + count;

            await updateGroupChatMessageCountMut(newMessagesCount);
        }
    }

    const createUnreadMessageAsync = async (messageId) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].userId === me?.id) {
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

    const leaveFromChatAsync = async (id) => {
        const deletedItem = await removeGroupChatUserAsyncMut(id);
        if (deletedItem.data !== undefined) {
            setSelectedChat(null);
        }
    }

    const removeChatAsync = async () => {
        const deletedItem = await removeGroupChatAsyncMut(chat.id);
        if (deletedItem.data !== undefined) {
            setSelectedChat(null);
        }
    }

    if (isLoading || usersIsLoading
        || myUsersIsLoading || myMessagesCountLoading) {
        return <></>;
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="name" title={chat.name}>{chat.name}</div>
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
                            peopleToJoin={peopleIdToJoin}
                            setPeopleToJoin={setPeopleToJoin}
                        />
                        <div className="item-result">
                            <input type="button" value={t("Invite")} className="btn btn-success" onClick={async () => await createGroupChatUserAsync()} />
                            <input type="button" value={t("Cancel")} className="btn btn-light" onClick={() => setShowAddPeople(false)} />
                        </div>
                    </div>
                }
            </div>
            <div className={`settings${settingsIsShow ? "_active" : ""}`}>
                <div className="settings__content">
                    <div className="main-settings">
                        <input type="button" value={t("Members")} className="btn btn-light" onClick={() => setPeopleInspectionMode((item) => !item)} />
                        <input type="button" value={t("Invite")} className="btn btn-light" onClick={() => setShowAddPeople((item) => !item)} />
                        <input type="button" value={t("Documents")} className="btn btn-light" disabled />
                    </div>
                    <div className="danger-settings">
                        {me?.id === chat.customerId &&
                            <input type="button" value={t("RemoveChat")} className="btn btn-danger" onClick={async () => await removeChatAsync()} />
                        }
                        <input type="button" value={t("Leave")} className="btn btn-warning" onClick={async () => await leaveFromChatAsync(meInChat?.id)} />
                    </div>
                </div>
                <div className={`settings__people-inspection${peopleInspectionModeOn ? "_active" : ""}`}>
                    <div>{t("Members")}</div>
                    <ul>
                        {
                            groupChatUsers.map((item) => (
                                <li key={item.id}>
                                    <User
                                        me={me}
                                        targetCustomerId={item.userId}
                                        setUserInformation={setUserInformation}
                                        allowRemoveFriend={false}
                                    />
                                    {(me?.id === chat.customerId && item.userId !== chat.customerId)
                                        ? peopleIdToRemove.includes(item.id)
                                            ? <FontAwesomeIcon
                                                icon={faRightFromBracket}
                                                title={t("RomeFromChat")}
                                                onClick={() => removePeopleForRemove(item.id)}
                                            />
                                            : <FontAwesomeIcon
                                                icon={faMinus}
                                                title={t("RomeFromChat")}
                                                onClick={() => addPeopleForRemove(item.id)}
                                            />
                                        : null
                                    }
                                </li>
                            ))
                        }
                    </ul>
                    <div className="item-result">
                        <input type="button" value={t("Accept")} className="btn btn-success" onClick={async () => await removeGroupChatUserAsync()} />
                        <input type="button" value={t("Close")} className="btn btn-secondary" onClick={() => setPeopleInspectionMode((item) => !item)} />
                    </div>

                </div>
            </div>
            {userInformation}
        </div>
    );
}

export default memo(GroupChat);