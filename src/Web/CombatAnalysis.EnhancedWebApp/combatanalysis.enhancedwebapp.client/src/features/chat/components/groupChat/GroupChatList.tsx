import Store from '@/app/Store';
import ChatHubContext from '@/context/ChatHubContext';
import { GroupChatUserApi } from '@/features/chat/api/GroupChatUser.api';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useContext, useEffect, type SetStateAction } from 'react';
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
    const { data: groupChats, refetch, isLoading } = useFindChatUsersQuery(myselfId);

    const context = useContext(ChatHubContext);
    if (!context) {
        throw new Error("Child must be inside ChatHubContext.Provider");
    }

    useEffect(() => {
        let isCancelled = false;

        const connect = async () => {
            if (isCancelled) {
                return;
            }

            await context.groupChatEvents.connectToGroupChatAsync();

            context.groupChatEvents.subscribeToGroupChat((groupChatUser) => {
                Store.dispatch(
                    GroupChatUserApi.util.updateQueryData("findChatUsers", myselfId, (draft) => {
                        draft.push(groupChatUser);
                    })
                );
            });

            context.groupChatEvents.subscribeToRemovedFromGroupChat((appUserId, chatId) => {
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
        }

        connect();

        return () => {
            isCancelled = true;

            context.personalChatEvents.disconnectFromPersonalChatHubAsync();
        };
    }, []);

    useEffect(() => {
        if (!selectedChat) {
            return;
        }

        (async () => {
            context.groupChatEvents.subscribeToRemovedFromGroupChat((_, chatId) => {
                if (chatId === selectedChat.id) {
                    setSelectedChat(null);
                }
            });
        })();
    }, [selectedChat]);

    if (!groupChats || isLoading) {
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
                                refetch={refetch}
                                myselfInChat={myselfInChat}
                                setSelectedGroupChat={setSelectedChat}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default GroupChatList;