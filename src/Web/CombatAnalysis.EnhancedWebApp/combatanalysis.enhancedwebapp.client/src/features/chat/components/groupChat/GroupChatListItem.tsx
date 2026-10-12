import ChatHubContext from '@/context/ChatHubContext';
import { updateGroupChatUser } from '@/features/chat/store/GroupChatUserSlice';
import { useContext, useEffect, type SetStateAction } from 'react';
import { useDispatch } from 'react-redux';
import { useGetGroupChatByIdQuery } from '../../api/GroupChat.api';
import { useFindChatUsersQuery } from '../../api/GroupChatUser.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';

type RefetchType = ReturnType<
    typeof useFindChatUsersQuery
>["refetch"];

interface GroupChatListItemProps {
    refetch: RefetchType;
    myselfInChat: GroupChatUserModel;
    setSelectedGroupChat: (value: SetStateAction<GroupChatModel | PersonalChatModel | null>) => void;
}

const GroupChatListItem: React.FC<GroupChatListItemProps> = ({ myselfInChat, refetch, setSelectedGroupChat }) => {
    const dispatch = useDispatch();

    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    const { data: chat, isLoading } = useGetGroupChatByIdQuery(myselfInChat.groupChatId);

    useEffect(() => {
        let isCancelled = false;

        const handleMessage = async () => {
            await refetch();
        }

        const connect = async () => {
            if (isCancelled) {
                return;
            }

            await context.groupChatEvents.connectToChatMessagesAsync(myselfInChat.groupChatId);
            context.groupChatEvents.subscribeToChatMessages(handleMessage);
            context.groupChatEvents.subscribeToMessageRead(handleMessage);
        };

        connect();

        return () => {
            isCancelled = true;

            context.groupChatEvents.unsubscribeToChatMessages(handleMessage);
            context.groupChatEvents.unsubscribeToMessageRead(handleMessage);
            context.groupChatEvents.disconnectFromChatMessageHubAsync();
        };
    }, []);

    const selectChatHandle = () => {
        if (chat) {
            setSelectedGroupChat(chat);
            dispatch(updateGroupChatUser(myselfInChat));
        }
        else {
            setSelectedGroupChat(null);
        }
    }

    if (isLoading || !chat) {
        return (<div className="chat-loading-yet">Loading...</div>);
    }

    return (
        <span className="chat-card" onClick={selectChatHandle}>
            <div className="username">{chat?.name}</div>
            {myselfInChat.unreadMessages > 0 &&
                <div className="chat-tooltip">
                    <div className="unread-message-count">{myselfInChat.unreadMessages > 99 ? "99+" : myselfInChat.unreadMessages}</div>
                </div>
            }
        </span>
    );
}

export default GroupChatListItem;