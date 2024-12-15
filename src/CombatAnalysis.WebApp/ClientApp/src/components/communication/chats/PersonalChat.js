import { useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import { useGetMessagesByPersonalChatIdQuery, useLazyGetMoreMessagesByPersonalChatIdQuery } from '../../../store/api/core/Chat.api';
import {
    useLazyFindPersonalChatMessageCountQuery,
    useUpdatePersonalChatMessageCountAsyncMutation
} from '../../../store/api/chat/PersonalChatMessagCount.api';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/chat/PersonalChatMessage.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import PersonalChatMessageInput from './PersonalChatMessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import "../../../styles/communication/chats/personalChat.scss";

const getPersonalChatMessagesInterval = 500;

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const chatContainerRef = useRef(null);
    const pageSizeRef = useRef(1);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState([]);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({
        chatId: chat.id,
        pageSize: pageSizeRef.current
    }, {
        pollingInterval: getPersonalChatMessagesInterval,
        refetchOnMountOrArgChange: true
    });
    const [getMoreMessagesByPersonalChatIdAsync] = useLazyGetMoreMessagesByPersonalChatIdQuery();

    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [updateChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();
    const [updatePersonalChatMessageCountMut] = useUpdatePersonalChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindPersonalChatMessageCountQuery();

    useEffect(() => {
        pageSizeRef.current = process.env.REACT_APP_CHAT_PAGE_SIZE;
    }, []);

    useEffect(() => {
        if (!messages || messages.length === 0) {
            return;
        }

        const newMessages = addUniqueElements(currentMessages, messages);
        setCurrentMessages(newMessages);
    }, [messages]);

    useEffect(() => {
        if (!messages || messages.length === 0) {
            return;
        }

        const handleScroll = () => {
            const chatContainer = chatContainerRef.current;
            if (chatContainer.scrollTop === 0) {
                const moreMessagesCount = count - currentMessages.length + messages.length - pageSizeRef.current;
                setHaveMoreMessage(moreMessagesCount > 0);
            }
            else if (chatContainer.scrollHeight - chatContainer.scrollTop === chatContainer.clientHeight) {
                setHaveMoreMessage(false);
            }
        }

        const scrollContainer = chatContainerRef.current;
        scrollContainer?.addEventListener("scroll", handleScroll);

        return () => {
            scrollContainer?.removeEventListener("scroll", handleScroll);
        }
    }, [currentMessages, messages]);

    useEffect(() => {
        if (currentMessages.length === 0 || messagesIsLoaded) {
            return;
        }

        scrollToBottom();

        setMessagesIsLoaded(true);
    }, [currentMessages]);

    useEffect(() => {
        if (currentMessages.length === 0 || areLoadingOldMessages) {
            return;
        }

        scrollToBottom();
    }, [currentMessages]);

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

        setTimeout(() => {
            chatContainer.scrollTop = chatContainer.scrollHeight - previousScrollHeight + previousScrollTop;
        }, 0);
    }

    const scrollToBottom = () => {
        const chatContainer = chatContainerRef.current;
        chatContainer.scrollTop = chatContainer.scrollHeight;
    }

    const addUniqueElements = (oldArray, newArray) => {
        const oldSet = new Set(oldArray.map(item => item.id));
        const uniqueNewElements = newArray.filter(item => !oldSet.has(item.id));
        const refreshedArray = oldArray.concat(uniqueNewElements);

        return refreshedArray;
    }

    const getMoreMessagesAsync = async (offset) => {
        const arg = {
            chatId: chat.id,
            offset,
            pageSize: pageSizeRef.current
        };

        const response = await getMoreMessagesByPersonalChatIdAsync(arg);
        if (response.data) {
            return response.data;
        }

        return [];
    }

    const handleLoadMoreMessagesAsync = async () => {
        setAreLoadingOldMessages(true);

        const moreMessages = await getMoreMessagesAsync(currentMessages.length);

        setCurrentMessages(prevMessages => [...moreMessages, ...prevMessages]);

        saveScrollState();
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
                <PersonalChatTitle
                    chat={chat}
                    companionUsername={companion.username}
                    setSelectedChat={setSelectedChat}
                    haveMoreMessages={haveMoreMessages}
                    setHaveMoreMessage={setHaveMoreMessage}
                    loadMoreMessagesAsync={handleLoadMoreMessagesAsync}
                    t={t}
                />
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
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    t={t}
                />
            </div>
        </div>
    );
}

export default PersonalChat;