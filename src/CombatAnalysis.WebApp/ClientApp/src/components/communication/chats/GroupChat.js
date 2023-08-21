import { faGear, faPaperPlane, faMinus, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useFindGroupChatMessageByChatIdQuery, useGetGroupChatUserByUserIdQuery } from '../../../store/api/ChatApi';
import { useRemoveGroupChatAsyncMutation, useUpdateGroupChatAsyncMutation } from '../../../store/api/GroupChat.api';
import {
    useCreateGroupChatMessageAsyncMutation, useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/GroupChatMessage.api';
import { useCreateGroupChatUserAsyncMutation, useGetGroupChatUserByChatIdQuery, useRemoveGroupChatUserAsyncMutation } from '../../../store/api/GroupChatUser.api';
import AddPeople from '../../AddPeople';
import ChatMessage from './ChatMessage';
import GroupChatUser from './GroupChatUser';

import "../../../styles/communication/chats/groupChat.scss";

const getGroupChatMessagesInterval = 1000;

const GroupChat = ({ chat, customer, setSelectedChat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [peopleIdToJoin, setPeopleToJoin] = useState([]);
    const [peopleIdToRemove, setPeopleToRemove] = useState([]);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);

    const messageInput = useRef(null);

    const { data: messages, isLoading } = useFindGroupChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getGroupChatMessagesInterval
    });
    const { data: muGroupChatUsers } = useGetGroupChatUserByUserIdQuery(customer?.id);
    const { data: groupChatUsers, isLoading: usersIsLoading } = useGetGroupChatUserByChatIdQuery(chat.id);
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [removeGroupChatAsyncMut] = useRemoveGroupChatAsyncMutation();
    const [removeGroupChatUserAsyncMut] = useRemoveGroupChatUserAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();

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

    const getChatUserByMyIdAsync = async () => {
        const currentGroupChatUser = muGroupChatUsers?.filter((chatUser) => chatUser.groupChatId === chat.id)[0];
        await leaveFromChatAsync(currentGroupChatUser.id);
    }

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
        for (var i = 0; i < peopleIdToJoin.length; i++) {
            const newGroupChatUser = {
                userId: peopleIdToJoin[i],
                groupChatId: chat.id,
            };

            await createGroupChatUserMutAsync(newGroupChatUser);
        }

        setShowAddPeople(false);
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
            ownerId: customer?.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined) {
            await updateGroupChatAsync(message);
        }
    }

    const updateGroupChatAsync = async (message) => {
        chat.lastMessage = message;

        await updateGroupChatAsyncMut(chat);
    }

    const updateMessageAsync = async (myMessage, newMessageContent) => {
        myMessage.message = newMessageContent;

        await updateGroupChatMessageAsync(myMessage);
    }

    const deleteMessageAsync = async (messageId) => {
        await removeGroupChatMessageAsync(messageId);
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

    if (isLoading || usersIsLoading) {
        return <></>;
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="title__companion">{chat.name}</div>
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
                                <ChatMessage
                                    customer={customer}
                                    message={item}
                                    updateMessageAsync={updateMessageAsync}
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
                            customer={customer}
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
                        {customer?.id === chat.ownerId &&
                            <input type="button" value={t("RemoveChat")} className="btn btn-danger" onClick={async () => await removeChatAsync()} />
                        }
                        <input type="button" value={t("Leave")} className="btn btn-warning" onClick={async () => await getChatUserByMyIdAsync()} />
                    </div>
                </div>
                <div className={`settings__people-inspection${peopleInspectionModeOn ? "_active" : ""}`}>
                    <div>{t("Members")}</div>
                    <ul>
                        {
                            groupChatUsers.map((item) => (
                                <li key={item.id}>
                                    <GroupChatUser
                                        userId={item.userId}
                                    />
                                    {(customer?.id === chat.ownerId && item.userId !== chat.ownerId)
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
        </div>
    );
}

export default memo(GroupChat);