import { faArrowDown, faArrowUp, faCircleXmark, faMagnifyingGlassChart, faTriangleExclamation, faSpinner } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useLazyFindGroupChatUserByUserIdQuery } from '../../store/api/chat/GroupChatUser.api';
import { useLazyGetByUserIdAsyncQuery } from '../../store/api/chat/PersonalChat.api';
import CombatLogItemDiscussion from './CombatLogItemDiscussion';
import CombatLogGroupChatList from './CombatLogGroupChatList';
import PersonalChatList from './PersonalChatList';

const CombatLogItem = ({ log, isAuth }) => {
    const { t } = useTranslation("combatDetails/mainInformation");

    const navigate = useNavigate();

    const me = useSelector((state) => state.customer.value);
    const [getPersonalChatsByUserIdAsync] = useLazyGetByUserIdAsyncQuery();
    const [getGroupChatsUserAsync] = useLazyFindGroupChatUserByUserIdQuery();

    const [showChats, setShowChats] = useState(false);
    const [showGroupChats, setShowGroupChats] = useState(true);
    const [showPersonalChats, setShowPersonalChats] = useState(true);
    const [personalChats, setPersonalChats] = useState([]);
    const [groupChatsUser, setGroupChatsUser] = useState([]);

    const getChatsByUserIdAsync = async () => {
        if (!isAuth) {
            return;
        }

        setShowChats(true);

        const personalChats = await getPersonalChatsByUserIdAsync(me?.id);
        if (personalChats.data !== undefined) {
            setPersonalChats(personalChats.data);
        }

        const groupChats = await getGroupChatsUserAsync(me?.id);
        if (groupChats.data !== undefined) {
            setGroupChatsUser(groupChats.data);
        }
    }

    return (
        <div className="card">
            <ul className="list-group list-group-flush">
                <li className="list-group-item title">
                    <div className="title__main">
                        {log.combatsInQueue > 0 &&
                            <>
                                <FontAwesomeIcon
                                    icon={faSpinner}
                                    title={t("Uploading")}
                                />
                                <div>{log.numberReadyCombats} / {log.combatsInQueue}</div>
                            </>
                        }
                        <div>{log.name}</div>
                    </div>
                    <div className="actions">
                        {!isAuth &&
                            <FontAwesomeIcon
                                icon={faTriangleExclamation}
                                className="authorization"
                                title={t("ShouldAuthorize")}
                            />
                        }
                        <CombatLogItemDiscussion
                            isAuth={isAuth}
                            getChatsByUserIdAsync={getChatsByUserIdAsync}
                        />
                    </div>
                </li>
                <li className="list-group-item">{format(new Date(log.date), 'MM/dd/yyyy HH:mm')}</li>
            </ul>
            <div className="card-body">
                {log.numberReadyCombats > 0 &&
                    <div className="btn-shadow" onClick={() => navigate(`/general-analysis?id=${log.id}`)}>
                        <FontAwesomeIcon
                            icon={faMagnifyingGlassChart}
                        />
                        <div>{t("Analyzing")}</div>
                    </div>
                }
            </div>
            {showChats &&
                <div className="chat-list">
                    <div className="chat-list__close">
                        <FontAwesomeIcon
                            icon={faCircleXmark}
                            onClick={() => setShowChats(false)}
                            title={t("Close")}
                        />
                    </div>
                    <div>{t("Chats")}</div>
                    <div className="chat-list__chats">
                        <div className="title">
                            <div className="name">{t("GroupChats")}</div>
                            {showGroupChats
                                ? <FontAwesomeIcon
                                    icon={faArrowUp}
                                    onClick={() => setShowGroupChats(false)}
                                    title={t("HideChats")}
                                />
                                : <FontAwesomeIcon
                                    icon={faArrowDown}
                                    onClick={() => setShowGroupChats(true)}
                                    title={t("ShowChats")}
                                />
                            }
                        </div>
                        {showGroupChats &&
                            <ul>
                                {groupChatsUser?.map((item) => (
                                        <li key={item.id} className="chat-list__group-chats">
                                            <CombatLogGroupChatList
                                                log={log}
                                                chatId={item.chatId}
                                            />
                                        </li>
                                    ))
                                }
                            </ul>
                        }
                        <div className="title">
                            <div className="name">{t("PersonalChats")}</div>
                            {showPersonalChats
                                ? <FontAwesomeIcon
                                    icon={faArrowUp}
                                    onClick={() => setShowPersonalChats(false)}
                                    title={t("HideChats")}
                                />
                                : <FontAwesomeIcon
                                    icon={faArrowDown}
                                    onClick={() => setShowPersonalChats(true)}
                                    title={t("ShowChats")}
                                />
                            }
                        </div>
                        {showPersonalChats &&
                            <ul>
                                {personalChats?.map((item) => (
                                        <li key={item.id} className="chat-list__personal-chats">
                                            <PersonalChatList
                                                log={log}
                                                chat={item}
                                                companionId={item.initiatorId === me?.id ? item.companionId : item.initiatorId}
                                            />
                                        </li>
                                    ))
                                }
                            </ul>
                        }
                    </div>
                    <input type="button" value={t("Close")} className="btn btn-light" onClick={() => setShowChats(false)} />
                </div>
            }
        </div>
    );
}

export default CombatLogItem;