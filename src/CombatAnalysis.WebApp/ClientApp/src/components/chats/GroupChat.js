import { memo, useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { faPaperPlane, faPen, faTrash, faCloudArrowUp, faGear, faPerson, faRightToBracket, faRightFromBracket, faFileWaveform, faFolderOpen } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../styles/chats/groupChat.scss";

const GroupChat = ({ chat, setChatIsLeaft }) => {
    const chatMessageUpdateInterval = 200;

    const user = useSelector((state) => state.user.value);

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
        const response = await fetch(`/api/v1/GroupChatMessage/findByChatId/${chat.id}`);
        const status = response.status;
        if (status === 200) {
            const messages = await response.json();

            setChatMessage(messages);
        }
    }

    const getChatUsersAsync = async () => {
        const response = await fetch(`/api/v1/GroupChatUser/findByChatId/${chat.id}`);
        const status = response.status;
        if (status === 200) {
            const groupChatUsers = await response.json();
            let users = [];

            for (var i = 0; i < groupChatUsers.length; i++) {
                const user = await getUserAsync(groupChatUsers[i].userId);
                users.push(user);
            }

            setGroupChatUsers(users);
        }
    }

    const getChatUserByMyIdAsync = async () => {
        const response = await fetch(`/api/v1/GroupChatUser/${user.id}`);
        const status = response.status;
        if (status === 200) {
            const myGroupChatUsers = await response.json();
            const currentGroupChatUser = myGroupChatUsers.filter((chatUser) => chatUser.groupChatId === chat.id)[0];

            await leaveFromChatAsync(currentGroupChatUser.id);
        }
    }

    const getUserAsync = async (id) => {
        const response = await fetch(`/api/v1/Account/${id}`);
        const status = response.status;
        if (status === 200) {
            const user = await response.json();
            return user;
        }

        return {
            id: -1,
            email: "undefined"
        };
    }

    const sendMessageAsync = async () => {
        await createChatMessageAsync(messageInput.current.value);
        messageInput.current.value = "";
    }

    const createChatMessageAsync = async (message) => {
        const today = new Date();
        const data = {
            id: 0,
            userName: "temp@yandex.by",
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            groupChatId: chat.id
        };

        const response = await fetch("/api/v1/GroupChatMessage", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status == 200) {
            await updateGroupChatAsync(message);
        }
    }

    const updateGroupChatAsync = async (message) => {
        chat.lastMessage = message;

        await fetch("/api/v1/GroupChat", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(chat)
        });
    }

    const updateMessageAsync = async (myMessage) => {
        setEditMode(false);
        myMessage.message = editMessageInput.current.value;

        await fetch("/api/v1/GroupChatMessage", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(myMessage)
        });
    }

    const deleteMessageAsync = async (messageId) => {
        setDeleteMode(false);
        await fetch(`/api/v1/GroupChatMessage/${messageId}`, {
            method: 'DELETE'
        });
    }

    const leaveFromChatAsync = async (id) => {
        const response = await fetch(`/api/v1/GroupChatUser/${id}`, {
            method: 'DELETE'
        });

        const status = response.status;
        if (status === 200) {
            setChatIsLeaft(true);
        }
    }

    const createMessage = (element) => {
        const isMyMessage = user.email === element.username;
        const elementIsSelected = element.id == selectedMessageId;

        return (<li key={element.id} className={`group-chat-messages__${isMyMessage ? "right" : "left"}`} onClick={() => setSelectedMessageId(element.id)}>
            {!isMyMessage &&
                <div className="username">{element.username}</div>
            }
            {editModeIsOn && isMyMessage && elementIsSelected
                ? <div className="edit-message">
                    <input className="form-control" defaultValue={element.message} ref={editMessageInput} />
                    <FontAwesomeIcon icon={faCloudArrowUp} title="Save" onClick={async () => await updateMessageAsync(element)} />
                </div>
                : <div className="message">{element.message}</div>
            }
            {deleteModeIsOn && isMyMessage && elementIsSelected &&
                <FontAwesomeIcon icon={faTrash} title="Save" onClick={async () => await deleteMessageAsync(element.id)} />
            }
        </li>);
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
            </div>);
    }

    return render();
}

export default memo(GroupChat);