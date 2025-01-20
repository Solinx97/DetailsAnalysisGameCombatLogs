import { faArrowLeft, faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { memo, useCallback, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import Loading from '../../Loading';
import CommunicationMenu from '../CommunicationMenu';
import CreateGroupChat from '../create/CreateGroupChat';
import GroupChat from './GroupChat';
import GroupChatList from './GroupChatList';
import PersonalChat from './PersonalChat';
import PersonalChatList from './PersonalChatList';

import "../../../styles/communication/chats/chats.scss";

const Chats = () => {
    const { t } = useTranslation("communication/chats/chats");

    const me = useSelector((state) => state.user.value);

    const [selectedChat, setSelectedChat] = useState({ type: null, chat: null });
    const [chatsHidden, setChatsHidden] = useState({ group: false, personal: false });
    const [showCreateGroupChat, setShowCreateGroupChat] = useState(false);
    const [showMenu, setShowMenu] = useState(false);

    const maxWidth = 425;
    const screenSize = useMemo(() => ({
        width: window.innerWidth,
        height: window.innerHeight
    }), []);

    const toggleChatsHidden = useCallback((type) => {
        setChatsHidden(prevState => ({ ...prevState, [type]: !prevState[type] }));
    }, []);

    if (me === null) {
        return (
            <>
                <CommunicationMenu currentMenuItem={1} />
                <Loading />
            </>
        );
    }

    if (selectedChat.type !== null && screenSize.width <= maxWidth) {
        return (
            <>
                {showCreateGroupChat &&
                    <CreateGroupChat />
                }
                {showMenu &&
                    <CommunicationMenu
                        currentMenuItem={1}
                    />
                }
                <div className="communication-content">
                    <div className="chats">
                        <div className="chats__title">
                            <FontAwesomeIcon
                                icon={faArrowLeft}
                                onClick={() => setSelectedChat({ type: null, chat: null })}
                            />
                            <FontAwesomeIcon
                                icon={faBars}
                                onClick={() => setShowMenu(!showMenu)}
                            />
                        </div>
                        {selectedChat.type === "group"
                            ? <GroupChat
                                chat={selectedChat.chat}
                                me={me}
                                setSelectedChat={setSelectedChat}
                            />
                            : selectedChat.type === "personal"
                                ? <PersonalChat
                                    chat={selectedChat.chat}
                                    me={me}
                                    setSelectedChat={setSelectedChat}
                                    companionId={selectedChat.chat.initiatorId === me?.id ? selectedChat.chat.companionId : selectedChat.chat.initiatorId}
                                />
                                : <div className="select-chat">
                                    {t("SelectChat")} <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span> {t("NewChat")}
                                </div>
                        }
                    </div>
                </div>
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
            <CommunicationMenu
                currentMenuItem={1}
            />
            <div className="communication-content">
                <div className="chats">
                    <div className="chats__my-chats">
                        <GroupChatList
                            meId={me?.id}
                            t={t}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={chatsHidden.group}
                            toggleChatsHidden={() => toggleChatsHidden("group")}
                            setShowCreateGroupChat={setShowCreateGroupChat}
                        />
                        <PersonalChatList
                            meId={me?.id}
                            t={t}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={chatsHidden.personal}
                            toggleChatsHidden={() => toggleChatsHidden("personal")}
                        />
                    </div>
                    {selectedChat.type === "group"
                        ? <GroupChat
                            chat={selectedChat.chat}
                            me={me}
                            setSelectedChat={setSelectedChat}
                        />
                        : selectedChat.type === "personal"
                            ? <PersonalChat
                                chat={selectedChat.chat}
                                me={me}
                                setSelectedChat={setSelectedChat}
                                companionId={selectedChat.chat.initiatorId === me?.id ? selectedChat.chat.companionId : selectedChat.chat.initiatorId}
                            />
                            : <div className="select-chat">
                                {t("SelectChat")} <span onClick={() => setShowCreateGroupChat(true)}>{t("Create")}</span> {t("NewChat")}
                            </div>
                    }
                </div>
            </div>
        </>
    );
}

export default memo(Chats);