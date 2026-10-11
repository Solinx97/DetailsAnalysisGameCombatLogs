import { updateGroupChatUser } from '@/features/chat/store/GroupChatUserSlice';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { useEffect, type SetStateAction } from 'react';
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

    const chatHub = useChatHub();

    const { data: chat, isLoading } = useGetGroupChatByIdQuery(myselfInChat.groupChatId);

    useEffect(() => {
        if (!chatHub) {
            return;
        }

        let isCancelled = false;

        const handleMessage = async () => {
            await refetch();
        }

        const connect = async () => {
            await chatHub.connectToGroupChatMessagesAsync(myselfInChat.groupChatId);

            if (isCancelled) {
                return;
            }

            chatHub.subscribeToGroupChatMessages(handleMessage);
        };

        void connect();

        return () => {
            isCancelled = true;

            chatHub.unsubscribeToGroupChatMessages(handleMessage);
            chatHub.disconnectFromGroupChatMessageHubAsync();
        };
    }, [chatHub, myselfInChat, refetch]);

    const selectChatHandle = () => {
        setSelectedGroupChat(null);

        if (chat) {
            setSelectedGroupChat(chat);
            dispatch(updateGroupChatUser(myselfInChat));
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