import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useChatHub } from '../../../context/ChatHubProvider';
import { useFindGroupChatUserByUserIdQuery } from '../../../store/api/chat/GroupChatUser.api';
import GroupChatListItem from './GroupChatListItem';

const GroupChatList = ({ meId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden, setShowCreateGroupChat }) => {
    const { data, isLoading } = useFindGroupChatUserByUserIdQuery(meId);

    const { connectToGroupChatUnreadMessagesAsync, subscribeToUnreadPersonalMessagesUpdated, subscribeToGroupChat } = useChatHub();

    const [meInGroupChats, setMeInGroupChats] = useState([]);

    useEffect(() => {
        if (!data) {
            return;
        }

        setMeInGroupChats(data);

        const connectToPersonalChatUnreadMessages = async () => {
            await connectToGroupChatUnreadMessagesAsync(data);

            subscribeToGroupChat((groupChatUser) => {
                setMeInGroupChats(prev => [...prev, groupChatUser]);
            });
        }

        connectToPersonalChatUnreadMessages();
    }, [data]);

    if (isLoading) {
        return (<></>);
    }

    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("GroupChats")}</div>
                <div className="not-found">
                    <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                </div>
                <FontAwesomeIcon
                    icon={chatsHidden ? faArrowDown : faArrowUp}
                    title={chatsHidden ? t("ShowChats") : t("HideChats")}
                    onClick={toggleChatsHidden}
                />
            </div>
            <ul className={`chat-list__chats${!chatsHidden ? "_active" : ""}`}>
                {meInGroupChats.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                    </div>
                    : meInGroupChats.map((meInChat) => (
                        <li key={meInChat.id} className={selectedChat.type === "group" && selectedChat.chat?.id === meInGroupChats?.chatId ? `selected` : ``}>
                            <GroupChatListItem
                                chatId={meInChat.chatId}
                                setSelectedGroupChat={setSelectedChat}
                                meInChatId={meInChat.id}
                                subscribeToUnreadPersonalMessagesUpdated={subscribeToUnreadPersonalMessagesUpdated}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GroupChatList;