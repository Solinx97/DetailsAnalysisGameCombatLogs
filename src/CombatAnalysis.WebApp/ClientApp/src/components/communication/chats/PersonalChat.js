import { faPaperPlane, faPersonWalkingArrowRight } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef } from "react";
import { useTranslation } from 'react-i18next';
import { useFindPersonalChatMessageByChatIdQuery } from '../../../store/api/ChatApi';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import { useRemovePersonalChatAsyncMutation, useUpdatePersonalChatAsyncMutation } from '../../../store/api/PersonalChat.api';
import {
    useCreatePersonalChatMessageAsyncMutation, useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/PersonalChatMessage.api';
import ChatMessage from './ChatMessage';

import "../../../styles/communication/personalChat.scss";

const getPersonalChatMessagesInterval = 1000;

const PersonalChat = ({ chat, customer, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const messageInput = useRef(null);

    const { data: messages, isLoading } = useFindPersonalChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getPersonalChatMessagesInterval
    });
    const { data: user, isLoading: userIsLoading } = useGetCustomerByIdQuery(companionId);
    const [createPersonalChatMessageAsync] = useCreatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
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
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            personalChatId: chat.id,
            ownerId: customer?.id
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
        const deletedItem = await removePersonalChatAsync(chat.id);
        if (deletedItem.data !== undefined) {
            setSelectedChat(null);
        }
    }

    if (isLoading || userIsLoading) {
        return <></>;
    }

    return (
        <div className="chats__messages">
            <div className="title">
                <div className="title__container">
                    <div className="title__companion">{user.username}</div>
                    <FontAwesomeIcon
                        icon={faPersonWalkingArrowRight}
                        title={t("LeaveFromChat")}
                        className="remove-chat-handler"
                        onClick={() => leaveFromChatAsync()}
                    />
                </div>
            </div>
            <ul className="chat-messages">
                {
                    messages?.map((item) => (
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
            <div className="form-group chats__messages_input-message">
                <input type="text" className="form-control" placeholder={t("TypeYourMessage")} ref={messageInput} />
                <FontAwesomeIcon
                    icon={faPaperPlane}
                    title={t("SendMessage")}
                    onClick={sendMessageAsync}
                />
            </div>
        </div>
    );
}

export default PersonalChat;