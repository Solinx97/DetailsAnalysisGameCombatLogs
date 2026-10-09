import type { RootState } from '@/app/Store';
import CommunicationMenu from '@/shared/components/CommunicationMenu';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useLocation } from 'react-router-dom';
import { useLazyGetGroupChatByIdQuery } from '../api/GroupChat.api';
import { useLazyGetPersonalChatByIdQuery } from '../api/PersonalChat.api';
import type { GroupChatModel } from '../types/GroupChatModel';
import type { PersonalChatModel } from '../types/PersonalChatModel';
import CreateGroupChat from './create/CreateGroupChat';
import GroupChat from './groupChat/GroupChat';
import GroupChatList from './groupChat/GroupChatList';
import PersonalChat from './personalChat/PersonalChat';
import PersonalChatList from './personalChat/PersonalChatList';

import './Chats.scss';

const Chats: React.FC = () => {
    const { t } = useTranslation('communication/chats/chats');

    const location = useLocation();

    const myself = useSelector((state: RootState) => state.user.value);

    const [getPersonalChatByIdAsync] = useLazyGetPersonalChatByIdQuery();
    const [getGroupChatByIdAsync] = useLazyGetGroupChatByIdQuery();

    const [selectedChat, setSelectedChat] = useState<PersonalChatModel | GroupChatModel | null>(null);
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [showCreateGroupChat, setShowCreateGroupChat] = useState(false);

    const maxWidth = 425;
    const screenSize = useMemo(() => ({
        width: window.innerWidth,
        height: window.innerHeight
    }), []);

    useEffect(() => {
        (() => {
            const searchParams = new URLSearchParams(location.search);
            if (searchParams.get("personal") !== null) {
                const getPersonalChatById = async () => {
                    const id = parseInt(searchParams.get("personal") ?? "1");
                    console.log(id);
                    const chat = await getPersonalChatByIdAsync(id).unwrap();
                    setSelectedChat(chat);
                }

                getPersonalChatById();
            } else if (searchParams.get("group") !== null) {
                const getPersonalChatById = async () => {
                    const id = parseInt(searchParams.get("group") ?? "1");
                    const chat = await getGroupChatByIdAsync(id).unwrap();
                    setSelectedChat(chat);
                }

                getPersonalChatById();
            }
        })();
    }, []);

    const getCompanionId = (chat: PersonalChatModel | null) => {
        if (!chat) {
            return "0";
        }

        const id = chat.initiatorId === myself?.id ? chat.companionId : chat.initiatorId;

        return id;
    }

    if (screenSize.width <= maxWidth) {
        return (
            <>
                {showCreateGroupChat &&
                    <CreateGroupChat
                        setShowCreateGroupChat={setShowCreateGroupChat}
                    />
                }
                <div className="communication-content">
                    <div className="chats">
                        <div className="chats__title">
                            <FontAwesomeIcon
                                icon={faArrowLeft}
                                onClick={() => setSelectedChat(null)}
                            />
                        </div>
                        {(selectedChat && myself)
                            ? "ownerId" in selectedChat
                                ? <GroupChat
                                    key={selectedChat.id}
                                    chat={selectedChat}
                                    setSelectedChat={setSelectedChat}
                                />
                                : <PersonalChat
                                    key={selectedChat.id}                                
                                    chat={selectedChat}
                                    setSelectedChat={setSelectedChat}
                                    companionId={getCompanionId((selectedChat && "initiatorId" in selectedChat) ? selectedChat : null)}
                                />
                            : <div className="select-chat">
                                {t("SelectChat")} <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span> {t("NewChat")}
                            </div>
                        }
                    </div>
                </div>
                <CommunicationMenu
                    currentMenuItem={1}
                    hasSubMenu={false}
                />
            </>
        );
    }

    return (
        <>
            {showCreateGroupChat &&
                <CreateGroupChat
                    setShowCreateGroupChat={setShowCreateGroupChat}
                />
            }
            <div className="communication-content">
                <div className="chats">
                    {myself &&
                        <div className="chats__my-chats">
                            <GroupChatList
                                myselfId={myself.id}
                                t={t}
                                selectedChat={selectedChat}
                                setSelectedChat={setSelectedChat}
                                chatsHidden={groupChatsHidden}
                                toggleChatsHidden={() => setGroupChatsHidden(prev => !prev)}
                                setShowCreateGroupChat={setShowCreateGroupChat}
                            />
                            <PersonalChatList
                                myselfId={myself.id}
                                t={t}
                                selectedChat={selectedChat}
                                setSelectedChat={setSelectedChat}
                                chatsHidden={personalChatsHidden}
                                toggleChatsHidden={() => setPersonalChatsHidden(prev => !prev)}
                            />
                        </div>
                    }
                    {(selectedChat && myself)
                        ? "ownerId" in selectedChat
                            ? <GroupChat
                                key={selectedChat.id}
                                chat={selectedChat}
                                setSelectedChat={setSelectedChat}
                            />
                            : <PersonalChat
                                key={selectedChat.id}
                                chat={selectedChat}
                                setSelectedChat={setSelectedChat}
                                companionId={getCompanionId((selectedChat && "initiatorId" in selectedChat) ? selectedChat : null)}
                            />
                        : <div className="select-chat">
                            {t("SelectChat")} <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span> {t("NewChat")}
                        </div>
                    }
                </div>
            </div>
            <CommunicationMenu
                currentMenuItem={1}
                hasSubMenu={false}
            />
        </>
    );
}

export default memo(Chats);