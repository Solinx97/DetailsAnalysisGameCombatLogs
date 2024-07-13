import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import io from 'socket.io-client';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
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

const GroupChat = ({ chat, me, setSelectedChat, callMinimazedData }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);
    const [userInformation, setUserInformation] = useState(null);
    const [usersOnCall, setUsersOnCall] = useState(0);

    const socketRef = useRef(null);

    const { messages, meInChat, groupChatUsers, isLoading } = useGroupChatData(chat.id, me.id);

    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [removeGroupChatMessageAsync] = useRemoveGroupChatMessageAsyncMutation();

    useEffect(() => {
        let socket;

        const connectSocket = async () => {
            try {
                socket = io.connect("192.168.0.161:2000");

                socket.on("connect_error", (error) => {
                    console.error("Socket connection error:", error);
                });

                socket.emit("checkUsersOnCall", chat.id);

                socket.on("checkedUsersOnCall", (count) => {
                    setUsersOnCall(count);
                });

                socketRef.current = socket;
            } catch (error) {
                console.error("Socket connection error:", error);
            }
        };

        //connectSocket();

        return () => {
            if (socket) {
                socket.emit("removeUserFromChat", chat.id);
                socket.off("checkedUsersOnCall");
                socket.disconnect();
            }
        };
    }, []);

    useEffect(() => {
        if (groupChatUsers === undefined) {
            return;
        }

        const customersId = [];
        for (let i = 0; i < groupChatUsers.length; i++) {
            customersId.push(groupChatUsers[i].customerId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatUsers]);

    const decreaseGroupChatMessagesCountAsync = async () => {
        const myGroupChatUser = groupChatUsers.filter(x => x.customerId === me?.id)[0];

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

    if (isLoading) {
        return (<Loading />);
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <GroupChatTitle
                    chat={chat}
                    me={me}
                    usersOnCall={usersOnCall}
                    callMinimazedData={callMinimazedData}
                    settingsIsShow={settingsIsShow}
                    setSettingsIsShow={setSettingsIsShow}
                    t={t}
                />
                <ul className="chat-messages">
                    {messages?.map((message) => (
                        <li key={message.id}>
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

export default memo(WithVoiceContext(GroupChat));