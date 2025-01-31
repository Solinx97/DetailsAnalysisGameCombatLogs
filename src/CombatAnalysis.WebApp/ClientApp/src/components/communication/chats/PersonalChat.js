import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useChatHub } from '../../../context/ChatHubProvider';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/chat/PersonalChatMessage.api';
import { useGetMessagesByPersonalChatIdQuery, useLazyGetMoreMessagesByPersonalChatIdQuery } from '../../../store/api/core/Chat.api';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import MessageInput from './MessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import "../../../styles/communication/chats/personalChat.scss";

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const { personalChatMessagesHubConnection, connectToPersonalChatMessagesAsync, subscribeToPersonalChatMessages, subscribeToPersonalMessageHasBeenRead } = useChatHub();

    const chatContainerRef = useRef(null);
    const pageSizeRef = useRef(process.env.REACT_APP_CHAT_PAGE_SIZE);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState(null);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({
        chatId: chat.id,
        pageSize: pageSizeRef.current
    });
    const [getMoreMessagesByPersonalChatIdAsync] = useLazyGetMoreMessagesByPersonalChatIdQuery();

    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [removePersonalChatMessageAsync] = useRemovePersonalChatMessageAsyncMutation();
    const [updateChatMessageAsync] = useUpdatePersonalChatMessageAsyncMutation();

    useEffect(() => {
        const connectToPersonalChatMessages = async () => {
            await connectToPersonalChatMessagesAsync(chat.id);
        }

        connectToPersonalChatMessages();
    }, []);

    useEffect(() => {
        if (!personalChatMessagesHubConnection) {
            return;
        }

        subscribeToPersonalChatMessages((message) => {
            setCurrentMessages(prevMessages => [...prevMessages, message]);
        });
    }, [personalChatMessagesHubConnection]);

    useEffect(() => {
        if (!messages) {
            return;
        }

        setCurrentMessages(messages);
    }, [messages]);

    useEffect(() => {
        if (!messages) {
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
        if (!currentMessages || messagesIsLoaded) {
            return;
        }

        scrollToBottom();

        setMessagesIsLoaded(true);
    }, [currentMessages]);

    useEffect(() => {
        if (!currentMessages || areLoadingOldMessages) {
            return;
        }

        scrollToBottom();
    }, [currentMessages]);

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
        if (chatContainer !== null) {
            chatContainer.scrollTop = chatContainer.scrollHeight;
        }
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
                                    meInChatId={me.id}
                                    reviewerId={me.id}
                                    messageOwnerId={message.appUserId}
                                    message={message}
                                    updateChatMessageAsync={updateChatMessageAsync}
                                    deleteMessageAsync={deleteMessageAsync}
                                    chatMessagesHubConnection={personalChatMessagesHubConnection}
                                    subscribeToMessageHasBeenRead={subscribeToPersonalMessageHasBeenRead}
                                />
                            </li>
                    ))}
                </ul>
                <MessageInput
                    chat={chat}
                    meInChat={me}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    targetChatType={0}
                    t={t}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);