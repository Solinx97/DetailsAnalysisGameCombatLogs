import React, { useCallback, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { NavLink } from 'react-router-dom';
import { useFindGroupChatUserByUserIdQuery } from '../../../store/api/communication/chats/GroupChatUser.api';
import { useGetByUserIdAsyncQuery } from '../../../store/api/communication/chats/PersonalChat.api';
import Loading from '../../Loading';
import CommunicationMenu from '../CommunicationMenu';
import GroupChat from './GroupChat';
import GroupChatList from './GroupChatList';
import PersonalChat from './PersonalChat';
import PersonalChatList from './PersonalChatList';

import "../../../styles/communication/chats/chats.scss";

const pollingInterval = 3000;

const Chats = () => {
    const { t } = useTranslation("communication/chats/chats");

    const me = useSelector((state) => state.user.value);

    const [selectedChat, setSelectedChat] = useState({ type: null, chat: null });
    const [chatsHidden, setChatsHidden] = useState({ group: false, personal: false });
    const [skipFetching, setSkipFetching] = useState(true);

    const { data: personalChats, isError: personalChatError, isLoading: personalChatLoading } = useGetByUserIdAsyncQuery(me?.id, {
        pollingInterval,
        skip: skipFetching
    });
    const { data: groupChatUsers, isError: groupChatError, isLoading: groupChatLoading } = useFindGroupChatUserByUserIdQuery(me?.id, {
        pollingInterval,
        skip: skipFetching
    });

    useEffect(() => {
        me !== null ? setSkipFetching(false) : setSkipFetching(true);
    }, [me]);

    const toggleChatsHidden = useCallback((type) => {
        setChatsHidden(prevState => ({ ...prevState, [type]: !prevState[type] }));
    }, []);

    const isLoading = personalChatLoading || groupChatLoading;
    const isError = personalChatError || groupChatError;

    if (isLoading || isError || me === null || personalChats === null || groupChatUsers === null) {
        return (
            <>
                <CommunicationMenu currentMenuItem={1} />
                <Loading />
            </>
        );
    }

    return (
        <>
            <CommunicationMenu
                currentMenuItem={1}
            />
            <div className="communication__content">
                <div className="chats">
                    <div className="chats__my-chats">
                        <GroupChatList
                            t={t}
                            groupChatUsers={groupChatUsers}
                            selectedChat={selectedChat}
                            setSelectedChat={setSelectedChat}
                            chatsHidden={chatsHidden.group}
                            toggleChatsHidden={() => toggleChatsHidden("group")}
                        />
                        <PersonalChatList
                            meId={me?.id}
                            t={t}
                            personalChats={personalChats}
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
                                {t("SelectChat")} <NavLink to="/chats/create">{t("Create")}</NavLink> {t("NewChat")}
                            </div>
                    }
                </div>
            </div>
        </>
    );
}

export default Chats;