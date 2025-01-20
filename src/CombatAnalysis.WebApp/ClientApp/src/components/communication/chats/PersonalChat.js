import * as signalR from '@microsoft/signalr';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
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

const PersonalChat = ({ chat, me, setSelectedChat, companionId, unreadMessageHubConnection }) => {
    const { t } = useTranslation("communication/chats/personalChat");

    const hubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_ADDRESS}`;

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

    useEffect(() => {
        const connectToChat = async () => {
            await connectToChatHubAsync();
        }

        connectToChat();
    }, []);

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

    useEffect(() => {
        return () => {
            if (hubConnection) {
                const disconnectFromChat = async () => {
                    await hubConnection.invoke("LeaveFromRoom", chat?.id);
                    await hubConnection.stop();
                }

                disconnectFromChat();
            }
        }
    }, [hubConnection]);

    const connectToChatHubAsync = async () => {
        if (hubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(hubURL)
                .withAutomaticReconnect()
                .build();
            setHubConnection(hubConnection);

            await hubConnection.start();

            await hubConnection.invoke("JoinRoom", chat.id);

            hubConnection.on("ReceiveMessage", (message) => {
                setCurrentMessages(prevMessages => [...prevMessages, message]);
            });
        } catch (e) {
            console.error(e);
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
                                    reviewerId={me.id}
                                    messageOwnerId={message.appUserId}
                                    message={message}
                                    updateChatMessageAsync={updateChatMessageAsync}
                                    deleteMessageAsync={deleteMessageAsync}
                                    hubConnection={hubConnection}
                                    unreadMessageHubConnection={unreadMessageHubConnection}
                                />
                            </li>
                    ))}
                </ul>
                <MessageInput
                    hubConnection={hubConnection}
                    unreadMessageHubConnection={unreadMessageHubConnection}
                    chat={chat}
                    meInChat={me}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    t={t}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);