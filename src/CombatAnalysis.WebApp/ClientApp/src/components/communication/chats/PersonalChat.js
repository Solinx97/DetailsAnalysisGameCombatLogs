import { faCloudArrowUp, faPaperPlane, faPen, faPersonWalkingArrowRight, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState } from "react";
import { useFindPersonalChatMessageByChatIdQuery } from '../../../store/api/ChatApi';
import { useRemovePersonalChatAsyncMutation, useUpdatePersonalChatAsyncMutation } from '../../../store/api/PersonalChat.api';
import {
    useCreatePersonalChatMessageAsyncMutation, useRemovePersonalChatMessageAsyncMutation,
    useRemovePersonalChatMessageByChatIdAsyncMutation, useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/PersonalChatMessage.api';

import "../../../styles/communication/personalChat.scss";

const PersonalChat = ({ chat, customer, setChatIsLeaft }) => {
    const [selectedMessageId, setSelectedMessageId] = useState(0);
    const [editModeIsOn, setEditMode] = useState(false);
    const [deleteModeIsOn, setDeleteMode] = useState(false);

    const messageInput = useRef(null);
    const editMessageInput = useRef(null);

    const { data: messages, isLoading} = useFindPersonalChatMessageByChatIdQuery(chat.id);
    const [createPersonalChatMessageAsync] = useCreatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [removePersonalChatMessageByChatIdAsync] = useRemovePersonalChatMessageByChatIdAsyncMutation();
    const [removePersonalChatAsync] = useRemovePersonalChatAsyncMutation();
    const [updatePersonalChatAsyncMut] = useUpdatePersonalChatAsyncMutation();

    const updateMessageAsync = async (myMessage) => {
        setEditMode(false);
        myMessage.message = editMessageInput.current.value;

        await updatePersonalChatMessageAsync(myMessage);
    }

    const deleteMessageChatAsync = async (messageId) => {
        setDeleteMode(false);

        await removePersonalChatMessageAsync(messageId);
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

        const createdMessage = await createPersonalChatMessageAsync(newMessage);

        if (createdMessage.data !== undefined) {
            await updatePersonalChatAsync(message);
        }
    }

    const updatePersonalChatAsync = async (message) => {
        chat.lastMessage = message;

        await updatePersonalChatAsyncMut(chat);
    }

    const leaveFromChatAsync = async () => {
        const deleteItem = await removePersonalChatAsync(chat.id);
        if (deleteItem.data !== undefined) {
            await removePersonalChatMessageByChatIdAsync(chat.id);

            setChatIsLeaft(true);
        }
    }

    if (isLoading) {
        return <></>;
    }

    return (
        <div className="chats__messages">
            <div className="title">
                <div className="title__container">
                    <div className="title__companion">{chat.companionUsername}</div>
                    <FontAwesomeIcon
                        icon={faPersonWalkingArrowRight}
                        title="Remove chat"
                        className="remove-chat-handler"
                        onClick={() => leaveFromChatAsync()}
                    />
                </div>
                <div className={`title__message-panel${selectedMessageId > 0 ? "-active" : ""}`}>
                    <FontAwesomeIcon
                        icon={faPen} title="Edit"
                        className={`edit-message-handler${editModeIsOn ? "-active" : ""}`}
                        onClick={() => setEditMode((item) => !item)}
                    />
                    <FontAwesomeIcon
                        icon={faTrash}
                        title="Delete"
                        className={`delete-message-handler${deleteModeIsOn ? "-active" : ""}`}
                        onClick={() => setDeleteMode((item) => !item)}
                    />
                </div>
            </div>
            <ul className="personal-chat-messages">
                {
                    messages.map((item) => (
                        <li key={item.id} className={`personal-chat-messages__${customer.username === item.username ? "right" : "left"}`}
                            onClick={() => setSelectedMessageId(item.id)}>
                            {customer?.username !== item.username &&
                                <div className="username">{item.username}</div>
                            }
                            {editModeIsOn && customer.username === item.username && item.id === selectedMessageId
                                ? <div className="edit-message">
                                    <input className="form-control" defaultValue={item.message} ref={editMessageInput} />
                                    <FontAwesomeIcon
                                        icon={faCloudArrowUp}
                                        title="Save"
                                        onClick={async () => await updateMessageAsync(item)}
                                    />
                                </div>
                                : <div className="message">{item.message}</div>
                            }
                            {deleteModeIsOn && customer.username === item.username && item.id === selectedMessageId &&
                                <FontAwesomeIcon
                                    icon={faTrash}
                                    title="Save"
                                    onClick={async () => await deleteMessageChatAsync(item.id)}
                                />
                            }
                        </li>
                    ))
                }
            </ul>
            <div className="form-group chats__messages_input-message">
                <input type="text" className="form-control" placeholder="Type your message" ref={messageInput} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title="Send message"
                    onClick={sendMessageAsync}
                />
            </div>
        </div>
    );
}

export default PersonalChat;