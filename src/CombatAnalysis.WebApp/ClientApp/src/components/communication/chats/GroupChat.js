import { faFileWaveform, faFolderOpen, faGear, faPaperPlane, faPerson, faRightFromBracket, faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useRef, useState } from 'react';
import { useFindGroupChatMessageByChatIdQuery, useGetGroupChatUserByUserIdQuery } from '../../../store/api/ChatApi';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/GroupChat.api';
import {
    useCreateGroupChatMessageAsyncMutation, useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/GroupChatMessage.api';
import { useCreateGroupChatUserAsyncMutation, useGetGroupChatUserByChatIdQuery, useRemoveGroupChatUserAsyncMutation } from '../../../store/api/GroupChatUser.api';
import AddPeople from '../../AddPeople';
import ChatMessageItem from './ChatMessageItem';
import GroupChatUser from './GroupChatUser';

import "../../../styles/communication/groupChat.scss";

const getGroupChatMessagesInterval = 1000;

const GroupChat = ({ chat, customer, setChatIsLeaft }) => {
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);

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
    const [removeGroupChatUserAsync] = useRemoveGroupChatUserAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();

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

    const createGroupChatUserAsync = async (whomId) => {
        const newGroupChatUser = {
            userId: whomId,
            groupChatId: chat.id,
        };

        await createGroupChatUserMutAsync(newGroupChatUser);
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const newMessage = {
            userName: customer.username,
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            groupChatId: chat.id
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
        const deletedItem = await removeGroupChatUserAsync(id);
        if (deletedItem.data !== undefined) {
            setChatIsLeaft(true);
        }
    }

    if (isLoading || usersIsLoading) {
        return <></>;
    }

    return (
        <div className="chats__messages">
            <div className="title">
                <div className="title__container">
                    <div className="title__companion">{chat.name}</div>
                    <FontAwesomeIcon
                        icon={faGear} title="Settings"
                        className={`settings-handler${settingsIsShow ? "-active" : ""}`}
                        onClick={() => setSettingsIsShow(!settingsIsShow)}
                    />
                </div>
            </div>
            <div className={`settings${settingsIsShow ? "-active" : ""}`}>
                <div>
                    <FontAwesomeIcon
                        icon={faPerson}
                        title="Group members"
                        className={`people-inspection-handler${peopleInspectionModeOn ? "-active" : ""}`}
                        onClick={() => setPeopleInspectionMode((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faUserPlus}
                        title="Invite a new person"
                        className={`add-new-people-handler${showAddPeople ? "-active" : ""}`}
                        onClick={() => setShowAddPeople((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faFileWaveform}
                        title="Show description"
                    />
                    <FontAwesomeIcon
                        icon={faFolderOpen}
                        title="Show documents"
                    />
                    <FontAwesomeIcon
                        icon={faRightFromBracket}
                        title="Leave from chat"
                        className="leave-from-chat"
                        onClick={getChatUserByMyIdAsync}
                    />
                </div>
                <ul className={`people-inspection${peopleInspectionModeOn ? "-active" : ""}`}>
                    {
                        groupChatUsers.map((item) => (
                            <li key={item.id}>
                                <GroupChatUser
                                    userId={item.userId}
                                />
                            </li>
                        ))
                    }
                </ul>
            </div>
            <ul className="chat-messages">
                {
                    messages.map((item) => (
                        <li key={item.id}>
                            <ChatMessageItem
                                customer={customer}
                                message={item}
                                updateMessageAsync={updateMessageAsync}
                                deleteMessageAsync={deleteMessageAsync}
                            />
                        </li>
                    ))
                }
            </ul>
            <div className="form-group chats__messages_input-message">
                <input type="text" className="form-control" placeholder="Type your message" ref={messageInput} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title="Send message"
                    onClick={async () => await sendMessageAsync()}
                />
            </div>
            {showAddPeople &&
                <AddPeople
                    customer={customer}
                    communityUsersId={groupChatUsers}
                    setShowAddPeople={setShowAddPeople}
                    createInviteAsync={createGroupChatUserAsync}
                />
            }
        </div>
    );
}

export default memo(GroupChat);