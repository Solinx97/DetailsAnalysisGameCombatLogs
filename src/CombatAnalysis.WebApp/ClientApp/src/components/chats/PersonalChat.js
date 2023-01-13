import { useEffect, useRef, useState } from "react";
import { useSelector } from 'react-redux';
import { faPaperPlane, faPen, faTrash, faCloudArrowUp, faPersonWalkingArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import "../../styles/chats/personalChat.scss";

const PersonalChat = ({ chat, setChatIsLeaft }) => {
    const chatMessageUpdatesInterval = 200;

    const user = useSelector((state) => state.user.value);

    const [chatMessages, setChatMessage] = useState(null);
    const [selectedMessageId, setSelectedMessageId] = useState(0);
    const [editModeIsOn, setEditMode] = useState(false);
    const [deleteModeIsOn, setDeleteMode] = useState(false);

    const messageInput = useRef(null);
    const editMessageInput = useRef(null);

    useEffect(() => {
        async function getChatMessages() {
            await getChatMessagesAsync();
        };

        let interval = setInterval(() => {
            getChatMessages();
        }, chatMessageUpdatesInterval);

        return () => {
            clearInterval(interval);
        };
    }, [chat])

    const getChatMessagesAsync = async () => {
        const response = await fetch(`/api/v1/PersonalChatMessage/findByChatId/${chat.id}`);
        const status = response.status;
        if (status === 200) {
            var messages = await response.json();

            setChatMessage(messages);
        }
    }

    const createMessage = (element) => {
        const isMyMessage = user.email === element.username;
        const elementIsSelected = element.id == selectedMessageId;

        return (<li key={element.id} className={`personal-chat-messages__${isMyMessage ? "right" : "left"}`}
            onClick={() => setSelectedMessageId(element.id)}>
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
                <FontAwesomeIcon icon={faTrash} title="Save" onClick={async () => await deleteMessageChatAsync(element.id)} />
            }
        </li>);
    }

    const updateMessageAsync = async (myMessage) => {
        setEditMode(false);
        myMessage.message = editMessageInput.current.value;

        await fetch("/api/v1/PersonalChatMessage", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(myMessage)
        });
    }

    const deleteMessageChatAsync = async (messageId) => {
        setDeleteMode(false);
        await fetch(`/api/v1/PersonalChatMessage/${messageId}`, {
            method: 'DELETE'
        });
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
            personalChatId: chat.id
        };

        const response = await fetch("/api/v1/PersonalChatMessage", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

        if (response.status == 200) {
            await updatePersonalChatAsync(message);
        }
    }

    const updatePersonalChatAsync = async (message) => {
        chat.lastMessage = message;

        await fetch("/api/v1/PersonalChat", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(chat)
        });
    }

    const leaveFromChatAsync = async () => {
        const response = await fetch(`/api/v1/PersonalChat/${chat.id}`, {
            method: 'DELETE'
        });

        const status = response.status;
        if (status === 200) {
            await fetch(`/api/v1/PersonalChatMessage/deleteByChatId/${chat.id}`, {
                method: 'DELETE'
            });

            setChatIsLeaft(true);
        }
    }

    const render = () => {
        return (
            <div className="chats__messages">
                <div className="title">
                    <div className="title__container">
                        <div className="title__companion">{chat.companionUsername}</div>
                        <FontAwesomeIcon icon={faPersonWalkingArrowRight} title="Remove chat" className="remove-chat-handler" onClick={() => leaveFromChatAsync()} />
                    </div>
                    <div className={`title__message-panel${selectedMessageId > 0 ? "-active" : ""}`}>
                        <FontAwesomeIcon icon={faPen} title="Edit" className={`edit-message-handler${editModeIsOn ? "-active" : ""}`} onClick={() => setEditMode(!editModeIsOn)} />
                        <FontAwesomeIcon icon={faTrash} title="Delete" className={`delete-message-handler${deleteModeIsOn ? "-active" : ""}`} onClick={() => setDeleteMode(!deleteModeIsOn)} />
                    </div>
                </div>
                <ul className="personal-chat-messages">
                    {chatMessages !== null &&
                        chatMessages.map((element) => createMessage(element))
                    }
                </ul>
                <div className="form-group chats__messages_input-message">
                    <input type="text" className="form-control" placeholder="Type your message" ref={messageInput} />
                    <FontAwesomeIcon icon={faPaperPlane} title="Send message" onClick={sendMessageAsync} />
                </div>
            </div>);
    }

    return render();
}

export default PersonalChat;