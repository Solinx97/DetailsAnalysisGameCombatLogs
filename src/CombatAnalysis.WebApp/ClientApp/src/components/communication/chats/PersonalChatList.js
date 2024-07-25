import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import PersonalChatListItem from './PersonalChatListItem';

const PersonalChatList = ({ meId, t, personalChats, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden }) => {
    return (
        <div className="chat-list">
            <div className="chats__my-chats_title">
                <div>{t("PersonalChats")}</div>
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
                {personalChats?.length === 0
                    ? <div className="personal-chats__not-found">{t("PersonalChatsEmptyYet")}</div>
                    : personalChats?.map((chat) => (
                        <li key={chat.id} className={selectedChat.type === "personal" && selectedChat.chat?.id === chat?.id ? `selected` : ``}>
                            <PersonalChatListItem
                                chat={chat}
                                setSelectedPersonalChat={setSelectedChat}
                                companionId={chat.initiatorId === meId ? chat.companionId : chat.initiatorId}
                                meId={meId}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default PersonalChatList;