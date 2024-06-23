import { useTranslation } from 'react-i18next';
import { useGetCustomerByIdQuery } from '../../../store/api/Customer.api';
import {
    useFindPersonalChatMessageByChatIdQuery, useRemovePersonalChatMessageAsyncMutation
} from '../../../store/api/communication/chats/PersonalChatMessage.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import ChatRemoveNotification from './ChatRemoveNotification';
import MessageInput from './MessageInput';

import "../../../styles/communication/chats/personalChat.scss";

const getPersonalChatMessagesInterval = 1000;

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const { data: messages, isLoading } = useFindPersonalChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getPersonalChatMessagesInterval
    });
    const { data: companion, isLoading: companionIsLoading } = useGetCustomerByIdQuery(companionId);
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();

    const deleteMessageAsync = async (messageId) => {
        await removePersonalChatMessageAsync(messageId);
    }

    if (isLoading || companionIsLoading) {
        return (<Loading />);
    }

    return (
        <div className="chats__selected-chat personal-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="name">{companion.username}</div>
                </div>
                <ul className="chat-messages">
                    {
                        messages?.map((item) => (
                            <li key={item.id}>
                                <ChatMessage
                                    me={me}
                                    message={item}
                                    messageStatus={item.status}
                                    deleteMessageAsync={deleteMessageAsync}
                                />
                            </li>
                        ))
                    }
                </ul>
                <MessageInput
                    chat={chat}
                    meId={me?.id}
                    companionId={companion?.id}
                    t={t}
                />
            </div>
            <ChatRemoveNotification
                chat={chat}
                setSelectedChat={setSelectedChat}
                t={t}
            />
        </div>
    );
}

export default PersonalChat;