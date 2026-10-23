import { type RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import ChatHubContext from '@/context/ChatHubContext';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { ChatType, } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { faPhone } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useContext, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useGetMessagesByGroupChatIdQuery } from '../../api/Chat.api';
import { useCountGroupChatMessagesQuery, useUpdateGroupChatMessageMarkMutation, useUpdateGroupChatMessageTextMutation } from '../../api/GroupChatMessage.api';
import { useFindAllChatUsersQuery } from '../../api/GroupChatUser.api';
import useMessageEvents from '../../hooks/useMessageEvents';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import GroupChatMenu from './GroupChatMenu';
import GroupChatTitle from './GroupChatTitle';

import './GroupChat.scss';

interface GroupChatProps {
    chat: GroupChatModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
}

const GroupChat: React.FC<GroupChatProps> = ({ chat, setSelectedChat }) => {
    const { t } = useTranslation('communication/chats/groupChat');

    const groupChatUser = useSelector((state: RootState) => state.groupChatUser.value);

    const navigate = useNavigate();

    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [usersId, setUsersId] = useState<string[]>([]);
    const [voiceChatUsersCount, setVoiceChatUsersCount] = useState<number>(0);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 10);

    const { groupChatEvents } = useMessageEvents(chat.id, page);

    const { data: messages, isLoading } = useGetMessagesByGroupChatIdQuery({ chatId: chat.id, page, pageSize: pageSizeRef.current });
    const { data: count } = useCountGroupChatMessagesQuery(chat.id);
    
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindAllChatUsersQuery(chat.id);
    
    const [updateMessageText] = useUpdateGroupChatMessageTextMutation();
    const [updateMessageMark] = useUpdateGroupChatMessageMarkMutation();

    useEffect(() => {
        if (count === undefined) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < count);
    }, [page, count]);

    useEffect(() => {
        let isCancelled = false;

        const connect = async () => {
            if (isCancelled) {
                return;
            }

            await context.groupChatEvents.connectToChatMessagesAsync(chat.id);

            context.groupChatEvents.subscribeToChatMessages(groupChatEvents.handleMessage);
            context.groupChatEvents.subscribeToMessageRead(groupChatEvents.handleMessageRead);
            context.groupChatEvents.subscribeToChatMessageTextEdit(groupChatEvents.handleMessageTextEdit);
            context.groupChatEvents.subscribeToChatMessageMarkEdit(groupChatEvents.handleMessageMarkEdit);

            const count = await context.voiceChatEvents.connectToVoiceChatUsersCount(chat.id.toString());
            setVoiceChatUsersCount(count);
            
            context.voiceChatEvents.subscribeToVoiceChatUsersCount(getVoiceChatUserCount);
        }

        connect();

        return () => {
            isCancelled = true;

            context.groupChatEvents.unsubscribeToChatMessages(groupChatEvents.handleMessage);
            context.groupChatEvents.unsubscribeToMessageRead(groupChatEvents.handleMessageRead);
            context.groupChatEvents.unsubscribeToChatMessageTextEdit(groupChatEvents.handleMessageTextEdit);
            context.groupChatEvents.unsubscribeToChatMessageMarkEdit(groupChatEvents.handleMessageMarkEdit);
            context.groupChatEvents.disconnectFromChatMessageHubAsync();

            context.voiceChatEvents.unsubscribeToVoiceChatUsersCount(getVoiceChatUserCount);
            context.voiceChatEvents.leaveFromChatUsersCountAsync(chat.id.toString());
        };
    }, []);

    useEffect(() => {
        if (!groupChatUsers) {
            return;
        }

        const receivedUsersId: string[] = [];
        for (let i = 0; i < groupChatUsers.length; i++) {
            receivedUsersId.push(groupChatUsers[i].appUserId);
        }

        setUsersId(receivedUsersId);
    }, [groupChatUsers]);

    const getVoiceChatUserCount = (count: number) => {
        setVoiceChatUsersCount(count);
    }

    const updateMessageTextAsync = async (messageId: string, newMessage: string | undefined) => {
        if (!newMessage) {
            return;
        }

        try {
            await updateMessageText({ id: messageId, newMessage }).unwrap();
        } catch (e) {
            logger.error("Failed to update group chat message", e);
        }
    }

    const updateMessageMarkAsync = async (messageId: string, newMark: number) => {
        try {
            await updateMessageMark({ id: messageId, newMark }).unwrap();
        } catch (e) {
            logger.error("Failed to update group chat message", e);
        }
    }

    const call = () => {
        navigate(`/chats/voice/${chat.id}/${chat.name}`);
    }

    if (isLoading || !messages
        || usersIsLoading || !groupChatUsers) {
        return (
            <div className="chats__selected-chat personal-chat">
                <div className="messages-container">
                    <GroupChatTitle
                        chat={chat}
                        settingsIsShow={settingsIsShow}
                        setSettingsIsShow={setSettingsIsShow}
                        setSelectedChat={setSelectedChat}
                        t={t}
                    />
                    <ul className="chat-messages" ref={chatContainerRef}></ul>
                    <MessageInput
                        page={page}
                        chatId={chat.id}
                        initiator={groupChatUser}
                        targetChatType={ChatType["GROUP"]}
                        t={t}
                    />
                </div>
                {(settingsIsShow && groupChatUser) &&
                    <GroupChatMenu
                        setSelectedChat={setSelectedChat}
                        usersId={usersId}
                        chat={chat}
                        t={t}
                    />
                }
            </div>
        );
    }

    return (
        <div className="chats__selected-chat">
            <div className="messages-container">
                <div className="voice-chat-actions">
                    <div className={`voice-chat-actions__call ${voiceChatUsersCount > 0 ? 'active' : ''}`} onClick={call}>
                        <FontAwesomeIcon
                            icon={faPhone}
                            className="call"
                        />
                        <div>Voice chat</div>
                        {voiceChatUsersCount > 0 &&
                            <div>{voiceChatUsersCount}</div>
                        }
                    </div>
                </div>
                <GroupChatTitle
                    chat={chat}
                    settingsIsShow={settingsIsShow}
                    setSettingsIsShow={setSettingsIsShow}
                    setSelectedChat={setSelectedChat}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {messages.map((message) => (
                        <li className="message" key={message.id}>
                            <ChatMessage
                                message={message}
                                lastReadMessageId={groupChatUser?.lastReadMessageId}
                                updateMessageTextAsync={updateMessageTextAsync}
                                updateMessageMarkAsync={updateMessageMarkAsync}
                            />
                        </li>
                    ))}
                    <li className="message">
                        <InfiniteScrollTrigger
                            onLoadMore={() => setPage(p => p + 1)}
                            hasMore={hasMore}
                            isLoading={isLoading}
                        />
                    </li>
                </ul>
                <MessageInput
                    page={page}
                    chatId={chat.id}
                    initiator={groupChatUser}
                    targetChatType={ChatType["GROUP"]}
                    t={t}
                />
            </div>
            {(settingsIsShow && groupChatUser) &&
                <GroupChatMenu
                    setSelectedChat={setSelectedChat}
                    usersId={usersId}
                    chat={chat}
                    t={t}
                />
            }
        </div>
    );
}

export default memo(GroupChat);