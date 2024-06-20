import { faCloudArrowUp, faGear, faPen, faPhone } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import io from 'socket.io-client';
import WithVoiceContext from '../../../hocHelpers/WithVoiceContext';
import { useFindGroupChatMessageByChatIdQuery } from '../../../store/api/ChatApi';
import { useUpdateGroupChatAsyncMutation } from '../../../store/api/communication/chats/GroupChat.api';
import {
    useCreateGroupChatMessageCountAsyncMutation, useLazyFindGroupChatMessageCountQuery,
    useUpdateGroupChatMessageCountAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessagCount.api';
import {
    useCreateGroupChatMessageAsyncMutation,
    useUpdateGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/GroupChatMessage.api';
import {
    useCreateGroupChatUserAsyncMutation, useFindGroupChatUserByChatIdQuery,
    useFindGroupChatUserQuery
} from '../../../store/api/communication/chats/GroupChatUser.api';
import {
    useCreateUnreadGroupChatMessageAsyncMutation
} from '../../../store/api/communication/chats/UnreadGroupChatMessage.api';
import AddPeople from '../../AddPeople';
import GroupChatMenu from './GroupChatMenu';
import GroupChatMessageList from './GroupChatMessageList';

import '../../../styles/communication/chats/groupChat.scss';
import SendGroupChatMessage from './SendGroupChatMessage';

const getGroupChatMessagesInterval = 1000;
const messageType = {
    default: 0,
    system: 1
};

const GroupChat = ({ chat, me, setSelectedChat, callMinimazedData }) => {
    const { t } = useTranslation("communication/chats/groupChat");

    const navigate = useNavigate();

    const [showAddPeople, setShowAddPeople] = useState(false);
    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [peopleToJoin, setPeopleToJoin] = useState([]);
    const [groupChatUsersId, setGroupChatUsersId] = useState([]);
    const [userInformation, setUserInformation] = useState(null);
    const [editNameOn, setEditNameOn] = useState(false);
    const [usersOnCall, setUsersOnCall] = useState(0);

    const chatNameInput = useRef(null);
    const socketRef = useRef(null);

    const { data: messages, isLoading } = useFindGroupChatMessageByChatIdQuery(chat.id, {
        pollingInterval: getGroupChatMessagesInterval
    });
    const { data: meInChat, isLoading: myUsersIsLoading } = useFindGroupChatUserQuery({ chatId: chat?.id, userId: me?.id });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUserByChatIdQuery(chat.id);
    const [createGroupChatMessageAsync] = useCreateGroupChatMessageAsyncMutation();
    const [updateGroupChatMessageAsync] = useUpdateGroupChatMessageAsyncMutation();
    const [updateGroupChatAsyncMut] = useUpdateGroupChatAsyncMutation();
    const [createGroupChatUserMutAsync] = useCreateGroupChatUserAsyncMutation();
    const [updateGroupChatMessageCountMut] = useUpdateGroupChatMessageCountAsyncMutation();
    const [getMessagesCount] = useLazyFindGroupChatMessageCountQuery();
    const [createGroupChatCountAsyncMut] = useCreateGroupChatMessageCountAsyncMutation();
    const [createUnreadGroupChatMessageAsyncMut] = useCreateUnreadGroupChatMessageAsyncMutation();

    useEffect(() => {
        socketRef.current = io.connect("192.168.0.161:2000");

        socketRef.current.emit("checkUsersOnCall", chat.id);

        socketRef.current.on("checkedUsersOnCall", count => {
            setUsersOnCall(count);
        });

        return () => {
            socketRef.current.emit("removeUserFromChat", chat.id);

            socketRef.current.destroy();
        }
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

    const createGroupChatUserAsync = async () => {
        for (let i = 0; i < peopleToJoin.length; i++) {
            const newGroupChatUser = {
                id: " ",
                username: peopleToJoin[i].username,
                customerId: peopleToJoin[i].id,
                groupChatId: chat.id,
            };

            const created = await createGroupChatUserMutAsync(newGroupChatUser);
            if (created.data !== undefined) {
                await createGroupChatCountAsync(chat.id, created.data.id);

                const systemMessage = `'${me?.username}' added '${peopleToJoin[i].username}' to chat`;
                await createMessageAsync(systemMessage, messageType["system"]);
            }
        }

        setPeopleToJoin([]);
        setShowAddPeople(false);
    }

    const createGroupChatCountAsync = async (chatId, customerId) => {
        const newMessagesCount = {
            count: 0,
            groupChatUserId: customerId,
            groupChatId: +chatId,
        };

        const createdMessagesCount = await createGroupChatCountAsyncMut(newMessagesCount);
        return createdMessagesCount.data !== undefined;
    }

    const createMessageAsync = async (message, type) => {
        const today = new Date();
        const newMessage = {
            message: message,
            time: `${today.getHours()}:${today.getMinutes()}`,
            status: 0,
            type: type,
            groupChatId: chat.id,
            customerId: me?.id
        };

        const createdMessage = await createGroupChatMessageAsync(newMessage);
        if (createdMessage.data !== undefined && type !== messageType["system"]) {
            await messageSentSuccessfulAsync(createdMessage.data);
        }
    }

    const messageSentSuccessfulAsync = async (createdMessage) => {
        await updateGroupChatLastMessageAsync(createdMessage.message);

        const updateForMessage = Object.assign({}, createdMessage);
        updateForMessage.status = 1;

        await updateGroupChatMessageAsync(updateForMessage);

        const increaseUnreadMessages = 1;
        await updateGroupChatMessagesCountAsync(increaseUnreadMessages);

        await createUnreadMessageAsync(createdMessage.id);
    }

    const updateGroupChatLastMessageAsync = async (message) => {
        chat.lastMessage = message;

        await updateGroupChatAsyncMut(chat);
    }

    const updateGroupChatNameAsync = async () => {
        setEditNameOn(false);

        chat.name = chatNameInput.current.value;

        await updateGroupChatAsyncMut(chat);
    }

    const updateGroupChatMessagesCountAsync = async (count) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === me?.id) {
                continue;
            }

            const messagesCount = await getMessagesCount({ chatId: chat?.id, userId: groupChatUsers[i].id });
            if (messagesCount.data !== undefined) {
                const newMessagesCount = Object.assign({}, messagesCount.data);
                newMessagesCount.count = newMessagesCount.count + count;

                await updateGroupChatMessageCountMut(newMessagesCount);
            }
        }
    }

    const createUnreadMessageAsync = async (messageId) => {
        for (let i = 0; i < groupChatUsers.length; i++) {
            if (groupChatUsers[i].customerId === me?.id) {
                continue;
            }

            const newUnreadMessage = {
                groupChatUserId: groupChatUsers[i].id,
                groupChatMessageId: messageId
            }

            await createUnreadGroupChatMessageAsyncMut(newUnreadMessage);
        }
    }

    const call = () => {
        document.cookie = "callAlreadyStarted=true";

        navigate(`/chats/voice/${chat.id}/${chat.name}`);
    }

    if (isLoading || usersIsLoading || myUsersIsLoading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <div className="title">
                    <div className="title__content">
                        {chat?.customerId === me?.id &&
                            <FontAwesomeIcon
                                icon={faPen}
                                title={t("EditName")}
                                className={`settings-handler${editNameOn ? "_active" : ""}`}
                                onClick={() => setEditNameOn((item) => !item)}
                            />
                        }
                        {editNameOn
                            ? <>
                                <input className="form-control" type="text" defaultValue={chat.name} ref={chatNameInput} />
                                <FontAwesomeIcon
                                    icon={faCloudArrowUp}
                                    title={t("Save")}
                                    className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                                    onClick={async () => await updateGroupChatNameAsync()}
                                />
                            </>
                            : <div className="name" title={chat.name}>{chat.name}</div>
                        }
                    </div>
                    {usersOnCall > 0 &&
                        <div className="title__call-started">Call started</div>
                    }
                    <div className="title__menu">
                        {(callMinimazedData.current.stream !== null && +callMinimazedData.current.roomId !== +chat.id)
                            ? <FontAwesomeIcon
                                icon={faPhone}
                                title={t("Call move to minimaze")}
                                className="call-minimazed"
                            />
                            : <FontAwesomeIcon
                                icon={faPhone}
                                title={t("Call")}
                                className={`call${usersOnCall > 0 ? "_active" : ""}`}
                                onClick={call}
                            />
                        }
                        <FontAwesomeIcon
                            icon={faGear}
                            title={t("Settings")}
                            className={`settings-handler${settingsIsShow ? "_active" : ""}`}
                            onClick={() => setSettingsIsShow(!settingsIsShow)}
                        />
                    </div>
                </div>
                <GroupChatMessageList
                    chat={chat}
                    messages={messages}
                    me={me}
                />
                <SendGroupChatMessage
                    chat={chat}
                    me={me}
                    groupChatUsers={groupChatUsers}
                    messageType={messageType}
                />
                {showAddPeople &&
                    <div className="add-people-to-chat box-shadow">
                        <AddPeople
                            customer={me}
                            communityUsersId={groupChatUsersId}
                            peopleToJoin={peopleToJoin}
                            setPeopleToJoin={setPeopleToJoin}
                        />
                        <div className="item-result">
                            <div className="btn-border-shadow invite" onClick={async () => await createGroupChatUserAsync()}>{t("Invite")}</div>
                            <div className="btn-border-shadow" onClick={() => setShowAddPeople(false)}>{t("Close")}</div>
                        </div>
                    </div>
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