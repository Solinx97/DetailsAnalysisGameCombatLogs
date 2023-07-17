import { faCloudArrowUp, faPaperPlane, faPen, faPersonWalkingArrowRight, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useRef, useState } from "react";
import { useSelector } from 'react-redux';
import PersonalChatMessageService from '../../../services/PersonalChatMessageService';
import PersonalChatService from '../../../services/PersonalChatService';

import "../../../styles/communication/personalChat.scss";

const PersonalChat = ({ chat, setChatIsLeaft }) => {
    const chatMessageUpdatesInterval = 200;

    const personalChatMessageService = new PersonalChatMessageService();
    const personalChatService = new PersonalChatService();

    const customer = useSelector((state) => state.customer.value);

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
        const messages = await personalChatMessageService.findByChatIdAsync(chat.id);
        if (messages !== null) {
            setChatMessage(messages);
        }
    }

    const createMessage = (message) => {
        const isMyMessage = customer.username === message.username;
        const elementIsSelected = message.id === selectedMessageId;

        return (
            <li key={message.id} className={`personal-chat-messages__${isMyMessage ? "right" : "left"}`}
                onClick={() => setSelectedMessageId(message.id)}>
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
                    <FontAwesomeIcon icon={faTrash} title="Save" onClick={async () => await deleteMessageChatAsync(message.id)} />
                }
            </li>
        );
    }

    const updateMessageAsync = async (myMessage) => {
        setEditMode(false);
        myMessage.message = editMessageInput.current.value;

        await personalChatMessageService.updateAsync(myMessage);
    }

    const deleteMessageChatAsync = async (messageId) => {
        setDeleteMode(false);
        await personalChatMessageService.deleteAsync(messageId);
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
            personalChatId: chat.id
        };

        const createdMessage = await personalChatMessageService.createAsync(newMessage);

        if (createdMessage.status !== null) {
            await updatePersonalChatAsync(message);
        }
    }

    const updatePersonalChatAsync = async (message) => {
        chat.lastMessage = message;

        await personalChatService.updateAsync(chat);
    }

    const leaveFromChatAsync = async () => {
        const deleteItem = await personalChatService.deleteAsync(chat.id);
        if (deleteItem !== null) {
            await personalChatMessageService.deleteByChatIdAsync(chat.id);

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
                        <FontAwesomeIcon icon={faPen} title="Edit" className={`edit-message-handler${editModeIsOn ? "-active" : ""}`} onClick={() => setEditMode((item) => !item)} />
                        <FontAwesomeIcon icon={faTrash} title="Delete" className={`delete-message-handler${deleteModeIsOn ? "-active" : ""}`} onClick={() => setDeleteMode((item) => !item)} />
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
            </div>
        );
    }

    return render();
}

export default PersonalChat;