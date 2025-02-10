import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { memo, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { SelectedChat } from '../../../types/components/communication/Chat';
import Loading from '../../Loading';
import CommunicationMenu from '../CommunicationMenu';
import CreateGroupChat from '../create/CreateGroupChat';
import GroupChat from './GroupChat';
import GroupChatList from './GroupChatList';
import PersonalChat from './PersonalChat';
import PersonalChatList from './PersonalChatList';

import "../../../styles/communication/chats/chats.scss";

const Chats: React.FC = () => {
    const { t } = useTranslation("communication/chats/chats");

    const me = useSelector((state: any) => state.user.value);

    const [selectedChat, setSelectedChat] = useState<SelectedChat>({ type: null, chat: null });
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [showCreateGroupChat, setShowCreateGroupChat] = useState(false);

    const maxWidth = 425;
    const screenSize = useMemo(() => ({
        width: window.innerWidth,
        height: window.innerHeight
    }), []);

    if (!me) {
        return (
            <>
                <CommunicationMenu
                    currentMenuItem={1}
                    hasSubMenu={false}
                />
                <Loading />
            </>
        );
    }

    if (selectedChat.type !== null && screenSize.width <= maxWidth) {
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
                                onClick={() => setSelectedChat({ type: null, chat: null })}
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
                    <div className="chats__my-chats">
                        <GroupChatList
                            meId={me?.id}
                            t={t}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={groupChatsHidden}
                            toggleChatsHidden={() => setGroupChatsHidden(prev => !prev)}
                            setShowCreateGroupChat={setShowCreateGroupChat}
                        />
                        <PersonalChatList
                            meId={me?.id}
                            t={t}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={personalChatsHidden}
                            toggleChatsHidden={() => setPersonalChatsHidden(prev => !prev)}
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
            <CommunicationMenu
                currentMenuItem={1}
                hasSubMenu={false}
            />
        </>
    );
}

export default memo(Chats);