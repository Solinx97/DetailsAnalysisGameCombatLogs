import Store, { type RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import { useChatHub } from '@/shared/hooks/useChatHub';
import logger from '@/utils/Logger';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { ChatApi } from '../../api/Chat.api';
import { useFindGroupChatUsersByChatIdQuery } from '../../api/GroupChatUser.api';
import { usePartialUpdateGroupChatMessageMutation } from '../../api/GroupChatMessage.api';
import { useGetMessagesByGroupChatIdQuery } from '../../api/Chat.api';
import type { GroupChatMessageModel } from '../../types/GroupChatMessageModel';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { ChatMessagePatch } from '../../types/patches/ChatMessagePatch';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import GroupChatMenu from './GroupChatMenu';
import GroupChatTitle from './GroupChatTitle';
import { useSelector } from 'react-redux';

import './GroupChat.scss';

interface GroupChatProps {
    chat: GroupChatModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
}

const GroupChat: React.FC<GroupChatProps> = ({ chat, setSelectedChat }) => {
    const { t } = useTranslation('communication/chats/groupChat');

    const groupChatUser = useSelector((state: RootState) => state.groupChatUser.value);

    const chatHub = useChatHub();

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const [settingsIsShow, setSettingsIsShow] = useState(false);
    const [groupChatUsersId, setGroupChatUsersId] = useState<string[]>([]);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 10);

    const { data: messages, isLoading } = useGetMessagesByGroupChatIdQuery({ chatId: chat.id, page, pageSize: pageSizeRef.current });
    const { data: groupChatUsers, isLoading: usersIsLoading } = useFindGroupChatUsersByChatIdQuery(chat.id);

    const [partialUpdateGroupChatMessage] = usePartialUpdateGroupChatMessageMutation();

    useEffect(() => {
        if (!messages) {
            return;
        }

        setHasMore(((page - 1) * pageSizeRef.current) < messages.length);
    }, [page, messages]);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        (async () => {
            await chatHub.connectToGroupChatMessagesAsync(chat.id);

            chatHub.subscribeToGroupChatMessages((message: GroupChatMessageModel) => {
                Store.dispatch(
                    ChatApi.util.updateQueryData(
                        'getMessagesByGroupChatId',
                        { chatId: chat.id, page, pageSize: pageSizeRef.current },
                        draft => {
                            draft.unshift(message);
                        }
                    )
                );
            });

            chatHub.subscribeToGroupChatMessageEdit((messagePatch: ChatMessagePatch) => {
                Store.dispatch(
                    ChatApi.util.updateQueryData(
                        'getMessagesByGroupChatId',
                        { chatId: chat.id, page, pageSize: pageSizeRef.current },
                        draft => {
                            const message = draft.find(m => m.id === messagePatch.id);
                            if (message && messagePatch) {
                                const updatedMessage = Object.assign({}, message);
                                updatedMessage.message = messagePatch.message ?? "";
                                updatedMessage.status = messagePatch.status ?? "Sent";
                                updatedMessage.markedType = messagePatch.markedType ?? 0;

                                Object.assign(message, updatedMessage);
                            }
                        }
                    )
                );
            });
        })();

        return () => {
            (async () => {
                await chatHub.disconnectFromGroupChatMessageHubAsync();
            })();
        }
    }, [chat]);

    useEffect(() => {
        if (!groupChatUsers) {
            return;
        }

        const customersId: string[] = [];
        for (let i = 0; i < groupChatUsers.length; i++) {
            customersId.push(groupChatUsers[i].appUserId);
        }

        setGroupChatUsersId(customersId);
    }, [groupChatUsers]);

    const updateMessageAsync = async (message: ChatMessagePatch) => {
        try {
            await partialUpdateGroupChatMessage({ id: message.id, message }).unwrap();

            if (chatHub && chatHub.groupChatMessagesHubConnectionRef.current) {
                await chatHub.groupChatMessagesHubConnectionRef.current.invoke("RequestEditedMessage", message);
            }
        } catch (e) {
            logger.error("Failed to update group chat message", e);
        }
    }

    if (isLoading || !messages || !chatHub
        || usersIsLoading || !groupChatUsers) {
        return (<></>);
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
                                hubConnection={chatHub.groupChatMessagesHubConnectionRef.current}
                                subscribeToChatMessageHasBeenRead={chatHub.subscribeToGroupMessageHasBeenRead}
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
                        chatId={chat.id}
                        initiator={groupChatUser}
                        targetChatType={1}
                        t={t}
                    />
                }
            </div>
            {(settingsIsShow && groupChatUser) &&
                <GroupChatMenu
                    setSelectedChat={setSelectedChat}
                    groupChatUsersId={groupChatUsersId}
                    chat={chat}
                    t={t}
                />
            }
        </div>
    );
}

export default memo(GroupChat);