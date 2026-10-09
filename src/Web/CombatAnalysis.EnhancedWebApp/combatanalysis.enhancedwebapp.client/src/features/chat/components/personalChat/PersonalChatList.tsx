import Store from '@/app/Store';
import { useChatHub } from '@/shared/hooks/useChatHub';
import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useEffect, type SetStateAction } from 'react';
import { PersonalChatApi, useGetPersonalChatsByUserIdQuery } from '../../api/PersonalChat.api';
import type { GroupChatModel } from '../../types/GroupChatModel';
import type { PersonalChatModel } from '../../types/PersonalChatModel';
import PersonalChatListItem from './PersonalChatListItem';

interface PersonalChatListProps {
    myselfId: string;
    selectedChat: PersonalChatModel | GroupChatModel | null;
    setSelectedChat: (value: SetStateAction<PersonalChatModel | GroupChatModel | null>) => void;
    chatsHidden: boolean;
    toggleChatsHidden: () => void;
    t: (key: string) => string;
}

const PersonalChatList: React.FC<PersonalChatListProps> = ({ myselfId, t, selectedChat, setSelectedChat, chatsHidden, toggleChatsHidden }) => {
    const { data: personalChats, isLoading } = useGetPersonalChatsByUserIdQuery(myselfId);

    const chatHub = useChatHub();

    useEffect(() => {
        return () => {
            (async () => {
                await chatHub?.disconnectFromPersonalChatHubAsync();
            })();
        }
    }, [chatHub]);

    useEffect(() => {
        if (!chatHub || !personalChats) {
            return;
        }

        (async () => {
            await chatHub.connectToPersonalChatAsync();

            chatHub?.subscribeToPersonalChat((chat) => {
                Store.dispatch(
                    PersonalChatApi.util.updateQueryData("getPersonalChatsByUserId", myselfId, (draft) => {
                        draft.push(chat);
                    })
                );
            });

            chatHub?.subscribeToRemovedFromPersonalChat((appUserId, chatId) => {
                Store.dispatch(
                    PersonalChatApi.util.updateQueryData("getPersonalChatsByUserId", myselfId, (draft) => {
                        const index = draft.findIndex(
                            chat => (chat.initiatorId === appUserId || chat.companionId === appUserId) && chat.id === chatId
                        );

                        if (index !== -1) {
                            draft.splice(index, 1);
                        }
                    })
                );
            });
        })();
    }, [personalChats]);

    useEffect(() => {
        if (!chatHub || !selectedChat) {
            return;
        }

        (async () => {
            chatHub?.subscribeToRemovedFromPersonalChat((_, chatId) => {
                if (chatId === selectedChat.id) {
                    setSelectedChat(null);
                }
            });
        })();
    }, [selectedChat]);

    if (!personalChats || !chatHub || isLoading) {
        return (<div>Loading...</div>);
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
                {personalChats.length === 0
                    ? <div className="personal-chats not-found">
                        {t("PersonalChatsEmptyYet")}
                    </div>
                    : personalChats.map((chat) => (
                        <li key={chat.id} className={selectedChat && "initiatorId" in selectedChat && selectedChat.id === chat.id ? `selected` : ``}>
                            <PersonalChatListItem
                                chat={chat}
                                setSelectedChat={setSelectedChat}
                                companionId={chat.initiatorId === myselfId ? chat.companionId : chat.initiatorId}
                                userId={myselfId}
                                chatHub={chatHub}
                            />
                        </li>
                    ))
                }
            </ul>
        </div>
    );
}

export default PersonalChatList;