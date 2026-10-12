import Store, { type RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import ChatHubContext from '@/context/ChatHubContext';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { ChatType, MessageStatus } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { memo, useContext, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { ChatApi, useGetMessagesByGroupChatIdQuery } from '../../api/Chat.api';
import { useCountGroupChatMessagesQuery, usePartialUpdateGroupChatMessageMutation } from '../../api/GroupChatMessage.api';
import { useFindAllChatUsersQuery } from '../../api/GroupChatUser.api';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { ChatMessagePatch } from '../../types/patches/ChatMessagePatch';
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

    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [usersId, setUsersId] = useState<string[]>([]);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 10);

    const { data: messages, isLoading } = useGetMessagesByGroupChatIdQuery({ chatId: chat.id, page, pageSize: pageSizeRef.current });
    const { data: count } = useCountGroupChatMessagesQuery(chat.id);

    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindAllChatUsersQuery(chat.id);

    const [partialUpdateGroupChatMessage] = usePartialUpdateGroupChatMessageMutation();

    useEffect(() => {
        if (count === undefined) {
            return;
        }

        setHasMore((page * pageSizeRef.current) < count);
    }, [page, count]);

    useEffect(() => {
        let isCancelled = false;

        const handleMessageRead = (messageId: string) => {
            Store.dispatch(
                ChatApi.util.updateQueryData(
                    'getMessagesByGroupChatId',
                    { chatId: chat.id, page, pageSize: pageSizeRef.current },
                    draft => {
                        const findMessage = draft.find(m => m.id === messageId);
                        if (findMessage) {
                            const updatedMessage = Object.assign({}, findMessage);
                            updatedMessage.status = MessageStatus["READ"];
                            Object.assign(findMessage, updatedMessage);
                        }
                    }
                )
            );
        }

        const handleMessage = (message: GroupChatMessageModel) => {
            Store.dispatch(
                ChatApi.util.updateQueryData(
                    'getMessagesByGroupChatId',
                    { chatId: chat.id, page, pageSize: pageSizeRef.current },
                    draft => {
                        const findMessage = draft.find(m => m.id === message.id);
                        if (findMessage) {
                            Object.assign(findMessage, message);
                        }
                        else {
                            draft.unshift(message);
                        }
                    }
                )
            );
        }

        const handleMessageEdit = (messagePatch: ChatMessagePatch) => {
            Store.dispatch(
                ChatApi.util.updateQueryData(
                    'getMessagesByGroupChatId',
                    { chatId: chat.id, page, pageSize: pageSizeRef.current },
                    draft => {
                        const message = draft.find(m => m.id === messagePatch.id);
                        if (message && messagePatch) {
                            const updatedMessage = Object.assign({}, message);
                            updatedMessage.message = messagePatch.message ?? "";
                            updatedMessage.status = messagePatch.status ?? MessageStatus["SENT"]
                            updatedMessage.markedType = messagePatch.markedType ?? 0;

                            Object.assign(message, updatedMessage);
                        }
                    }
                )
            );
        }

        const connect = async () => {
            if (isCancelled) {
                return;
            }

            await context.groupChatEvents.connectToChatMessagesAsync(chat.id);

            context.groupChatEvents.subscribeToChatMessages(handleMessage);
            context.groupChatEvents.subscribeToMessageRead(handleMessageRead);
            context.groupChatEvents.subscribeToChatMessageEdit(handleMessageEdit);
        }

        connect();

        return () => {
            isCancelled = true;

            context.groupChatEvents.unsubscribeToChatMessages(handleMessage);
            context.groupChatEvents.unsubscribeToMessageRead(handleMessageRead);
            context.groupChatEvents.unsubscribeToChatMessageEdit(handleMessageEdit);
            context.groupChatEvents.disconnectFromChatMessageHubAsync();
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

    const updateMessageAsync = async (message: ChatMessagePatch) => {
        try {
            await partialUpdateGroupChatMessage({ id: message.id, message }).unwrap();

            if (context.groupChatMessagesHubConnectionRef.current) {
                await context.groupChatMessagesHubConnectionRef.current.invoke("RequestEditedMessage", message);
            }
        } catch (e) {
            logger.error("Failed to update group chat message", e);
        }
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
                        t={t}
                    />
                    <ul className="chat-messages" ref={chatContainerRef}></ul>
                    {groupChatUser &&
                        <MessageInput
                            page={page}
                            chatId={chat.id}
                            initiator={groupChatUser}
                            targetChatType={ChatType["GROUP"]}
                            t={t}
                        />
                    }
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
                <GroupChatTitle
                    chat={chat}
                    settingsIsShow={settingsIsShow}
                    setSettingsIsShow={setSettingsIsShow}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {messages.map((message) => (
                        <li className="message" key={message.id}>
                            <ChatMessage
                                message={message}
                                updateMessageAsync={updateMessageAsync}
                                lastReadMessageId={groupChatUser?.lastReadMessageId}
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
                {groupChatUser &&
                    <MessageInput
                        page={page}
                        chatId={chat.id}
                        initiator={groupChatUser}
                        targetChatType={ChatType["GROUP"]}
                        t={t}
                    />
                }
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