import { type RootState } from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import ChatHubContext from '@/context/ChatHubContext';
import InfiniteScrollTrigger from '@/events/InfiniteScrollTrigger';
import { ChatType } from '@/shared/helpers/EnumHelper';
import logger from '@/utils/Logger';
import { memo, useContext, useEffect, useRef, useState, type SetStateAction } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import { useGetMessagesByPersonalChatIdQuery } from '../../api/Chat.api';
import {
    useCountPersonalChatMessagesQuery,
    useUpdatePersonalChatMessageMarkMutation,
    useUpdatePersonalChatMessageTextMutation
} from '../../api/PersonalChatMessage.api';
import useMessageEvents from '../../hooks/useMessageEvents';
import type { GroupChatModel } from '../../types/GroupChatModel';
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
    
    const { personalChatEvents } = useMessageEvents(chat.id, page);
  
    const { data: messages, isLoading } = useGetMessagesByPersonalChatIdQuery({ chatId: chat.id, page, pageSize: pageSizeRef.current });
    const { data: count } = useCountPersonalChatMessagesQuery(chat.id);

    const { data: companion } = useGetUserByIdQuery(companionId);

    const [updateMessageText] = useUpdatePersonalChatMessageTextMutation();
    const [updateMessageMark] = useUpdatePersonalChatMessageMarkMutation();

    useEffect(() => {
        if (!messages || !count) {
            return;
        }

        setHasMore(page * pageSizeRef.current < count);
    }, [messages, count]);

    useEffect(() => {
        let isCancelled = false;

        const connect = async () => {
            if (isCancelled) {
                return;
            }

            await context.personalChatEvents.connectToChatMessagesAsync(chat.id);

            context.personalChatEvents.subscribeToChatMessages(personalChatEvents.handleMessage);
            context.personalChatEvents.subscribeToMessageRead(personalChatEvents.handleMessageRead);
            context.personalChatEvents.subscribeToChatMessageTextEdit(personalChatEvents.handleMessageTextEdit);
            context.personalChatEvents.subscribeToChatMessageMarkEdit(personalChatEvents.handleMessageMarkEdit);
        }

        connect();

        return () => {
            isCancelled = true;

            context.personalChatEvents.unsubscribeToChatMessages(personalChatEvents.handleMessage);
            context.personalChatEvents.unsubscribeToMessageRead(personalChatEvents.handleMessageRead);
            context.personalChatEvents.unsubscribeToChatMessageTextEdit(personalChatEvents.handleMessageTextEdit);
            context.personalChatEvents.unsubscribeToChatMessageMarkEdit(personalChatEvents.handleMessageMarkEdit);
            context.personalChatEvents.disconnectFromChatMessageHubAsync();
        };
    }, []);

    const updateMessageTextAsync = async (messageId: string, newMessage: string | undefined) => {
        if (!newMessage) {
            return;
        }

        try {
            await updateMessageText({ id: messageId, newMessage }).unwrap();
        } catch (e) {
            logger.error("Failed to update personal chat message", e);
        }
    }

    const updateMessageMarkAsync = async (messageId: string, newMark: number) => {
        try {
            await updateMessageMark({ id: messageId, newMark }).unwrap();
        } catch (e) {
            logger.error("Failed to update group chat message", e);
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
                    <ul className="chat-messages" ref={chatContainerRef}></ul>
                    <MessageInput
                        page={page}
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
                    initiator={myself}
                    targetChatType={ChatType["PERSONAL"]}
                    t={t}
                />
            </div>
        </div>
    );
}

export default memo(PersonalChat);