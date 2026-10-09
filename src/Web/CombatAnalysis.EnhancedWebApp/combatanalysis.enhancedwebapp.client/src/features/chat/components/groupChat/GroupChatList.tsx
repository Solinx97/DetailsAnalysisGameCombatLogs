import Store from '@/app/Store';
import { GroupChatUserApi } from '@/features/chat/api/GroupChatUser.api';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, type SetStateAction } from 'react';
import { useFindChatUsersQuery } from '../../api/GroupChatUser.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import GroupChatListItem from './GroupChatListItem';

interface GroupChatListProps {
    myselfId: string;
    selectedChat: GroupChatModel | PersonalChatModel | null;
    setSelectedChat: (value: SetStateAction<GroupChatModel | PersonalChatModel | null>) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    setShowCreateGroupChat: (value: SetStateAction<boolean>) => void;
    t: (key: string) => string;
}

const GroupChatList: React.FC<GroupChatListProps> = ({ myselfId, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden, setShowCreateGroupChat, t }) => {
    const { data: groupChats, isLoading } = useFindChatUsersQuery(myselfId);

    const chatHub = useChatHub();

    useEffect(() => {
        return () => {
            (async () => {
                await chatHub?.disconnectFromGroupChatHubAsync();
            })();
        }
    }, [chatHub]);

    useEffect(() => {
        if (!chatHub || !groupChats) {
            return;
        }

        (async () => {
            await chatHub.connectToGroupChatAsync();

            chatHub?.subscribeToGroupChat((groupChatUser) => {
                Store.dispatch(
                    GroupChatUserApi.util.updateQueryData("findChatUsers", myselfId, (draft) => {
                        draft.push(groupChatUser);
                    })
                );
            });

            chatHub?.subscribeToRemovedFromGroupChat((appUserId, chatId) => {
                Store.dispatch(
                    GroupChatUserApi.util.updateQueryData("findChatUsers", myselfId, (draft) => {
                        const index = draft.findIndex(
                            chatUser => chatUser.appUserId === appUserId && chatUser.groupChatId === chatId
                        );

                        if (index !== -1) {
                            draft.splice(index, 1);
                        }
                    })
                );
            });
        })();
    }, [groupChats]);

    useEffect(() => {
        if (!chatHub || !selectedChat) {
            return;
        }

        (async () => {
            chatHub?.subscribeToRemovedFromGroupChat((_, chatId) => {
                if (chatId === selectedChat.id) {
                    setSelectedChat(null);
                }
            });
        })();
    }, [selectedChat]);

    if (!groupChats || isLoading || !chatHub) {
        return (<div>Loading...</div>);
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
                {groupChats.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                    </div>
                    : groupChats.map((myselfInChat) => (
                        <li key={myselfInChat.id} className={selectedChat && "ownerId" in selectedChat && selectedChat.id === myselfInChat.groupChatId ? `selected` : ``}>
                            <GroupChatListItem
                                myselfInChat={myselfInChat}
                                setSelectedGroupChat={setSelectedChat}
                                chatHub={chatHub}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GroupChatList;