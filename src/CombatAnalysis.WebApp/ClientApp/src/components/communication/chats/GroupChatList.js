import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GroupChatListItem from './GroupChatListItem';
import { NavLink } from 'react-router-dom';

const GroupChatList = ({ t, groupChatUsers, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden }) => {
    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("GroupChats")}</div>
                <FontAwesomeIcon
                    icon={chatsHidden ? faArrowDown : faArrowUp}
                    title={chatsHidden ? t("ShowChats") : t("HideChats")}
                    onClick={toggleChatsHidden}
                />
            </div>
            <ul className={`chat-list__chats${!chatsHidden ? "_active" : ""}`}>
                {groupChatUsers?.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <NavLink to="/chats/create">{t("Create")}</NavLink> 
                    </div>
                    : groupChatUsers?.map((groupChatUser) => (
                        <li key={groupChatUser.id} className={selectedChat.type === "group" && selectedChat.chat?.id === groupChatUser?.chatId ? `selected` : ``}>
                            <GroupChatListItem
                                chatId={groupChatUser.chatId}
                                setSelectedGroupChat={setSelectedChat}
                                groupChatUserId={groupChatUser.id}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GroupChatList;