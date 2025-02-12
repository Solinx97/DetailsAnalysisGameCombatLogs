import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, useState } from 'react';
import { useChatHub } from '../../../context/ChatHubProvider';
import { useGetByUserIdAsyncQuery } from '../../../store/api/chat/PersonalChat.api';
import { PersonalChat } from '../../../types/PersonalChat';
import { PersonalChatListProps } from '../../../types/components/communication/chats/PersonalChatListProps';
import PersonalChatListItem from './PersonalChatListItem';

const PersonalChatList: React.FC<PersonalChatListProps> = ({ meId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden }) => {
    const { data: personalChats, isLoading } = useGetByUserIdAsyncQuery(meId);

    const { connectToPersonalChatUnreadMessagesAsync, subscribeToUnreadPersonalMessagesUpdated, subscribeToPersonalChat } = useChatHub();

    const [chats, setChats] = useState<PersonalChat[]>([]);

    useEffect(() => {
        if (!personalChats) {
            return;
        }

        setChats(personalChats);

        const connectToPersonalChatUnreadMessages = async () => {
            await connectToPersonalChatUnreadMessagesAsync(personalChats);

            subscribeToPersonalChat((chat: PersonalChat) => {
                setChats(prevChats => [...prevChats, chat]);
            });
        }

        connectToPersonalChatUnreadMessages();
    }, [personalChats]);

    if (isLoading) {
        return (<></>);
    }

    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("PersonalChats")}</div>
                <FontAwesomeIcon
                    icon={chatsHidden ? faArrowDown : faArrowUp}
                    title={chatsHidden ? t("ShowChats") : t("HideChats")}
                    onClick={toggleChatsHidden}
                />
            </div>
            <ul className={`chat-list__chats${!chatsHidden ? "_active" : ""}`}>
                {chats.length === 0
                    ? <div className="personal-chats not-found">
                        {t("PersonalChatsEmptyYet")}
                    </div>
                    : chats.map((chat) => (
                        <li key={chat.id} className={selectedChat.type === "personal" && selectedChat.chat?.id === chat?.id ? `selected` : ``}>
                            <PersonalChatListItem
                                chat={chat}
                                setSelectedChat={setSelectedChat}
                                companionId={chat.initiatorId === meId ? chat.companionId : chat.initiatorId}
                                meId={meId}
                                subscribeToUnreadPersonalMessagesUpdated={subscribeToUnreadPersonalMessagesUpdated}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default PersonalChatList;