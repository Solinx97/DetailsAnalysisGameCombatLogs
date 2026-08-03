import { faArrowDown, faArrowUp, faCircleXmark, faMagnifyingGlassChart, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { format } from 'date-fns';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { CombatLogModel } from '../types/CombatLogModel';
import CombatLogItemDiscussion from './CombatLogItemDiscussion';
import CombatLogItemActions from './CombatLogItemActions';

interface CombatLogItemProps {
    t: (key: string) => string;
    appUserId: string;
    log: CombatLogModel;
    isAuth: boolean;
}

const GameCombatLogItem: React.FC<CombatLogItemProps> = ({ t, appUserId, log, isAuth }) => {
    const navigate = useNavigate();

    const [showChats, setShowChats] = useState(false);
    const [showGroupChats, setShowGroupChats] = useState(true);
    const [showPersonalChats, setShowPersonalChats] = useState(true);

    return (
        <div className="card">
            <ul className="list-group list-group-flush">
                <li className="list-group-item title">
                    <div className="title__main">
                        <div>{log.name}</div>
                    </div>
                    <div className="actions">
                        <div className="actions__communication">
                            {!isAuth &&
                                <FontAwesomeIcon
                                    icon={faTriangleExclamation}
                                    className="authorization"
                                    title={t("ShouldAuthorize")}
                                />
                            }
                            <CombatLogItemDiscussion
                                t={t}
                            />
                        </div>
                        {(appUserId === log.appUserId) &&
                            <CombatLogItemActions
                                t={t}
                                combatLogId={log.id}
                            />
                        }
                    </div>
                </li>
                <li className="list-group-item">{format(new Date(log.date), 'MM/dd/yyyy HH:mm')}</li>
            </ul>
            <div className="card-body">
                <div className="btn-shadow" onClick={() => navigate(`/general-analysis?id=${log.id}`)}>
                    <FontAwesomeIcon
                        icon={faMagnifyingGlassChart}
                    />
                    <div>{t("Analyzing")}</div>
                </div>
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
                            <FontAwesomeIcon
                                icon={showGroupChats ? faArrowUp : faArrowDown}
                                onClick={() => setShowGroupChats(!showGroupChats)}
                                title={showGroupChats ? t("HideChats") : t("ShowChats")}
                            />
                        </div>
                        <div className="title">
                            <div className="name">{t("PersonalChats")}</div>
                            <FontAwesomeIcon
                                icon={showPersonalChats ? faArrowUp : faArrowDown}
                                onClick={() => setShowPersonalChats(!showPersonalChats)}
                                title={showPersonalChats ? t("HideChats") : t("ShowChats")}
                            />
                        </div>
                    </div>
                    <input type="button" value={t("Close")} className="btn btn-light" onClick={() => setShowChats(false)} />
                </div>
            }
        </div>
    );
}

export default GameCombatLogItem;