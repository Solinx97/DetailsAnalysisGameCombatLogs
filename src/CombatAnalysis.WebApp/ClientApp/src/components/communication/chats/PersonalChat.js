import { faPaperPlane, faPersonWalkingArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef } from "react";
import { useFindPersonalChatMessageByChatIdQuery } from '../../../store/api/ChatApi';
import { useRemovePersonalChatAsyncMutation, useUpdatePersonalChatAsyncMutation } from '../../../store/api/PersonalChat.api';
import {
    useCreatePersonalChatMessageAsyncMutation, useRemovePersonalChatMessageAsyncMutation,
    useRemovePersonalChatMessageByChatIdAsyncMutation, useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/PersonalChatMessage.api';
import ChatMessageItem from './ChatMessageItem';

import "../../../styles/communication/personalChat.scss";

const getPersonalChatMessagesInterval = 1000;

const PersonalChat = ({ chat, customer, setChatIsLeaft }) => {
    const messageInput = useRef(null);

    const { data: messages, isLoading } = useFindPersonalChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getPersonalChatMessagesInterval
    });
    const [createPersonalChatMessageAsync] = useCreatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [removePersonalChatMessageByChatIdAsync] = useRemovePersonalChatMessageByChatIdAsyncMutation();
    const [removePersonalChatAsync] = useRemovePersonalChatAsyncMutation();
    const [updatePersonalChatAsyncMut] = useUpdatePersonalChatAsyncMutation();

    const updateMessageAsync = async (myMessage, newMessageContent) => {
        myMessage.message = newMessageContent;

        await updatePersonalChatMessageAsync(myMessage);
    }

    const deleteMessageAsync = async (messageId) => {
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
                    onClick={sendMessageAsync}
                />
            </div>
        </div>
    );
}

export default PersonalChat;