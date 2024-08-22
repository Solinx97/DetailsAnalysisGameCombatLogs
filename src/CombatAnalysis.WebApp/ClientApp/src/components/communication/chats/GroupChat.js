import { memo, useEffect, useState, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import useGroupChatData from '../../../hooks/useGroupChatData';
import {
    useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useRemoveGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import Loading from '../../Loading';
import ChatMessage from './ChatMessage';
import GroupChatAddUser from './GroupChatAddUser';
import GroupChatMenu from './GroupChatMenu';
import GroupChatMessageInput from './GroupChatMessageInput';
import GroupChatTitle from './GroupChatTitle';

import '../../../styles/communication/chats/groupChat.scss';

const messageType = {
    default: 0,
    system: 1
};

const GroupChat = ({ chat, me, setSelectedChat }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);
    const [userInformation, setUserInformation] = useState(null);
    const [haveMoreMessages, setHaveMoreMessage] = useState(false);
    const [currentMessages, setCurrentMessages] = useState([]);

    const chatContainerRef = useRef(null);
    const messagePageRef = useRef(1);

    const { messages, count, meInChat, groupChatUsers, isLoading } = useGroupChatData(chat.id, me.id, messagePageRef);

    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();

    useEffect(() => {
        if (messages.length === 0) {
            return;
        }

        saveScrollState();

        const newMessages = addUniqueElements(currentMessages, messages);
        setCurrentMessages(newMessages);

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

    useEffect(() => {
        if (!groupChatUsers) {
            return;
        }

        const customersId = [];
        for (let i = 0; i < groupChatUsers.length; i++) {
            customersId.push(groupChatUsers[i].appUserId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatUsers]);

    const decreaseGroupChatMessagesCountAsync = async () => {
        const myGroupChatUser = groupChatUsers.filter(x => x.appUserId === me?.id)[0];

        const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: myGroupChatUser.id });
        if (messagesCount.data !== undefined) {
            const unblockedObject = Object.assign({}, messagesCount.data);
            unblockedObject.count = --unblockedObject.count;

            await updateGroupChatMessageCountMut(unblockedObject);
        }
    }

    const deleteMessageAsync = async (messageId) => {
        await removeGroupChatMessageAsync(messageId);
    }

    const saveScrollState = () => {
        const chatContainer = chatContainerRef.current;
        const previousScrollHeight = chatContainer.scrollHeight;
        const previousScrollTop = chatContainer.scrollTop;

        //const newMessages = addUniqueElements(currentMessages, messages);
        //setCurrentMessages(prevMessages => [...messages, ...prevMessages]);

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

    if (isLoading) {
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
                    messagePageRef={messagePageRef}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {currentMessages.map((message) => (
                        <li className="message" key={message.id}>
                            <ChatMessage
                                me={me}
                                message={message}
                                messageStatus={message.status}
                                updateChatMessageAsync={updateGroupChatMessageAsync}
                                deleteMessageAsync={deleteMessageAsync}
                                decreaseChatMessagesCountAsync={decreaseGroupChatMessagesCountAsync}
                            />
                        </li>
                    ))}
                </ul>
                <GroupChatMessageInput
                    chat={chat}
                    me={me}
                    groupChatUsers={groupChatUsers}
                    messageType={messageType}
                    t={t}
                />
                {showAddPeople &&
                    <GroupChatAddUser
                        chat={chat}
                        me={me}
                        groupChatUsersId={groupChatUsersId}
                        setShowAddPeople={setShowAddPeople}
                        groupChatUsers={groupChatUsers}
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
                    groupChatUsers={groupChatUsers}
                    meInChat={meInChat}
                    chat={chat}
                />
            }
            {userInformation}
        </div>
    );
}

export default memo(GroupChat);