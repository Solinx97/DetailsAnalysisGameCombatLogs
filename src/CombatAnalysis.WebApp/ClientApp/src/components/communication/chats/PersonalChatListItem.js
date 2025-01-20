import { useEffect, useState } from 'react';
import { useFindPersonalChatMessageCountQuery } from '../../../store/api/chat/PersonalChatMessagCount.api';
import { useGetUserByIdQuery } from '../../../store/api/user/Account.api';

const PersonalChatListItem = ({ chat, setSelectedPersonalChat, companionId, meId, hubConnection }) => {
    const [unreadMessageCount, setUnreadMessageCount] = useState(-1);

    const { data: user, isLoading } = useGetUserByIdQuery(companionId);
    const { data: messagesCount, isLoading: messagesCountLoading } = useFindPersonalChatMessageCountQuery({ chatId: chat?.id, userId: meId });

    useEffect(() => {
        hubConnection?.on("ReceiveUnreadMessageUpdated", async (chatId) => {
            await hubConnection?.invoke("RequestUnreadMessages", chatId, meId);
        });

        hubConnection?.on("ReceiveUnreadMessage", (targetChatId, targetMeInChatId, count) => {
            if (targetChatId === chat?.id && targetMeInChatId === meId) {
                setUnreadMessageCount(count);
            }
        });
    }, [hubConnection]);

    useEffect(() => {
        if (!messagesCount) {
            return;
        }

        setUnreadMessageCount(messagesCount.count);
    }, [messagesCount]);

    if (isLoading || messagesCountLoading) {
        return (<div className="chat-loading-yet">Loading...</div>)
    }

    return (
        <span className="chat-card" onClick={() => setSelectedPersonalChat({ type: "personal", chat: chat })}>
            <div className="username">{user?.username}</div>
            {unreadMessageCount > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{unreadMessageCount > 99 ? "99+" : unreadMessageCount}</div>
                </div>
            }
        </span>
    );
}

export default PersonalChatListItem;