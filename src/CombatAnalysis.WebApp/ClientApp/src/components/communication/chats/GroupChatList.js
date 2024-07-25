import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import GroupChatListItem from './GroupChatListItem';

const GroupChatList = ({ t, groupChatUsers, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden }) => {
    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("GroupChats")}</div>
                {chatsHidden
                    ? <FontAwesomeIcon
                        icon={faArrowDown}
                        title={t("ShowChats")}
                        onClick={toggleChatsHidden}
                    />
                    : <FontAwesomeIcon
                        icon={faArrowUp}
                        title={t("HideChats")}
                        onClick={toggleChatsHidden}
                    />
                }
            </div>
            <ul className={`chat-list__chats${!chatsHidden ? "_active" : ""}`}>
                {groupChatUsers?.length === 0
                    ? <div className="group-chats__not-found">{t("GroupChatsEmptyYet")}</div>
                    : groupChatUsers?.map((groupChatUser) => (
                        <li key={groupChatUser.id} className={selectedChat.type === "group" && selectedChat.chat?.id === groupChatUser?.groupChatId ? `selected` : ``}>
                            <GroupChatListItem
                                chatId={groupChatUser.groupChatId}
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