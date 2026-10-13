import Store from '@/app/Store';
import { APP_CONFIG } from '@/config/appConfig';
import { MessageStatus } from '@/shared/helpers/EnumHelper';
import { useRef } from 'react';
import { ChatApi } from '../api/Chat.api';
import type { GroupChatMessageModel } from '../types/GroupChatMessageModel';
import type { PersonalChatMessageModel } from '../types/PersonalChatMessageModel';
import type { IMessageEvents } from '../interfaces/IMessageEvents';

const useMessageEvents = (
    chatId: number, 
    page: number
) : IMessageEvents => {
    const pageSizeRef = useRef<number>(APP_CONFIG.communication.chatPageSize ? +APP_CONFIG.communication.chatPageSize : 10);
    
    const handleGroupChatMessageReadEvent = (messageId: string) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByGroupChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
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

    const handleGroupChatMessageEvent = (message: GroupChatMessageModel) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByGroupChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
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

    const handleGroupChatMessageTextEditEvent = (mssageId: string, newMessage: string) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByGroupChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
                draft => {
                    const findMessage = draft.find(m => m.id === mssageId);
                    if (findMessage) {
                        const updatedMessage = Object.assign({}, findMessage);
                        updatedMessage.message = newMessage;
                        updatedMessage.isEdited = true;
                        Object.assign(findMessage, updatedMessage);
                    }
                }
            )
        );
    }

    const handleGroupChatMessageMarkEditEvent = (mssageId: string, newMark: number) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByGroupChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
                draft => {
                    const findMessage = draft.find(m => m.id === mssageId);
                    if (findMessage) {
                        const updatedMessage = Object.assign({}, findMessage);
                        updatedMessage.markedType = newMark;
                        Object.assign(findMessage, updatedMessage);
                    }
                }
            )
        );
    }

    const handlePersonalChatMessageReadEvent = (messageId: string) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByPersonalChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
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

    const handlePersonalChatMessageEvent = (message: PersonalChatMessageModel) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByPersonalChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
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

    const handlePersonalChatMessageTextEditEvent = (mssageId: string, newMessage: string) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByPersonalChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
                draft => {
                    const findMessage = draft.find(m => m.id === mssageId);
                    if (findMessage) {
                        const updatedMessage = Object.assign({}, findMessage);
                        updatedMessage.message = newMessage;
                        updatedMessage.isEdited = true;
                        Object.assign(findMessage, updatedMessage);
                    }
                }
            )
        );
    }

    const handlePersonalChatMessageMarkEditEvent = (mssageId: string, newMark: number) => {
        Store.dispatch(
            ChatApi.util.updateQueryData(
                'getMessagesByPersonalChatId',
                { chatId: chatId, page, pageSize: pageSizeRef.current },
                draft => {
                    const findMessage = draft.find(m => m.id === mssageId);
                    if (findMessage) {
                        const updatedMessage = Object.assign({}, findMessage);
                        updatedMessage.markedType = newMark;
                        Object.assign(findMessage, updatedMessage);
                    }
                }
            )
        );
    }

    const getGroupChatEvents = () => {
        return { 
            handleMessageRead: handleGroupChatMessageReadEvent,
            handleMessage: handleGroupChatMessageEvent,
            handleMessageTextEdit: handleGroupChatMessageTextEditEvent,
            handleMessageMarkEdit: handleGroupChatMessageMarkEditEvent
        };
    }

    const getPersonalChatEvents = () => {
        return { 
            handleMessageRead: handlePersonalChatMessageReadEvent,
            handleMessage: handlePersonalChatMessageEvent,
            handleMessageTextEdit: handlePersonalChatMessageTextEditEvent,
            handleMessageMarkEdit: handlePersonalChatMessageMarkEditEvent
        };
    }

    return { 
        groupChatEvents: getGroupChatEvents(),
        personalChatEvents: getPersonalChatEvents(),
    }
}

export default useMessageEvents;