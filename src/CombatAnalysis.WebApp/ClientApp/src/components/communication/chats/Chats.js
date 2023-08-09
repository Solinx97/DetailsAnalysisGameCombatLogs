import { faArrowDown, faArrowUp, faSquarePlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useGetGroupChatUserByUserIdQuery, useGetPersonalChatsByUserIdQuery } from '../../../store/api/ChatApi';
import Communication from '../Communication';
import CreateGroupChat from './CreateGroupChat';
import GroupChat from './GroupChat';
import MyGroupChat from './MyGroupChat';
import MyPersonalChat from './MyPersonalChat';
import PersonalChat from './PersonalChat';

import "../../../styles/communication/chats/chats.scss";

const getGroupChatUsersInterval = 2000;

const Chats = () => {
    const { t } = useTranslation("communication/chats/chats");

    const customer = useSelector((state) => state.customer.value);

    const { data: personalChats, isLoading } = useGetPersonalChatsByUserIdQuery(customer?.id);
    const { data: groupChatUsers, isLoading: chatUserIsLoading } = useGetGroupChatUserByUserIdQuery(customer?.id, {
        pollingInterval: getGroupChatUsersInterval
    });

    const [selectedGroupChat, setSelectedGroupChat] = useState(null);
    const [selectedPersonalChat, setSelectedPersonalChat] = useState(null);
    const [groupChatsHidden, setGroupChatsHidden] = useState(false);
    const [personalChatsHidden, setPersonalChatsHidden] = useState(false);
    const [createGroupChatIsActive, setCreateGroupChatIsActive] = useState(false);

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
        <div className="communication">
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
                            <FontAwesomeIcon
                                className="create"
                                icon={faSquarePlus}
                                title={t("CreateNewGroupChat")}
                                onClick={() => setCreateGroupChatIsActive(!createGroupChatIsActive)}
                            />
                        </div>
                        <ul className="chats__my-chats__group-chats">
                            {
                                groupChatUsers?.map((item) => (
                                    <li key={item.id} className={selectedGroupChat?.id === item?.groupChatId ? `active` : ``}>
                                        <MyGroupChat
                                            groupChatId={item.groupChatId}
                                            setSelectedGroupChat={setSelectedGroupChat}
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
                        <ul className="chats__my-chats__personal-chats">
                            {
                                personalChats?.map((item) => (
                                    <li key={item.id} className={selectedPersonalChat?.id === item?.id ? `active` : ``}>
                                        <MyPersonalChat
                                            personalChat={item}
                                            selectedGroupChatId={selectedPersonalChat?.id}
                                            setSelectedPersonalChat={setSelectedPersonalChat}
                                            companionId={item.initiatorId === customer?.id ? item.companionId : item.initiatorId}
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
                                customer={customer}
                                setSelectedChat={setSelectedGroupChat}
                            />
                            : selectedPersonalChat !== null &&
                            <PersonalChat
                                chat={Object.assign({}, selectedPersonalChat)}
                                customer={customer}
                                setSelectedChat={setSelectedPersonalChat}
                                companionId={selectedPersonalChat.initiatorId === customer?.id ? selectedPersonalChat.companionId : selectedPersonalChat.initiatorId}
                            />
                    }
                </div>
                <CreateGroupChat
                    setCreateGroupChatIsActive={setCreateGroupChatIsActive}
                    customer={customer}
                    createGroupChatIsActive={createGroupChatIsActive}
                />
            </div>
        </div>
    );
}

export default Chats;