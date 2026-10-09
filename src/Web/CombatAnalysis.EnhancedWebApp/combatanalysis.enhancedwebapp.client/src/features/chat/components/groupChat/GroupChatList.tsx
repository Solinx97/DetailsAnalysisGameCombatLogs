import Store from '@/app/Store';
import { GroupChatUserApi } from '@/features/chat/api/GroupChatUser.api';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState, type SetStateAction } from 'react';
import { useFindChatUsersQuery } from '../../api/GroupChatUser.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { GroupChatUserModel } from '../../types/GroupChatUserModel';
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
    const { data: myselfInGroupChats, isLoading } = useFindChatUsersQuery(myselfId);

    const chatHub = useChatHub();

    const [extendedMyselfInGroupChats, setExtendedMyselfInGroupChats] = useState<GroupChatUserModel[]>([]);
    const [newChatUser, setNewChatUser] = useState<GroupChatUserModel | null>(null);

    useEffect(() => {
        return () => {
            (async () => {
                await chatHub?.disconnectFromGroupChatUnreadMessagesHubAsync();
            })();
        }
    }, [chatHub]);

    useEffect(() => {
        if (!chatHub || !myselfInGroupChats) {
            return;
        }

        setExtendedMyselfInGroupChats(myselfInGroupChats);

        (async () => {
            const myGroupChatsId = myselfInGroupChats.map((key) => key.groupChatId);
            await chatHub.connectToGroupChatUnreadMessagesAsync(myGroupChatsId);

            chatHub?.subscribeToGroupChat((groupChatUser) => {
                Store.dispatch(
                    GroupChatUserApi.util.updateQueryData("findChatUsers", myselfId, (draft) => {
                        draft.push(groupChatUser);
                    })
                );

                setNewChatUser(groupChatUser);
            });
        })();
    }, [myselfInGroupChats]);

    useEffect(() => {
        if (!newChatUser) {
            return;
        }

        const updatedChatUsers = Array.from(extendedMyselfInGroupChats);
        updatedChatUsers.push(newChatUser);
        setExtendedMyselfInGroupChats(updatedChatUsers);
    }, [newChatUser]);

    if (isLoading || !chatHub) {
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
                {extendedMyselfInGroupChats.length === 0
                    ? <div className="group-chats not-found">
                        <div>{t("GroupChatsEmptyYet")}</div>
                        <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span>
                    </div>
                    : extendedMyselfInGroupChats.map((myselfInChat) => (
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