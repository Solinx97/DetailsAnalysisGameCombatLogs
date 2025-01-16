import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GroupChatListItem from './GroupChatListItem';
import { NavLink } from 'react-router-dom';

const GroupChatList = ({ t, meInGroupChats, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden, hubConnection }) => {
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
                {meInGroupChats?.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <NavLink to="/chats/create">{t("Create")}</NavLink> 
                    </div>
                    : meInGroupChats?.map((meInChat) => (
                        <li key={meInChat.id} className={selectedChat.type === "group" && selectedChat.chat?.id === meInGroupChats?.chatId ? `selected` : ``}>
                            <GroupChatListItem
                                chatId={meInChat.chatId}
                                setSelectedGroupChat={setSelectedChat}
                                meInChatId={meInChat.id}
                                hubConnection={hubConnection}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GroupChatList;