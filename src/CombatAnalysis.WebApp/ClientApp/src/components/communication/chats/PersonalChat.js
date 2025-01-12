import * as signalR from '@microsoft/signalr';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
    useLazyFindPersonalChatMessageCountQuery,
    useUpdatePersonalChatMessageCountAsyncMutation
} from '../../../store/api/chat/PersonalChatMessagCount.api';
import {
    useGetPersonalChatMessageCountByChatIdQuery,
    useRemovePersonalChatMessageAsyncMutation,
    useUpdatePersonalChatMessageAsyncMutation
} from '../../../store/api/chat/PersonalChatMessage.api';
import { useGetMessagesByPersonalChatIdQuery, useLazyGetMoreMessagesByPersonalChatIdQuery } from '../../../store/api/core/Chat.api';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import PersonalChatMessageInput from './PersonalChatMessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import "../../../styles/communication/chats/personalChat.scss";

const PersonalChat = ({ chat, me, setSelectedChat, companionId }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const hubURL = "https://localhost:7026/personalChatHub";

    const chatContainerRef = useRef(null);
    const pageSizeRef = useRef(process.env.REACT_APP_CHAT_PAGE_SIZE);

    const [hubConnection, setHubConnection] = useState(null);
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
    const [updatePersonalChatMessageCountMut] = useUpdatePersonalChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindPersonalChatMessageCountQuery();

    useEffect(() => {
        if (!messages || messages.length === 0) {
            return;
        }

        setCurrentMessages(messages);
    }, [messages]);

    useEffect(() => {
        if (!currentMessages) {
            return;
        }

        const connectToChat = async () => {
            await connectToChatAsync();
        }

        connectToChat();
    }, [currentMessages]);

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

    useEffect(() => {
        return () => {
            const disconnectFromChat = async () => {
                if (hubConnection) {
                    await hubConnection.invoke("LeaveFromRoom", `${chat.id}`);
                    await hubConnection.stop();
                }
            }

            disconnectFromChat();
        }
    }, [hubConnection]);

    const connectToChatAsync = async () => {
        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(hubURL)
                .withAutomaticReconnect()
                .build();
            setHubConnection(hubConnection);

            await hubConnection.start();

            await hubConnection.invoke("JoinRoom", `${chat.id}`);

            await hubConnection.on("ReceiveMessage", (message) => {
                const messages = Array.from(currentMessages);
                messages.push(message);

                setCurrentMessages(messages);
            });
        } catch (e) {
            console.log(e);
        }
    }

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

export default memo(PersonalChat);