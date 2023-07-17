import { faCloudArrowUp, faFileWaveform, faFolderOpen, faGear, faPaperPlane, faPen, faPerson, faRightFromBracket, faRightToBracket, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import AccountService from '../../../services/AccountService';
import GroupChatMessageService from '../../../services/GroupChatMessageService';
import GroupChatService from '../../../services/GroupChatService';
import GroupChatUserService from '../../../services/GroupChatUserService';

import "../../../styles/communication/groupChat.scss";

const GroupChat = ({ chat, setChatIsLeaft }) => {
    const chatMessageUpdateInterval = 200;

    const groupChatMessageService = new GroupChatMessageService();
    const groupChatUserService = new GroupChatUserService();
    const accountService = new AccountService();
    const groupChatService = new GroupChatService();

    const customer = useSelector((state) => state.customer.value);

    const [chatMessages, setChatMessage] = useState(null);
    const [groupChatUsers, setGroupChatUsers] = useState(null);
    const [selectedMessageId, setSelectedMessageId] = useState(0);
    const [editModeIsOn, setEditMode] = useState(false);
    const [deleteModeIsOn, setDeleteMode] = useState(false);
    const [peopleInspectionModeOn, setPeopleInspectionMode] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);

    const messageInput = useRef(null);
    const editMessageInput = useRef(null);

    useEffect(() => {
        async function getChatMessages() {
            await getChatMessagesAsync();
        };

        let interval = setInterval(() => {
            getChatMessages();
        }, chatMessageUpdateInterval);

        return () => {
            clearInterval(interval);
        };
    }, [chat])

    const getChatMessagesAsync = async () => {
        const messages = await groupChatMessageService.findByChatIdAsync(chat.id);
        if (messages !== null) {
            setChatMessage(messages);
        }
    }

    const getChatUsersAsync = async () => {
        const users = [];

        const groupChatUsers = await groupChatUserService.findByChatIdAsync(chat.id);
        if (groupChatUsers === null) {
            return;
        }

        for (let i = 0; i < groupChatUsers.length; i++) {
            const user = await getUserAsync(groupChatUsers[i].userId);
            users.push(user);
        }

        setGroupChatUsers(users);
    }

    const getChatUserByMyIdAsync = async () => {
        const myGroupChatUsers = await groupChatUserService.getByIdAsync(customer.id);
        if (myGroupChatUsers === null) {
            return;
        }

        const currentGroupChatUser = myGroupChatUsers.filter((chatUser) => chatUser.groupChatId === chat.id)[0];
        await leaveFromChatAsync(currentGroupChatUser.id);
    }

    const getUserAsync = async (id) => {
        const user = await accountService.getUserById(id);
        if (user === null) {
            return {
                id: -1,
                email: "undefined"
            };
        }

        return user;
    }

    const sendMessageAsync = async () => {
        if (messageInput.current.value.length === 0) {
            return;
        }

        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const newMessage = {
            userName: customer.username,
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            groupChatId: chat.id
        };

        const createdMessage = await groupChatMessageService.createAsync(newMessage);
        if (createdMessage !== null) {
            await updateGroupChatAsync(message);
        }
    }

    const updateGroupChatAsync = async (message) => {
        chat.lastMessage = message;

        await groupChatService.updateAsync(chat);
    }

    const updateMessageAsync = async (myMessage) => {
        setEditMode(false);
        myMessage.message = editMessageInput.current.value;

        await groupChatMessageService.updateAsync(myMessage);
    }

    const deleteMessageAsync = async (messageId) => {
        setDeleteMode(false);
        await groupChatMessageService.deleteAsync(messageId);
    }

    const leaveFromChatAsync = async (id) => {
        const deletedItem = await groupChatUserService.deleteAsync(id);

        if (deletedItem !== null) {
            setChatIsLeaft(true);
        }
    }

    const createMessage = (message) => {
        const isMyMessage = customer.username === message.username;
        const elementIsSelected = message.id === selectedMessageId;

        return (
            <li key={message.id} className={`group-chat-messages__${isMyMessage ? "right" : "left"}`} onClick={() => setSelectedMessageId(message.id)}>
                {!isMyMessage &&
                    <div className="username">{message.username}</div>
                }
                {editModeIsOn && isMyMessage && elementIsSelected
                    ? <div className="edit-message">
                        <input className="form-control" defaultValue={message.message} ref={editMessageInput} />
                        <FontAwesomeIcon icon={faCloudArrowUp} title="Save" onClick={async () => await updateMessageAsync(message)} />
                    </div>
                    : <div className="message">{message.message}</div>
                }
                {deleteModeIsOn && isMyMessage && elementIsSelected &&
                    <FontAwesomeIcon icon={faTrash} title="Save" onClick={async () => await deleteMessageAsync(message.id)} />
                }
            </li>
        );
    }

    const createGroupChatUser = (chatUser) => {
        return (<li key={chatUser.id}>{chatUser.email}</li>);
    }

    const peopleInspectionHandler = async () => {
        setPeopleInspectionMode(!peopleInspectionModeOn);
        await getChatUsersAsync();
    }

    const render = () => {
        return (
            <div className="chats__messages">
                <div className="title">
                    <div className="title__container">
                        <div className="title__companion">{chat.name}</div>
                        <FontAwesomeIcon icon={faGear} title="Settings" className={`settings-handler${settingsIsShow ? "-active" : ""}`} onClick={() => setSettingsIsShow(!settingsIsShow)} />
                    </div>
                    <div className={`title__message-panel${selectedMessageId > 0 ? "-active" : ""}`}>
                        <FontAwesomeIcon icon={faPen} title="Edit" className={`edit-message-handler${editModeIsOn ? "-active" : ""}`} onClick={() => setEditMode(!editModeIsOn)} />
                        <FontAwesomeIcon icon={faTrash} title="Delete" className={`delete-message-handler${deleteModeIsOn ? "-active" : ""}`} onClick={() => setDeleteMode(!deleteModeIsOn)} />
                    </div>
                </div>
                <div className={`settings${settingsIsShow ? "-active" : ""}`}>
                    <div>
                        <FontAwesomeIcon icon={faPerson} title="People inspection" className={`people-inspection-handler${peopleInspectionModeOn ? "-active" : ""}`} onClick={peopleInspectionHandler} />
                        <FontAwesomeIcon icon={faRightToBracket} title="Invite a new person" onClick={() => setEditMode(!editModeIsOn)} />
                        <FontAwesomeIcon icon={faFileWaveform} title="Show description" onClick={() => setEditMode(!editModeIsOn)} />
                        <FontAwesomeIcon icon={faFolderOpen} title="Show documents" onClick={() => setEditMode(!editModeIsOn)} />
                        <FontAwesomeIcon icon={faRightFromBracket} title="Leave from chat" className="leave-from-chat" onClick={getChatUserByMyIdAsync} />
                    </div>
                    <ul className={`people-inspection${peopleInspectionModeOn ? "-active" : ""}`}>
                        {groupChatUsers !== null &&
                            groupChatUsers.map((element) => createGroupChatUser(element))
                        }
                    </ul>
                </div>
                <ul className="group-chat-messages">
                    {chatMessages !== null &&
                        chatMessages.map((element) => createMessage(element))
                    }
                </ul>
                <div className="form-group chats__messages_input-message">
                    <input type="text" className="form-control" placeholder="Type your message" ref={messageInput} />
                    <FontAwesomeIcon icon={faPaperPlane} title="Send message" onClick={async () => await sendMessageAsync()} />
                </div>
            </div>
        );
    }

    return render();
}

export default memo(GroupChat);