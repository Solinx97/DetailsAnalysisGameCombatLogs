import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/Account.api';
import {
    useLazyFindPersonalChatMessageCountQuery,
    useUpdatePersonalChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/PersonalChatMessagCount.api';
import {
    useFindPersonalChatMessageByChatIdQuery, useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/communication/chats/PersonalChatMessage.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import ChatRemoveNotification from './ChatRemoveNotification';
import PersonalChatMessageInput from './PersonalChatMessageInput';

import "../../../styles/communication/chats/personalChat.scss";

const getPersonalChatMessagesInterval = 1000;

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const { data: messages, isLoading } = useFindPersonalChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getPersonalChatMessagesInterval
    });
    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [updateChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageCountMut] = useUpdatePersonalChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindPersonalChatMessageCountQuery();

    const decreasePersonalChatMessagesCountAsync = async () => {
        const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: me.id });
        if (messagesCount.data !== undefined) {
            const unblockedObject = Object.assign({}, messagesCount.data);
            unblockedObject.count = --unblockedObject.count;

            await updatePersonalChatMessageCountMut(unblockedObject);
        }
    }

    const deleteMessageAsync = async (messageId) => {
        await removePersonalChatMessageAsync(messageId);
    }

    if (isLoading || companionIsLoading) {
        return (
            <div className="chats__selected-chat_loading">
                <Loading />
            </div>
        );
    }

    return (
        <div className="chats__selected-chat personal-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="name">{companion.username}</div>
                </div>
                <ul className="chat-messages">
                    {messages?.map((message) => (
                            <li key={message.id}>
                                <ChatMessage
                                    me={me}
                                    message={message}
                                    messageStatus={message.status}
                                    updateChatMessageAsync={updateChatMessageAsync}
                                    deleteMessageAsync={deleteMessageAsync}
                                    decreaseChatMessagesCountAsync={decreasePersonalChatMessagesCountAsync}
                                />
                            </li>
                    ))}
                </ul>
                <PersonalChatMessageInput
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