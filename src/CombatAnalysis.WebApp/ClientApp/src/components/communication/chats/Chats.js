import { faArrowLeft, faBars } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as signalR from '@microsoft/signalr';
import React, { memo, useCallback, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { useFindGroupChatUserByUserIdQuery } from '../../../store/api/chat/GroupChatUser.api';
import { useGetByUserIdAsyncQuery } from '../../../store/api/chat/PersonalChat.api';
import Loading from '../../Loading';
import CommunicationMenu from '../CommunicationMenu';
import GroupChat from './GroupChat';
import GroupChatList from './GroupChatList';
import PersonalChat from './PersonalChat';
import PersonalChatList from './PersonalChatList';

import "../../../styles/communication/chats/chats.scss";

const Chats = () => {
    const { t } = useTranslation("communication/chats/chats");

    const me = useSelector((state) => state.user.value);

    const personalHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_PERSONAL_CHAT_UNREAD_MESSAGES_ADDRESS}`;
    const groupHubURL = `${process.env.REACT_APP_HUBS_URL}${process.env.REACT_APP_HUBS_GROUP_CHAT_UNREAD_MESSAGES_ADDRESS}`;

    const [personalHubConnection, setPersonalHubConnection] = useState(null);
    const [groupHubConnection, setGroupHubConnection] = useState(null);

    const [selectedChat, setSelectedChat] = useState({ type: null, chat: null });
    const [chatsHidden, setChatsHidden] = useState({ group: false, personal: false });
    const [showMenu, setShowMenu] = useState(false);

    const { data: personalChats, isError: personalChatError, isLoading: personalChatLoading } = useGetByUserIdAsyncQuery(me?.id);
    const { data: meInGroupChats, isError: groupChatError, isLoading: groupChatLoading } = useFindGroupChatUserByUserIdQuery(me?.id);

    const maxWidth = 425;
    const screenSize = useMemo(() => ({
        width: window.innerWidth,
        height: window.innerHeight
    }), []);

    useEffect(() => {
        if (!personalChats) {
            return;
        }

        const connectToChat = async () => {
            await connectToPersonalChatAsync();
        }

        connectToChat();
    }, [personalChats]);

    useEffect(() => {
        if (!meInGroupChats) {
            return;
        }

        const connectToChat = async () => {
            await connectToGroupChatAsync();
        }

        connectToChat();
    }, [meInGroupChats]);

    const connectToPersonalChatAsync = async () => {
        if (personalHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(personalHubURL)
                .withAutomaticReconnect()
                .build();
            setPersonalHubConnection(hubConnection);

            await hubConnection.start();

            for (let i = 0; i < personalChats?.length; i++) {
                await hubConnection.invoke("JoinRoom", personalChats[i].id);
            }
        } catch (e) {
            console.log(e);
        }
    }

    const connectToGroupChatAsync = async () => {
        if (groupHubConnection !== null) {
            return;
        }

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl(groupHubURL)
                .withAutomaticReconnect()
                .build();
            setGroupHubConnection(hubConnection);

            await hubConnection.start();

            for (let i = 0; i < meInGroupChats?.length; i++) {
                await hubConnection.invoke("JoinRoom", meInGroupChats[i].chatId);
            }
        } catch (e) {
            console.log(e);
        }
    }

    const toggleChatsHidden = useCallback((type) => {
        setChatsHidden(prevState => ({ ...prevState, [type]: !prevState[type] }));
    }, []);

    const isLoading = personalChatLoading || groupChatLoading;
    const isError = personalChatError || groupChatError;

    if (isLoading || isError || me === null || personalChats === null || meInGroupChats === null) {
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
                                unreadMessageHubConnection={groupHubConnection}
                            />
                            : selectedChat.type === "personal"
                                ? <PersonalChat
                                    chat={selectedChat.chat}
                                    me={me}
                                    setSelectedChat={setSelectedChat}
                                    companionId={selectedChat.chat.initiatorId === me?.id ? selectedChat.chat.companionId : selectedChat.chat.initiatorId}
                                    unreadMessageHubConnection={personalHubConnection}
                                />
                                : <div className="select-chat">
                                    {t("SelectChat")} <NavLink to="/chats/create">{t("Create")}</NavLink> {t("NewChat")}
                                </div>
                        }
                    </div>
                </div>
            </>
        );
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={1}
            />
            <div className="communication-content">
                <div className="chats">
                    <div className="chats__my-chats">
                        <GroupChatList
                            t={t}
                            meInGroupChats={meInGroupChats}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={chatsHidden.group}
                            toggleChatsHidden={() => toggleChatsHidden("group")}
                            hubConnection={groupHubConnection}
                        />
                        <PersonalChatList
                            meId={me?.id}
                            t={t}
                            personalChats={personalChats}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={chatsHidden.personal}
                            toggleChatsHidden={() => toggleChatsHidden("personal")}
                            hubConnection={personalHubConnection}
                        />
                    </div>
                    {selectedChat.type === "group"
                        ? <GroupChat
                            chat={selectedChat.chat}
                            me={me}
                            setSelectedChat={setSelectedChat}
                            unreadMessageHubConnection={groupHubConnection}
                        />
                        : selectedChat.type === "personal"
                            ? <PersonalChat
                                chat={selectedChat.chat}
                                me={me}
                                setSelectedChat={setSelectedChat}
                                companionId={selectedChat.chat.initiatorId === me?.id ? selectedChat.chat.companionId : selectedChat.chat.initiatorId}
                                unreadMessageHubConnection={personalHubConnection}
                            />
                            : <div className="select-chat">
                                {t("SelectChat")} <NavLink to="/chats/create">{t("Create")}</NavLink> {t("NewChat")}
                            </div>
                    }
                </div>
            </div>
        </>
    );
}

export default memo(Chats);