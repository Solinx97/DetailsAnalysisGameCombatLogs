import Store, { type RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import Loading from '@/shared/components/Loading';
import { useChatHub } from '@/shared/hooks/useChatHub';
import logger from '@/utils/Logger';
import { memo, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useSelector } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import { ChatApi, useGetMessagesByPersonalChatIdQuery } from '../../api/Chat.api';
import {
    usePartialUpdatePersonalChatMessageMutation
} from '../../api/PersonalChatMessage.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { ChatMessagePatch } from '../../types/patches/ChatMessagePatch';
import type { PersonalChatMessageModel } from '../../types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import PersonalChatTitle from './PersonalChatTitle';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';

import './PersonalChat.scss';

interface PersonalChatProps {
    chat: PersonalChatModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    companionId: string;
}

const PersonalChat: React.FC<PersonalChatProps> = ({ chat, setSelectedChat, companionId }) => {
    const { t } = useTranslation('communication/chats/personalChat');

    const myself = useSelector((state: RootState) => state.user.value);

    const chatHub = useChatHub();

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 10);

    // const { data: count, isLoading: countIsLoading } = useGetPersonalChatMessageCountByChatIdQuery(chat.id);
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({ chatId: chat.id, page, pageSize: pageSizeRef.current });

    const { data: companion, isLoading: companionIsLoading } = useGetUserByIdQuery(companionId);
    const [paerialUpdatePersonalChatMessage] = usePartialUpdatePersonalChatMessageMutation();

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
            await chatHub.connectToPersonalChatMessagesAsync(chat.id);

            chatHub.subscribeToPersonalChatMessages((message: PersonalChatMessageModel) => {
                Store.dispatch(
                    ChatApi.util.updateQueryData(
                        'getMessagesByPersonalChatId',
                        { chatId: chat.id, page, pageSize: pageSizeRef.current },
                        draft => {
                            draft.unshift(message);
                        }
                    )
                );
            });

            chatHub.subscribeToPersonalChatMessageEdit((messagePatch: ChatMessagePatch) => {
                Store.dispatch(
                    ChatApi.util.updateQueryData(
                        'getMessagesByPersonalChatId',
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

    const updateMessageAsync = async (message: ChatMessagePatch) => {
        try {
            await paerialUpdatePersonalChatMessage({ id: message.id, message }).unwrap();

            if (chatHub && chatHub.personalChatMessagesHubConnectionRef.current) {
                await chatHub.personalChatMessagesHubConnectionRef.current.invoke("RequestEditedMessage", message);
            }
        } catch (e) {
            logger.error("Failed to update personal chat message", e);
        }
    }

    if (!chatHub || isLoading || companionIsLoading) {
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
                    companionUsername={companion?.username ?? ""}
                    setSelectedChat={setSelectedChat}
                    t={t}
                />
                <ul className="chat-messages" ref={chatContainerRef}>
                    {messages?.map((message) => (
                        <li key={message.id}>
                            <ChatMessage
                                message={message}
                                updateMessageAsync={updateMessageAsync}
                                hubConnection={chatHub.personalChatMessagesHubConnectionRef.current}
                                subscribeToChatMessageHasBeenRead={chatHub.subscribeToPersonalMessageHasBeenRead}
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
                    chatId={chat.id}
                    initiator={myself}
                    targetChatType={0}
                    t={t}
                    recipientId={chat.initiatorId === myself?.id ? chat.companionId : chat.initiatorId}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);