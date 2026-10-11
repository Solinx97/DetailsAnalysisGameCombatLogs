import Store, { type RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import ChatHubContext from '@/context/ChatHubContext';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { ChatType, MessageStatus } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { memo, useContext, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import { ChatApi, useGetMessagesByPersonalChatIdQuery } from '../../api/Chat.api';
import {
    useCountPersonalChatMessagesQuery,
    usePartialUpdatePersonalChatMessageMutation
} from '../../api/PersonalChatMessage.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { ChatMessagePatch } from '../../types/patches/ChatMessagePatch';
import type { PersonalChatMessageModel } from '../../types/PersonalChatMessageModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import ChatMessage from '../ChatMessage';
import MessageInput from '../MessageInput';
import PersonalChatTitle from './PersonalChatTitle';

import './PersonalChat.scss';

interface PersonalChatProps {
    chat: PersonalChatModel;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    companionId: string;
}

const PersonalChat: React.FC<PersonalChatProps> = ({ chat, setSelectedChat, companionId }) => {
    const { t } = useTranslation('communication/chats/personalChat');

    const myself = useSelector((state: RootState) => state.user.value);

    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const [page, setPage] = useState(1);
    const [hasMore, setHasMore] = useState(false);

    const chatContainerRef = useRef<HTMLUListElement | null>(null);
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 10);

    const { data: messages, refetch, isLoading } = useGetMessagesByPersonalChatIdQuery({ chatId: chat.id, page, pageSize: pageSizeRef.current });
    const { data: count } = useCountPersonalChatMessagesQuery(chat.id);

    const { data: companion } = useGetUserByIdQuery(companionId);

    const [paerialUpdatePersonalChatMessage] = usePartialUpdatePersonalChatMessageMutation();

    useEffect(() => {
        if (!messages || !count) {
            return;
        }

        setHasMore(page * pageSizeRef.current < count);
    }, [messages, count]);

    useEffect(() => {
        (async () => {
            context.personalChatEvents.subscribeToPersonalMessageRead(async () => {
                await refetch();
            });

            context.personalChatEvents?.subscribeToPersonalChatMessages((message: PersonalChatMessageModel) => {
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

            context.personalChatEvents?.subscribeToPersonalChatMessageEdit((messagePatch: ChatMessagePatch) => {
                Store.dispatch(
                    ChatApi.util.updateQueryData(
                        'getMessagesByPersonalChatId',
                        { chatId: chat.id, page, pageSize: pageSizeRef.current },
                        draft => {
                            const message = draft.find(m => m.id === messagePatch.id);
                            if (message && messagePatch) {
                                const updatedMessage = Object.assign({}, message);
                                updatedMessage.message = messagePatch.message ?? "";
                                updatedMessage.status = messagePatch.status ?? MessageStatus["SENT"];
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
                await context.personalChatEvents?.disconnectFromPersonalChatMessageHubAsync();
            })();
        }
    }, [chat]);

    const updateMessageAsync = async (message: ChatMessagePatch) => {
        try {
            await paerialUpdatePersonalChatMessage({ id: message.id, message }).unwrap();

            if (context.personalChatMessagesHubConnectionRef.current) {
                await context.personalChatMessagesHubConnectionRef.current.invoke("RequestEditedMessage", message);
            }
        } catch (e) {
            logger.error("Failed to update personal chat message", e);
        }
    }

    if (isLoading) {
        return (
            <div className="chats__selected-chat personal-chat">
                <div className="messages-container">
                    <PersonalChatTitle
                        chat={chat}
                        companionUsername={companion?.username ?? ""}
                        setSelectedChat={setSelectedChat}
                        t={t}
                    />
                    <MessageInput
                        chatId={chat.id}
                        initiator={myself}
                        targetChatType={ChatType["PERSONAL"]}
                        t={t}
                    />
                </div>
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
                    targetChatType={ChatType["PERSONAL"]}
                    t={t}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);