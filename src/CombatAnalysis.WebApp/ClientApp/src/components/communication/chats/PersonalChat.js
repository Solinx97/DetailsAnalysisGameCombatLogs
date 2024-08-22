import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/Account.api';
import { useGetMessagesByPersonalChatIdQuery } from '../../../store/api/ChatApi';
import {
    useLazyFindPersonalChatMessageCountQuery,
    useUpdatePersonalChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/PersonalChatMessagCount.api';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/communication/chats/PersonalChatMessage.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import PersonalChatMessageInput from './PersonalChatMessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import "../../../styles/communication/chats/personalChat.scss";

const getPersonalChatMessagesInterval = 500;

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const pageSize = 10;

    const { t } = useTranslation("communication/chats/personalChat");

    const chatContainerRef = useRef(null);
    const messagePageRef = useRef(1);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState([]);

    const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({
        chatId: chat.id,
        pageSize
    }, {
        pollingInterval: getPersonalChatMessagesInterval,
        refetchOnMountOrArgChange: true
    });
    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [updateChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageCountMut] = useUpdatePersonalChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindPersonalChatMessageCountQuery();

    useEffect(() => {
        if (!messages || messages.length === 0) {
            return;
        }

        saveScrollState();
        //scrollToBottom();

        const handleScroll = () => {
            if (chatContainerRef.current.scrollTop === 0) {
                const moreMessagesCount = count - (messagePageRef.current * 10);

                setHaveMoreMessage(moreMessagesCount > 0);
            }
        }

        const scrollContainer = chatContainerRef.current;
        scrollContainer.addEventListener("scroll", handleScroll);

        return () => {
            scrollContainer.removeEventListener("scroll", handleScroll);
        };
    }, [messages]);

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

    const saveScrollState = () => {
        const chatContainer = chatContainerRef.current;
        const previousScrollHeight = chatContainer.scrollHeight;
        const previousScrollTop = chatContainer.scrollTop;

        setCurrentMessages(prevMessages => [...messages, ...prevMessages]);

        setTimeout(() => {
            chatContainer.scrollTop = chatContainer.scrollHeight - previousScrollHeight + previousScrollTop;
        }, 0);
    }

    const scrollToBottom = () => {
        const chatContainer = chatContainerRef.current;
        chatContainer.scrollTop = chatContainer.scrollHeight;
    }

    if (isLoading || companionIsLoading || countIsLoading) {
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
                    <PersonalChatTitle
                        chat={chat}
                        setSelectedChat={setSelectedChat}
                        haveMoreMessages={haveMoreMessages}
                        setHaveMoreMessage={setHaveMoreMessage}
                        messagePageRef={messagePageRef}
                        t={t}
                    />
                </div>
                <ul className="chat-messages" ref={chatContainerRef}>
                    {currentMessages?.map((message) => (
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
        </div>
    );
}

export default PersonalChat;