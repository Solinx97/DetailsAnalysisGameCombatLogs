import { faArrowDown, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useFindGroupChatUserByUserIdQuery } from '../../../store/api/communication/chats/GroupChatUser.api';
import { useGetByUserIdAsyncQuery } from '../../../store/api/communication/chats/PersonalChat.api';
import Communication from '../Communication';
import GroupChat from './GroupChat';
import MyGroupChat from './MyGroupChat';
import MyPersonalChat from './MyPersonalChat';
import PersonalChat from './PersonalChat';

import "../../../styles/communication/chats/chats.scss";

const getGroupChatUsersInterval = 2000;

const Chats = () => {
    const { t } = useTranslation("communication/chats/chats");

    const me = useSelector((state) => state.customer.value);

    const { data: personalChats, isLoading } = useGetByUserIdAsyncQuery(me?.id);
    const { data: groupChatUsers, isLoading: chatUserIsLoading } = useFindGroupChatUserByUserIdQuery(me?.id, {
        pollingInterval: getGroupChatUsersInterval
    });

    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [selectedPersonalChat, setSelectedPersonalChat] = useState(null);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);

    useEffect(() => {
        if (selectedGroupChat !== null) {
            setSelectedPersonalChat(null);
        }
    }, [selectedGroupChat]);

    useEffect(() => {
        if (selectedPersonalChat !== null) {
            setSelectedGroupChat(null);
        }
    }, [selectedPersonalChat]);

    if (isLoading || chatUserIsLoading) {
        return <></>;
    }

    return (
        <>
            <Communication
                currentMenuItem={1}
            />
            <div className="communication__content">
                <div className="chats">
                    <div className="chats__my-chats">
                        <div className="chats__my-chats_title">
                            <div>{t("GroupChats")}</div>
                            {groupChatsHidden
                                ? <FontAwesomeIcon
                                    icon={faArrowDown}
                                    title={t("ShowChats")}
                                    onClick={() => setGroupChatsHidden(!groupChatsHidden)}
                                />
                                : <FontAwesomeIcon
                                    icon={faArrowUp}
                                    title={t("HideChats")}
                                    onClick={() => setGroupChatsHidden(!groupChatsHidden)}
                                />
                            }
                        </div>
                        <ul className={`group-chats${!groupChatsHidden ? "_active" : ""}`}>
                            {
                                groupChatUsers?.map((item) => (
                                    <li key={item.id} className={selectedGroupChat?.id === item?.groupChatId ? `selected` : ``}>
                                        <MyGroupChat
                                            groupChatId={item.groupChatId}
                                            setSelectedGroupChat={setSelectedGroupChat}
                                            meId={me?.id}
                                        />
                                    </li>
                                ))
                            }
                        </ul>
                        <div className="chats__my-chats_title">
                            <div>{t("PersonalChats")}</div>
                            {personalChatsHidden
                                ? <FontAwesomeIcon
                                    icon={faArrowDown}
                                    title={t("ShowChats")}
                                    onClick={() => setPersonalChatsHidden((item) => !item)}
                                />
                                : <FontAwesomeIcon
                                    icon={faArrowUp}
                                    title={t("HideChats")}
                                    onClick={() => setPersonalChatsHidden((item) => !item)}
                                />
                            }
                        </div>
                        <ul className={`personal-chats${!personalChatsHidden ? "_active" : ""}`}>
                            {
                                personalChats?.map((item) => (
                                    <li key={item.id} className={selectedPersonalChat?.id === item?.id ? `selected` : ``}>
                                        <MyPersonalChat
                                            personalChat={item}
                                            selectedGroupChatId={selectedPersonalChat?.id}
                                            setSelectedPersonalChat={setSelectedPersonalChat}
                                            companionId={item.initiatorId === me?.id ? item.companionId : item.initiatorId}
                                            meId={me?.id}
                                        />
                                    </li>
                                ))
                            }
                        </ul>
                    </div>
                    {(selectedPersonalChat === null && selectedGroupChat === null)
                        ? <div className="select-chat">{t("SelectChat")}</div>
                        : selectedGroupChat !== null
                            ? <GroupChat
                                chat={Object.assign({}, selectedGroupChat)}
                                me={me}
                                setSelectedChat={setSelectedGroupChat}
                            />
                            : selectedPersonalChat !== null &&
                            <PersonalChat
                                chat={Object.assign({}, selectedPersonalChat)}
                                me={me}
                                setSelectedChat={setSelectedPersonalChat}
                                companionId={selectedPersonalChat.initiatorId === me?.id ? selectedPersonalChat.companionId : selectedPersonalChat.initiatorId}
                            />
                    }
                </div>
            </div>
        </>
    );
}

export default Chats;