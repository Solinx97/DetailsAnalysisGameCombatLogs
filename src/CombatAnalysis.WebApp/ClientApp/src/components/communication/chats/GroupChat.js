import * as signalR from '@microsoft/signalr';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import useGroupChatData from '../../../hooks/useGroupChatData';
import {
    useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/chat/GroupChatMessage.api';
import Loading from '../../Loading';
import GroupChatMessage from './GroupChatMessage';
import GroupChatAddUser from './GroupChatAddUser';
import GroupChatMenu from './GroupChatMenu';
import GroupChatTitle from './GroupChatTitle';
import MessageInput from './MessageInput';

import '../../../styles/communication/chats/groupChat.scss';

const messageType = {
    default: 0,
    system: 1
};

const GroupChat = ({ chat, me, setSelectedChat, unreadMessageHubConnection }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const hubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_ADDRESS}`;

    const [hubConnection, setHubConnection] = useState(null);
    const [showAddPeople, setShowAddPeople] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);
    const [userInformation, setUserInformation] = useState(null);

    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState(null);
    const [messagesIsLoaded, setMessagesIsLoaded] = useState(false);
    const [areLoadingOldMessages, setAreLoadingOldMessages] = useState(true);

    const chatContainerRef = useRef(null);
    const pageSizeRef = useRef(process.env.REACT_APP_CHAT_PAGE_SIZE);

    const { groupChatData, getMoreMessagesAsync } = useGroupChatData(chat.id, me.id, pageSizeRef);

    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();

    useEffect(() => {
        const connectToChat = async () => {
            await connectToChatAsync();
        }

        connectToChat();
    }, []);

    useEffect(() => {
        if (!groupChatData.messages) {
            return;
        }

        setCurrentMessages(groupChatData.messages);
    }, [groupChatData.messages]);

    useEffect(() => {
        if (!groupChatData.messages) {
            return;
        }

        const handleScroll = () => {
            const chatContainer = chatContainerRef.current;
            if (chatContainer.scrollTop === 0) {
                const moreMessagesCount = groupChatData.count - currentMessages.length + groupChatData.messages.length - pageSizeRef.current;

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
    }, [currentMessages, groupChatData.messages]);

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
        if (!groupChatData.groupChatUsers) {
            return;
        }

        const customersId = [];
        for (let i = 0; i < groupChatData.groupChatUsers.length; i++) {
            customersId.push(groupChatData.groupChatUsers[i].appUserId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatData.groupChatUsers]);

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
        await removeGroupChatMessageAsync(messageId);
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

    const handleLoadMoreMessagesAsync = async () => {
        setAreLoadingOldMessages(true);

        const moreMessages = await getMoreMessagesAsync(currentMessages.length);

        setCurrentMessages(prevMessages => [...moreMessages, ...prevMessages]);

        saveScrollState();
    }

    if (groupChatData.isLoading) {
        return (
            <div className="chats__selected-chat_loading">
                <Loading />
            </div>
        );
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <GroupChatTitle
                    chat={chat}
                    me={me}
                    settingsIsShow={settingsIsShow}
                    setSettingsIsShow={setSettingsIsShow}
                    haveMoreMessages={haveMoreMessages}
                    setHaveMoreMessage={setHaveMoreMessage}
                    loadMoreMessagesAsync={handleLoadMoreMessagesAsync}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {currentMessages?.map((message) => (
                        <li className="message" key={message.id}>
                            <GroupChatMessage
                                me={me}
                                meInChat={groupChatData.meInChat.id}
                                messageOwnerId={message.groupChatUserId}
                                message={message}
                                updateChatMessageAsync={updateGroupChatMessageAsync}
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
                    meInChat={groupChatData.meInChat}
                    setAreLoadingOldMessages={setAreLoadingOldMessages}
                    t={t}
                />
                {showAddPeople &&
                    <GroupChatAddUser
                        chat={chat}
                        me={me}
                        groupChatUsersId={groupChatUsersId}
                        setShowAddPeople={setShowAddPeople}
                        groupChatUsers={groupChatData.groupChatUsers}
                        messageType={messageType}
                        t={t}
                    />
                }
            </div>
            {settingsIsShow &&
                <GroupChatMenu
                    me={me}
                    setUserInformation={setUserInformation}
                    setSelectedChat={setSelectedChat}
                    setShowAddPeople={setShowAddPeople}
                    groupChatUsers={groupChatData.groupChatUsers}
                    meInChat={groupChatData.meInChat}
                    chat={chat}
                    t={t}
                />
            }
            {userInformation}
        </div>
    );
}

export default memo(GroupChat);