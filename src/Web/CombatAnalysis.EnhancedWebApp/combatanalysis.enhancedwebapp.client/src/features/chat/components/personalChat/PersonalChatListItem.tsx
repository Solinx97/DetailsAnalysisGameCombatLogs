import ChatHubContext from '@/context/ChatHubContext';
import { useContext, useEffect, type SetStateAction } from 'react';
import { useGetUserByIdQuery } from '../../../user/api/Account.api';
import type { useGetPersonalChatsByUserIdQuery } from '../../api/PersonalChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

type RefetchType = ReturnType<
    typeof useGetPersonalChatsByUserIdQuery
>["refetch"];

interface PersonalChatListItemProps {
    refetch: RefetchType;
    chat: PersonalChatModel;
    isInitiator: boolean;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
}

const PersonalChatListItem: React.FC<PersonalChatListItemProps> = ({ refetch, chat, isInitiator, setSelectedChat }) => {
    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const { data: companion, isLoading } = useGetUserByIdQuery(isInitiator ? chat.companionId : chat.initiatorId);

    useEffect(() => {
        let isCancelled = false;

        const handleMessage = async () => {
            await refetch();
        }

        const connect = async () => {
            if (isCancelled) {
                return;
            }
            
            await context.personalChatEvents.connectToChatMessagesAsync(chat.id);
            context.personalChatEvents.subscribeToChatMessages(handleMessage);
            context.personalChatEvents.subscribeToMessageRead(handleMessage);
        }

        connect();

        return () => {
            isCancelled = true;

            context.personalChatEvents.unsubscribeToChatMessages(handleMessage);
            context.personalChatEvents.unsubscribeToMessageRead(handleMessage);
            context.personalChatEvents.disconnectFromChatMessageHubAsync();
        };
    }, []);

    if (isLoading) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={() => setSelectedChat(chat)}>
            <div className="username">{companion?.username}</div>
            {isInitiator
                ? (chat.initiatorUnreadMessages && chat.initiatorUnreadMessages > 0)
                    ? <div className="chat-tooltip">
                        <div className="unread-message-count">{chat.initiatorUnreadMessages > 99 ? "99+" : chat.initiatorUnreadMessages}</div>
                    </div>
                    : <></>
                : (chat.companionUnreadMessages && chat.companionUnreadMessages > 0)
                    ? <div className="chat-tooltip">
                        <div className="unread-message-count">{chat.companionUnreadMessages > 99 ? "99+" : chat.companionUnreadMessages}</div>
                    </div>
                    : <></>
            }
        </span>
    );
}

export default PersonalChatListItem;